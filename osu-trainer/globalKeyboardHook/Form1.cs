using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Utilities;

namespace key_preview {
	public partial class Form1 : Form {
		globalKeyboardHook gkh = new globalKeyboardHook();

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			gkh.HookedKeys.Add(Keys.A);
			gkh.HookedKeys.Add(Keys.B);
			gkh.KeyDown += new KeyEventHandler(gkh_KeyDown);
			gkh.KeyUp += new KeyEventHandler(gkh_KeyUp);
		}

		void gkh_KeyUp(object sender, KeyEventArgs e) {
			lstLog.Items.Add("Up\t" + e.KeyCode.ToString());
			e.Handled = true;
		}

		void gkh_KeyDown(object sender, KeyEventArgs e) {
			lstLog.Items.Add("Down\t" + e.KeyCode.ToString());
			e.Handled = true;
		}
	}
}