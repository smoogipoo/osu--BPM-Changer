using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osu_trainer.Controls
{
    public abstract class AdvancedControl : Control
    {
        protected private bool _hover;
        protected private bool _down;

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

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _down = true;
            Invalidate(false);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _down = false;
            Invalidate(false);
        }

        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    e.Graphics.Clear(Parent.BackColor);
        //}
    }
}