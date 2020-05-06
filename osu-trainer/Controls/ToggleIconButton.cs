using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace osu_trainer.Controls
{
	public class ToggleIconButton : ButtonBase
	{
		private bool _isHover;
		private bool _isDown;

		public int SquareSize => Math.Min(Width, Height);

		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			_isHover = true;
			Invalidate(false);
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			_isHover = false;
			Invalidate(false);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseLeave(e);
			_isDown = true;
			Invalidate(false);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			_isDown = false;
			Invalidate(false);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			e.Graphics.Clear(Parent.BackColor);
		
			var color = Color.Transparent;

			if (_isDown)
				color = Color.FromArgb(32, 0, 0, 0);
			else if (_isHover)
				color = Color.FromArgb(32, 255, 255, 255);
			
			if (Checked)
				color = Color.FromArgb(32, 255, 255, 255);

			if (color != Color.Transparent)
			{
				var brush = new SolidBrush(color);
				e.Graphics.FillEllipse(brush, 0, 0, SquareSize, SquareSize);
			}

			var image = Checked ? CheckedImage : UncheckedImage;
			if (image != null)
			{
				var size = SquareSize / 2;
				var x = (SquareSize / 2) - (size / 2);
				var y = (SquareSize / 2) - (size / 2);
				
				e.Graphics.DrawImage(image, x, y, size, size);
			}

			if (Text != null)
			{
				e.Graphics.DrawString(Text, Font, Brushes.White, new Rectangle(0, 0, SquareSize, SquareSize), new StringFormat()
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center
				});
			}
		}
	
		[DefaultValue(false)]
		[Category("Appearance")]
		[RefreshProperties(RefreshProperties.All)]
		public bool Checked
		{
			get => _checked;
			set
			{
				_checked = value;
				
				CheckedChanged?.Invoke(this, EventArgs.Empty);

				Invalidate(false);
			}
		}

		public Image CheckedImage
		{
			get => _checkedImage;
			set
			{
				_checkedImage = value;
				Invalidate(false);
			}
		}

		public Image UncheckedImage
		{
			get => _uncheckedImage;
			set
			{
				_uncheckedImage = value; 
				Invalidate(false);
			}
		}

		public event EventHandler CheckedChanged;

		private bool _checked;
		private Image _checkedImage;
		private Image _uncheckedImage;

		protected override void OnClick(EventArgs e) => Checked = !Checked;
	}
}