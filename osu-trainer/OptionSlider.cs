using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osu_trainer
{
    class OptionSlider : Control
    {
        // Customizable parameters



        private decimal minValue;
        [
        Category("Data"),
        Description("Sets the value when the nipple is slid all the way to the left")
        ]
        public decimal MinValue
        {
            get => minValue;
            set
            {
                minValue = value;
                Invalidate();
            }
        }


        private decimal maxValue;
        [
        Category("Data"),
        Description("Sets the value when the nipple is slid all the way to the right")
        ]
        public decimal MaxValue
        {
            get => maxValue;
            set
            {
                maxValue = value;
                Invalidate();
            }
        }


        private decimal value;
        [
        Category("Data"),
        Description("Sets the value when the nipple is slid all the way to the right")
        ]
        public decimal Value
        {
            get => value;
            set
            {
                this.value = value;
                Invalidate();
            }
        }

        private int thickness;
        [
        Category("Appearance"),
        Description("adjust slider thicc")
        ]
        public int Thickness
        {
            get => thickness;
            set
            {
                thickness = value;
                Invalidate();
            }
        }


        private Color bodyColor;
        [
        Category("Appearance"),
        Description("Sets the color of the long part")
        ]
        public Color BodyColor
        {
            get => bodyColor;
            set
            {
                bodyColor = value;
                Invalidate();
            }
        }


        private Color nippleColor;
        [
        Category("Appearance"),
        Description("Sets the color of the Nipple.")
        ]
        public Color NippleColor
        {
            get => nippleColor;
            set
            {
                nippleColor = value;
                Invalidate();
            }
        }


        private bool fillNipple;
        [
        Category("Appearance"),
        Description("Determines whether the Nipple is filled with color.")
        ]
        public bool FillNipple
        {
            get => fillNipple;
            set
            {
                fillNipple = value;
                Invalidate();
            }
        }



        private int nippleDiameter;
        [
        Category("Appearance"),
        Description("Determines the size of the Nipple in resting position.")
        ]
        public int NippleDiameter
        {
            get => nippleDiameter;
            set
            {
                nippleDiameter = value;
                Invalidate();
            }
        }


        private int nippleExpandedDiameter;
        [
        Category("Appearance"),
        Description("Determines the size of the Nipple when it is expanded.")
        ]
        public int NippleExpandedDiameter
        {
            get => nippleExpandedDiameter;
            set
            {
                nippleExpandedDiameter = value;
                Invalidate();
            }
        }

        public OptionSlider() : base()
        {
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int centerY = Height / 2;
            int padding = Math.Max(Thickness / 2, NippleExpandedDiameter / 2);

            var bodyPen = new Pen(BodyColor, Thickness);
            bodyPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            bodyPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;

            var nippleBrush = new SolidBrush(NippleColor);
            var nipplePen = new Pen(nippleBrush);
            var backBrush = new SolidBrush(BackColor);
            int nippleX = (int)((Width - 2 * padding) * (Value - MinValue) / (MaxValue - MinValue));
            nippleX += padding;
            var nippleRect = new Rectangle(nippleX, centerY - NippleDiameter / 2, NippleDiameter, NippleDiameter);

            g.DrawLine(bodyPen, padding, centerY, Width - (2 * padding), centerY);
            g.FillEllipse(FillNipple ? nippleBrush : backBrush, nippleRect);
            g.DrawEllipse(nipplePen, nippleRect);
        }
    }
}
