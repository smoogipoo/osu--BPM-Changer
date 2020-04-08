using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osu_trainer
{
    [DefaultEvent("ValueChanged")]
    class OptionSlider : Control
    {
        #region Customizable Properties
        private decimal minValue;
        [
        Category("Data"),
        Description("Sets the value when the nipple is slid all the way to the left"),
        DefaultValue(0)
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
        Description("Sets the value when the nipple is slid all the way to the right"),
        DefaultValue(10)
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
        Description("Sets the initial value for the slider"),
        DefaultValue(0)
        ]
        public decimal Value
        {
            get => value;
            set
            {
                this.value = Helper.Clamp(value, MinValue, MaxValue);
                Invalidate();
            }
        }

        private decimal increment;
        [
        Category("Data"),
        Description("Sets value precision"),
        DefaultValue(0.1)
        ]
        public decimal Increment
        {
            get => increment;
            set
            {
                increment = value;
                Invalidate();
            }
        }

        private int thickness;
        [
        Category("Appearance"),
        Description("adjust slider thicc"),
        DefaultValue(5)
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
        Description("Sets the stroke color of the Nipple.")
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


        private Color nippleIdleColor;
        [
        Category("Appearance"),
        Description("Sets the stroke color of the Nipple when idle.")
        ]
        public Color NippleIdleColor
        {
            get => nippleIdleColor;
            set 
            {
                nippleIdleColor = value;
                Invalidate();
            }
        }


        private int nippleStrokeWidth;
        [
        Category("Appearance"),
        Description("Determines whether the Nipple is filled with color."),
        DefaultValue(1)
        ]
        public int NippleStrokeWidth
        {
            get => nippleStrokeWidth;
            set
            {
                nippleStrokeWidth = value;
                Invalidate();
            }
        }


        private bool fillNipple;
        [
        Category("Appearance"),
        Description("Determines whether the Nipple is filled with color."),
        DefaultValue(false)
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


        private bool fillDraggingNipple;
        [
        Category("Appearance"),
        Description("Determines whether the Nipple is filled with color."),
        DefaultValue(true)
        ]
        public bool FillDraggingNipple
        {
            get => fillDraggingNipple;
            set
            {
                fillDraggingNipple = value;
                Invalidate();
            }
        }



        private int nippleDiameter;
        [
        Category("Appearance"),
        Description("Determines the size of the Nipple in resting position."),
        DefaultValue(17)
        ]
        public int NippleDiameter
        {
            get => nippleDiameter;
            set
            {
                nippleDiameter = value;
                nippleCurrentDiameter = value;
                Invalidate();
            }
        }


        private int nippleExpandedDiameter;
        [
        Category("Appearance"),
        Description("Determines the size of the Nipple when it is expanded."),
        DefaultValue(30)
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
        #endregion

        #region OnValueChanged Event
        private EventHandler onValueChanged;

        [
        Category("Data"),
        Description("Sets the value when the nipple is slid all the way to the left")
        ]
        public event EventHandler ValueChanged
        {
            add { onValueChanged += value; }
            remove { onValueChanged += value; }
        }
        protected virtual void OnValueChanged(EventArgs e)
        {
            onValueChanged?.Invoke(this, e);
        }
        #endregion


        private int nippleCurrentDiameter;
        private bool dragging = false;
        private bool expanded = false;
        
        public OptionSlider() : base()
        {
            DoubleBuffered = true;
            Invalidate();
        }

        #region Event Handlers
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;


            // style
            var bodyPen = new Pen(BodyColor, Thickness);
            bodyPen.StartCap = LineCap.Round;
            bodyPen.EndCap = LineCap.Round;
            var nippleBrush = new SolidBrush(NippleColor);
            var nipplePen = new Pen(nippleBrush, NippleStrokeWidth);

            var nippleIdlePen = new Pen(NippleIdleColor, NippleStrokeWidth);
            var backBrush = new SolidBrush(BackColor);

            // size and position
            int width = GetEndX() - GetStartX();
            int centerY = Height / 2;
            int nippleX = GetStartX() + (int)(width * (Value - MinValue) / (MaxValue - MinValue));

            // draw objects
            g.DrawLine(bodyPen, GetStartX(), centerY, GetEndX(), centerY);
            if (!Enabled)
                return;
            bool fill = dragging ? true : FillNipple;
            g.FillEllipse(fill ? nippleBrush : backBrush, GetNippleRect());
            g.DrawEllipse(expanded ? nipplePen : nippleIdlePen, GetNippleRect());
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (!Enabled)
                return;
            // check if cursor is moused over nipple
            if (!dragging)
            {
                if (GetNippleRect().Contains(e.Location))
                {
                    nippleCurrentDiameter = NippleExpandedDiameter;
                    Cursor = Cursors.Hand;
                    expanded = true;
                }
                else
                {
                    nippleCurrentDiameter = NippleDiameter;
                    Cursor = Cursors.Default;
                    expanded = false;
                }
            }
            else // dragging
            {
                // convert mouse coordinates to slider coordinates
                int sliderX = e.X - GetStartX();

                // convert slider position to value
                decimal sliderPercent = ((decimal)sliderX / (decimal)(GetEndX() - GetStartX()));
                decimal oldValue = Value;
                Value = MinValue + (MaxValue - MinValue) * sliderPercent;
                // clamp value
                Value = Helper.Clamp(Value, MinValue, MaxValue);
                // quantize value
                Value = Helper.Quantize(Value, Increment);
                if (Value != oldValue)
                    OnValueChanged(EventArgs.Empty);
            }
            Invalidate();
            base.OnMouseMove(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!Enabled)
                return;
            if (GetNippleRect().Contains(e.Location))
            {
                dragging = true;
            }
            Invalidate();
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (!Enabled)
                return;
            dragging = false;
            expanded = GetNippleRect().Contains(e.Location);
            nippleCurrentDiameter = NippleDiameter;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            expanded = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (!Enabled)
                return;
            if (e.Delta > 0)
                Value = Helper.Clamp(Value + Increment, MinValue, MaxValue);
            if (e.Delta < 0)
                Value = Helper.Clamp(Value - Increment, MinValue, MaxValue);
            onValueChanged?.Invoke(this, e);
        }
        #endregion

        #region Geometry Functions
        private Rectangle GetNippleRect()
        {
            int width = GetEndX() - GetStartX();
            int centerY = Height / 2;
            int nippleX = GetStartX() + (int)(width * (Value - MinValue) / (MaxValue - MinValue));
            var nippleRect = new Rectangle(nippleX, centerY, nippleCurrentDiameter, nippleCurrentDiameter);
            nippleRect.X -= nippleCurrentDiameter / 2;
            nippleRect.Y -= nippleCurrentDiameter / 2;
            return nippleRect;
        }
        private int GetStartX()
        {
            return Math.Max(Thickness / 2, NippleExpandedDiameter / 2) + 2;
        }
        private int GetEndX()
        {
            return Width - GetStartX() - 2;
        }
        #endregion
    }
}
