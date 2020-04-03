using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osu_trainer.funorControls
{
    class RatioBar : Control
    {
        #region Customizable Properties
        
        private Color leftColour;
        [
            Category("Appearance"),
            Description("Sets the colour of the left bar"),
        ]
        public Color LeftColour
        {
            get => leftColour;
            set
            {
                leftColour = value;
                Invalidate();
            }
        }

        private Color rightColour;
        [
            Category("Appearance"),
            Description("Sets the colour of the right bar"),
        ]
        public Color RightColour
        {
            get => rightColour;
            set
            {
                rightColour = value;
                Invalidate();
            }
        }

        // RightColour
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

        #endregion


        public RatioBar()
        {
            DoubleBuffered = true;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // draw objects
            float barMiddle = Width * LeftPercent / 100.0f;
            g.DrawLine(new Pen(LeftColour, 2), 0, Height/2, barMiddle, Height/2);
            g.DrawLine(new Pen(RightColour, 2), barMiddle, Height/2, Width, Height/2);
        }

    }
}
