using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osu_trainer.Controls
{
    public class RatioBar : Control
    {
        private const int BarHeight = 2;
        
        #region Appearance Properties
        
        private Color leftColor;
        [
            Category("Appearance"),
            Description("Sets the color of the left bar"),
        ]
        public Color LeftColor
        {
            get => leftColor;
            set
            {
                leftColor = value;
                leftPen = new Pen(leftColor, BarHeight);
                leftBrush = new SolidBrush(leftColor);
                Invalidate();
            }
        }
        
        private Pen leftPen;
        private SolidBrush leftBrush;

        private Color rightColor;
        [
            Category("Appearance"),
            Description("Sets the color of the right bar"),
        ]
        public Color RightColor
        {
            get => rightColor;
            set
            {
                rightColor = value;
                rightPen = new Pen(rightColor, BarHeight);
                rightBrush = new SolidBrush(rightColor);
                Invalidate();
            }
        }

        private Pen rightPen;
        private SolidBrush rightBrush;
        
        #endregion
        
        #region Customizable Properties
        
        private int leftPercent;
        
        [
            Category("Data"),
            Description("Left percent"),
            DefaultValue(50)
        ]
        public int LeftPercent
        {
            get => leftPercent;
            set
            {
                leftPercent = value;
                Invalidate();
            }
        }

        private string _leftText;

        public string LeftText
        {
            get => _leftText;
            set
            {
                _leftText = value;
                Invalidate(false);
            }
        }
        
        private string _rightText;

        public string RightText
        {
            get => _rightText;
            set
            {
                _rightText = value;
                Invalidate(false);
            }
        }

        #endregion

        public RatioBar()
        {
            DoubleBuffered = true;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            // draw text
            var textHeight = Math.Max(g.MeasureString(LeftText, Font).Height, g.MeasureString(RightText, Font).Height);
            var textY = Height - BarHeight - textHeight;
            var textRectangle = new RectangleF(0, textY, Width, Height - textY);
            
            g.DrawString(LeftText, Font, leftBrush, textRectangle);
            g.DrawString(RightText, Font, rightBrush, textRectangle, new StringFormat()
            {
                Alignment = StringAlignment.Far
            });

            // draw bars
            var barRadius = BarHeight / 2;
            var barY = Height - barRadius;
            var barMiddle = Width * LeftPercent / 100.0f;
            
            g.DrawLine(new Pen(LeftColor, 2), barRadius, barY, barMiddle, barY);
            g.DrawLine(new Pen(RightColor, 2), barMiddle, barY, Width - barRadius, barY);
        }
    }
}
