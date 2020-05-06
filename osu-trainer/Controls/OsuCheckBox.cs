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

            var hoverPadding = 4f;
            var penWidth = 2;
            var indicatorWidth = 25;
            var indicatorRectangle = new RectangleF(
                Width - indicatorWidth - penWidth - (hoverPadding * 2),
                penWidth + hoverPadding,
                indicatorWidth - penWidth,
                13 - penWidth);

            var format = new StringFormat()
            {
                LineAlignment = StringAlignment.Center
            };
            var textOffset = Font.GetHeight() / 10;
            var textRectangle = new RectangleF(0, textOffset, indicatorRectangle.X - 2, Height + textOffset);
            e.Graphics.DrawString(Text, Font, new SolidBrush(ForeColor), textRectangle, format);

            var checkedBorderColor   = !Enabled ? DisabledColor : _hover ? Color.FromArgb(255, 221, 238)     : Color.FromArgb(255, 102, 170);
            var checkedFillColor     = !Enabled ? DisabledColor : _hover ? Color.FromArgb(255, 221, 238)     : Color.FromArgb(255, 102, 170);
            var uncheckedBorderColor = !Enabled ? DisabledColor : _hover ? Color.FromArgb(255, 221, 238)     : Color.FromArgb(255, 102, 170);
            var uncheckedFillColor   = !Enabled ? DisabledColor : _hover ? Color.FromArgb(172, 192, 22, 123) : Color.FromArgb(0, 0, 0, 0);

            using (var path = JunUtils.RoundedRect(indicatorRectangle, 5))
            {
                var fillRectangle = new Rectangle(Width - indicatorWidth, 0, indicatorWidth, 15);

                // inner fill
                using (var innerBrush = new SolidBrush(Checked ? checkedFillColor : uncheckedFillColor))
                {
                    e.Graphics.FillPath(innerBrush, path);
                }

                // outer border
                using (var pen = new Pen(Checked ? checkedBorderColor : uncheckedBorderColor, penWidth))
                {
                    e.Graphics.DrawPath(pen, path);
                }

            }
        }
    }
}