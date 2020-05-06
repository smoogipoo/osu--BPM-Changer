using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Timers;
using System.Windows.Forms;
using FsBeatmapProcessor;
using Timer = System.Timers.Timer;

namespace osu_trainer.Controls
{
    public class StarsDisplay : Control
    {
        private float _stars;
        private float _targetStars;

        public float Stars
        {
            get => _stars;
            set
            {
                _targetStars = value;
                _timer.Start();
                Invalidate(false);
            }
        }

        private GameMode gameMode = GameMode.osu;

        public GameMode GameMode
        {
            get => gameMode;
            set
            {
                updateIcon(gameMode = value, Colors.GetDifficultyColor(_stars));
                Invalidate(false);
            }
        }

        private Image _icon;

        private const float AnimationSpeed = .125f;
        private Timer _timer;

        private Color _lastColor = Color.Transparent;
        private GameMode _lastMode = GameMode.CatchtheBeat;

        public StarsDisplay()
        {
            DoubleBuffered = true;

            _timer = new Timer()
            {
                Interval = 33,
                Enabled = true
            };
            _timer.Elapsed += TimerOnElapsed;

            updateIcon(GameMode.osu, Colors.GetDifficultyColor(0));
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (_stars == _targetStars)
                return;

            if (_targetStars > _stars)
                _stars += (_targetStars - _stars) * AnimationSpeed;
            else if (_targetStars < _stars)
                _stars -= (_stars - _targetStars) * AnimationSpeed;

            updateIcon(gameMode, Colors.GetDifficultyColor(_stars));

            Invalidate(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            e.Graphics.Clear(Parent.BackColor);

            var x = Width;

            if (_icon != null)
            {
                var size = 16;

                x -= size;
                e.Graphics.DrawImage(_icon, x - 1, (Height / 2) - (size / 2) - 1, size, size);
                x -= 4;
            }

            var difficultyColor = Colors.GetDifficultyColor(Stars);

            using (var textBrush = new SolidBrush(difficultyColor))
            {
                var text = $"{Stars:0.00}";
                var rectangle = new RectangleF(4, 3, x - 4, Height - 3);
                var format = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Center,
                };

                e.Graphics.DrawString(text, Font, textBrush, rectangle, format);
            }
        }

        private void updateIcon(GameMode mode, Color color)
        {
            if (mode == _lastMode && color == _lastColor)
                return;

            if (_icon == null)
            {
                _icon = Icons.GenerateIcon(mode, color);
            }
            else
            {
                lock (_icon)
                {
                    _icon = Icons.GenerateIcon(mode, color);
                }
            }
        }
    }
}