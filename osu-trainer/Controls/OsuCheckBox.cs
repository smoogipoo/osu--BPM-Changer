using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace osu_trainer.Controls
{
    public class OsuCheckBox : CheckBox
    {
        public Color DisabledColor { get; set; } = Colors.Disabled;

        private bool _hover;
        private bool _down;

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hover = true;
            Invalidate(false);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            Invalidate(false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Parent.BackColor);

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            var hoverPadding = 2.5f;
            var penWidth = 3;
            var indicatorWidth = 25;
            var indicatorRectangle = new RectangleF(
                Width - indicatorWidth - penWidth - (hoverPadding * 2),
                penWidth + hoverPadding,
                indicatorWidth - penWidth,
                15 - penWidth);

            var format = new StringFormat()
            {
                LineAlignment = StringAlignment.Center
            };
            var textOffset = Font.GetHeight() / 10;
            var textRectangle = new RectangleF(0, textOffset, indicatorRectangle.X - 2, Height + textOffset);
            e.Graphics.DrawString(Text, Font, Brushes.White, textRectangle, format);

            var mainColor = Enabled ? Color.FromArgb(255, 103, 171) : DisabledColor;
            
            if (_hover)
            {
                var offsetRectangle = new RectangleF(
                    indicatorRectangle.X - hoverPadding,
                    indicatorRectangle.Y - hoverPadding,
                    indicatorRectangle.Width + (hoverPadding * 2),
                    indicatorRectangle.Height + (hoverPadding * 2));

                using (var path = JunUtils.RoundedRect(offsetRectangle, 8))
                using (var pinkBrush = new SolidBrush(mainColor))
                {
                    e.Graphics.FillPath(pinkBrush, path);
                }
            }

            using (var path = JunUtils.RoundedRect(indicatorRectangle, 6))
            {
                var fillRectangle = new Rectangle(Width - indicatorWidth, 0, indicatorWidth, 15);
                var color = _hover ? Color.FromArgb(192, 255, 255, 255) : mainColor;

                using (var pen = new Pen(color, 3))
                {
                    e.Graphics.DrawPath(pen, path);
                }

                using (var brush = new SolidBrush(color))
                {
                    if (Checked)
                    {
                        e.Graphics.FillPath(brush, path);
                    }
                }
            }
        }
    }
}