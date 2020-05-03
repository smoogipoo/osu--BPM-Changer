using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace osu_trainer
{
    class NumericUpDownFix : NumericUpDown
    {
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e is HandledMouseEventArgs hme)
                hme.Handled = true;

            if (e.Delta > 0)
            {
                if (Value + Increment > Maximum)
                    Value = Maximum;
                else
                    Value += Increment;
            }
            else if (e.Delta < 0)
            {
                if (Value - Increment < Minimum)
                    Value = Minimum;
                else
                    Value -= Increment;
            }
        }
    }
}
