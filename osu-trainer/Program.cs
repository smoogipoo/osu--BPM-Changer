using System;
using System.Globalization;
using System.Windows.Forms;

namespace osu_trainer
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            Application.CurrentCulture = new CultureInfo("en-US", false);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
