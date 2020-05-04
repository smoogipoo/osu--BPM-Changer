using System;
using System.Drawing.Text;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace osu_trainer
{
    public static class Program
    {
        public static PrivateFontCollection FontCollection { get; } = new PrivateFontCollection();

        [STAThread]
        public static void Main()
        {
            AddFont(FontCollection, Properties.Resources.Comfortaa_Bold);

            Application.CurrentCulture = new CultureInfo("en-US", false);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void AddFont(PrivateFontCollection collection, byte[] bytes)
        {
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            IntPtr pointer = handle.AddrOfPinnedObject();
            try
            {
                collection.AddMemoryFont(pointer, bytes.Length);
            }
            finally
            {
                handle.Free();
            }
        }
    }
}