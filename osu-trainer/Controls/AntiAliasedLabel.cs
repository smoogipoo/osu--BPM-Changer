using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace osu_trainer.Controls
{
    public class AntiAliasedLabel : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

            using (var foregroundBrush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(Text, Font, foregroundBrush, 0, 0);
            }
        }
    }
}