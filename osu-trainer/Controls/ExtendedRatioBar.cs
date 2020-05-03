using System;
using System.Drawing;
using System.Timers;

namespace osu_trainer.Controls
{
	public class ExtendedRatioBar : RatioBar
	{
		private readonly Timer _animationTimer;
		
		private Color _targetLeftColor;
		private Color _targetRightColor;
		private int _targetLeftPercent;
		private const double AnimationSpeed = 0.125;

		public ExtendedRatioBar()
		{
			_animationTimer = new Timer()
			{
				Interval = 33, // 30 fps
				Enabled = true,
			};
			_animationTimer.Elapsed += AnimationTimerOnElapsed;
		}

		private void AnimationTimerOnElapsed(object sender, ElapsedEventArgs e)
		{
			if (!_targetLeftColor.Equals(base.LeftColor))
			{
				var r = base.LeftColor.R + ((_targetLeftColor.R - base.LeftColor.R) * AnimationSpeed);
                var g = base.LeftColor.G + ((_targetLeftColor.G - base.LeftColor.G) * AnimationSpeed);
                var b = base.LeftColor.B + ((_targetLeftColor.B - base.LeftColor.B) * AnimationSpeed);
                
                base.LeftColor = Color.FromArgb((int)r, (int)g, (int)b);
			}
			
			if (!_targetRightColor.Equals(base.RightColor))
			{
				var r = base.RightColor.R + ((_targetRightColor.R - base.RightColor.R) * AnimationSpeed);
				var g = base.RightColor.G + ((_targetRightColor.G - base.RightColor.G) * AnimationSpeed);
				var b = base.RightColor.B + ((_targetRightColor.B - base.RightColor.B) * AnimationSpeed);

				base.RightColor = Color.FromArgb((int)r, (int)g, (int)b);
			}

			if (_targetLeftPercent != base.LeftPercent)
			{
				if (_targetLeftPercent > base.LeftPercent)
					base.LeftPercent += (int)Math.Max(1, (_targetLeftPercent - base.LeftPercent) * AnimationSpeed);
				else if (_targetLeftPercent < base.LeftPercent)
					base.LeftPercent -= (int)Math.Max(1, (base.LeftPercent - _targetLeftPercent) * AnimationSpeed);
			}
		}

		public new Color LeftColor
		{
			get => _targetLeftColor;
			set => _targetLeftColor = value;
		}

		public new int LeftPercent
		{
			get => _targetLeftPercent;
			set => _targetLeftPercent = value;
		}

		public new Color RightColor
		{
			get => _targetRightColor;
			set => _targetRightColor = value;
		}
	}
}