using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace osu_trainer.Controls
{
    // TODO: Smooth transition between back colors.
    public class OsuButton : Button, IButtonControl
    {
        private readonly Timer _timer;
        private readonly Random _random;

        private List<Triangle> _triangles;
        private SolidBrush[] _brushes;
        private SolidBrush[] _progressBrushes;

        private float _size = 1f;
        private bool _isDown = false;
        private const float _downSize = 0.95f;

        private bool _isHover = false;
        private int _opacity = 0;
        private const int _hoverOpacity = 48;

        private static int _count = 0;

        private static readonly StringFormat _format = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };
        private static readonly StringFormat _formatSubtext = new StringFormat()
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Near
        };

        private static readonly Brush _shadowBrush = new SolidBrush(Color.FromArgb(64, 0, 0, 0));

        [Description("How dark and light the colors based of the " + nameof(Color) + " property should be.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public float BrightnessRange { get; set; } = 0.01f;

        [Description("How many triangles should be visible at all times.")]
        [RefreshProperties(RefreshProperties.Repaint)]
        public int TriangleCount { get; set; } = 30;

        [RefreshProperties(RefreshProperties.Repaint)]
        public string Subtext { get; set; } = "";

        private Color _color = Color.Transparent;

        [RefreshProperties(RefreshProperties.Repaint)]
        public Color Color
        {
            get => _color;
            set => _brushes = GenerateColors(_color = value);
        }

        [
            RefreshProperties(RefreshProperties.Repaint),
            Description("Adjusts the vertical position of the text")
        ]
        public int TextYOffset { get; set; } = 0;

        [RefreshProperties(RefreshProperties.Repaint)]
        public Color SubtextColor { get; set; }

        private Color _progressColor = Color.Transparent;

        [RefreshProperties(RefreshProperties.Repaint)]
        public Color ProgressColor
        {
            get => _progressColor;
            set => _progressBrushes = GenerateColors(_progressColor = value);
        }

        private float progress;

        public float Progress
        {
            get => progress;
            set
            {
                progress = value;
                Invalidate(false);
            }
        }

        public override Color BackColor => Color.Transparent;

        public OsuButton()
        {
            _random = new Random((int)DateTime.Now.Ticks + _count++);

            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer | ControlStyles.UserPaint, true);

            if (Color == Color.Transparent)
                Color = Color.CornflowerBlue;

            ResetTriangles();

            if (!DesignMode)
            {
                _timer = new Timer()
                {
                    Enabled = true,
                    Interval = 1000 / 60,
                };

                _timer.Tick += Timer_Tick;
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Animate button canvas size
            if (_isDown && _size > _downSize)
                _size -= 0.005f;
            else if (!_isDown && _size < 1)
                _size += 0.025f;
            else
                _size = _isDown ? _downSize : 1f;

            // Animate hover effect opacity
            if (_isHover && _opacity < _hoverOpacity)
                _opacity += 16;
            else if (!_isHover && 0 < _opacity)
                _opacity -= 5;

            // Process triangles
            foreach (var item in _triangles)
            {
                if (item.Y < 0)
                    ResetTriangle(item, false);
                else
                    item.Y -= item.Speed;
            }

            // Refresh button to draw frame
            this.Invalidate(true);
        }

        private SolidBrush[] GenerateColors(Color baseColor)
        {
            var light = ControlPaint.Light(baseColor, BrightnessRange);
            var dark = ControlPaint.Dark(baseColor, BrightnessRange);

            var brushes = new SolidBrush[25];
            var total = brushes.Length;

            for (int i = 0; i < total; i++)
            {
                var rAverage = dark.R + ((light.R - dark.R) * i / total);
                var gAverage = dark.G + ((light.G - dark.G) * i / total);
                var bAverage = dark.B + ((light.B - dark.B) * i / total);

                var color = Color.FromArgb(rAverage, gAverage, bAverage);
                brushes[i] = new SolidBrush(color);
            }

            return brushes;
        }

        private void ResetTriangles()
        {
            _triangles = new List<Triangle>(TriangleCount);

            for (int i = 0; i < _triangles.Capacity; i++)
            {
                var triangle = new Triangle();
                ResetTriangle(triangle, true);

                _triangles.Add(triangle);
            }
        }

        private int GetRandomShade() => _random.Next(_brushes.Length - 1);

        private float GetRandomSize() => _random.Next(20, 60);

        private float GetRandomSpeed() => (float)(Math.Max(_random.NextDouble(), .1f) * .01f);

        /// <summary>
        /// Generates default values for this triangle.
        /// </summary>
        /// <param name="initialStart">
        /// Whether the triangle should start randomly in the Y-axis or rather start from the beginning.
        /// </param>
        private void ResetTriangle(Triangle triangle, bool initialStart)
        {
            triangle.X = (float)_random.NextDouble();
            triangle.Y = initialStart ? (float)_random.NextDouble() : 1f;
            triangle.Size = GetRandomSize();
            triangle.Shade = GetRandomShade();
            triangle.Speed = GetRandomSpeed();
        }

        #region Events

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);

            _isHover = true;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            _isHover = false;
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Left)
            {
                _isDown = true;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                _isDown = false;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(this.Parent.BackColor);

            using (var frame = GetFrame())
            {
                var width = frame.Width * _size;
                var height = frame.Height * _size;

                var x = (this.Width / 2) - (int)(width / 2);
                var y = (this.Height / 2) - (int)(height / 2);
                e.Graphics.DrawImage(frame, x, y, width, height);
            }
        }

        #endregion Events

        private Bitmap GetFrame()
        {
            var frame = new Bitmap(Width, Height);
            var rectangle = new Rectangle(0, 0, Width, Height);
            var progressRectangle = new RectangleF(0, 0, Width * Progress, Height);

            using (var g = Graphics.FromImage(frame))
            using (var path = JunUtils.RoundedRect(rectangle, 4))
            using (var progressPath = JunUtils.RoundedRect(progressRectangle, 4))
            {
                g.CompositingMode = CompositingMode.SourceOver;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                g.SetClip(path);
                g.Clear(Color);

                DrawTriangleEffect(g, _brushes);

                if (0f < Progress)
                {
                    var progressBrush = new SolidBrush(_progressColor);
                    g.FillRectangle(progressBrush, progressRectangle);
                    DrawTriangleEffect(g, _progressBrushes, Progress);
                }

                if (0 < _opacity)
                {
                    var color = Color.FromArgb(_opacity, 255, 255, 255);
                    var brush = new SolidBrush(color);
                    g.FillRectangle(brush, rectangle);
                }

                if (Image != null)
                {
                    var x = (Width / 2f) - (Image.Width / 2f);
                    var y = (Height / 2f) - (Image.Height / 2f);
                    g.DrawImage(Image, x, y, Image.Width, Image.Height);
                }
                else if (!string.IsNullOrWhiteSpace(Text))
                {
                    g.DrawString(
                        Text,
                        Font,
                        _shadowBrush,
                        new Rectangle(0, TextYOffset + 1, this.Width, this.Height),
                        _format
                    );
                    g.DrawString(
                        Text,
                        Font,
                        new SolidBrush(Enabled ? ForeColor : Colors.Disabled),
                        new Rectangle(0, TextYOffset, this.Width, this.Height),
                        _format
                    );

                    if (!string.IsNullOrWhiteSpace(Subtext))
                    {
                        int offsetY = 37;
                        g.DrawString(
                            Subtext,
                            new Font(this.Font.FontFamily, 7f),
                            _shadowBrush,
                            new Rectangle(0, TextYOffset + offsetY + 1, this.Width, this.Height),
                            _formatSubtext
                        );
                        g.DrawString(
                            Subtext,
                            new Font(this.Font.FontFamily, 7f),
                            new SolidBrush(Enabled ? SubtextColor : Colors.Disabled),
                            new Rectangle(0, TextYOffset + offsetY + 1, this.Width, this.Height),
                            _formatSubtext
                        );
                    }
                }

            }
            return frame;
        }

        private void DrawTriangleEffect(Graphics graphics, Brush[] brushes, float swipe = 1f)
        {
            var maxX = swipe * Width;

            foreach (var triangle in _triangles)
            {
                var points = triangle.ToPointF(Width, Height);

                //if (maxX <= points[0].X)
                //    continue;

                graphics.FillPolygon(brushes[triangle.Shade], points);
            }
        }

        private class Triangle
        {
            public float X { get; set; }
            public float Y { get; set; }

            public float Speed { get; set; }

            public float Size { get; set; }

            /// <summary>
            /// The index from a (usually one base color) color palette
            /// </summary>
            public int Shade { get; set; }

            public PointF[] ToPointF(int width, int height)
            {
                var x = (X * (width + Size)) - (Size / 2f);
                var y = Y * (height + Size);

                return new PointF[] {
                    new PointF(x              , y),
                    new PointF(x + (Size / 2f), y - Size),
                    new PointF(x +  Size      , y),
                };
            }
        }
    }
}