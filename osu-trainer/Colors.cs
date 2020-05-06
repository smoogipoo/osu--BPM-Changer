using System.Drawing;

namespace osu_trainer
{
	public static class Colors
	{
		public static Color FormBg     = Color.FromArgb(38, 35, 53);
		public static Color TextBoxBg  = Color.FromArgb(23, 16, 25);
		public static Color TextBoxFg  = Color.FromArgb(224, 224, 224);
		public static Color ReadOnlyBg = Color.FromArgb(36, 24, 38);
		public static Color ReadOnlyFg = Color.Silver;
		public static Color Disabled   = Color.FromArgb(136, 134, 144);
		
		// VALUES
		public static Color Easier = Color.FromArgb(114, 241, 184);
		
		// DIFFICULTIES
		public static Color DifficultyEasy       = Color.FromArgb(136, 179, 0);
		public static Color DifficultyNormal     =  Color.FromArgb(102, 204, 255);
		public static Color DifficultyHard       =  Color.FromArgb(255, 204, 34);
		public static Color DifficultyInsane     =  Color.FromArgb(255, 102, 170);
		public static Color DifficultyExpert     = Color.FromArgb(170, 136, 255);
		public static Color DifficultyExpertPlus = Color.FromArgb(23, 22, 28); // because the app is dark.
		
		// ACCENTS
		public static Color Salmon       = Color.FromArgb(249, 126, 114);
		public static Color PaleBlue     = Color.FromArgb(167, 154, 233);
		public static Color AccentRed    = Color.FromArgb(254, 68, 80);
		public static Color AccentOrange = Color.FromArgb(246, 122, 44);
		public static Color AccentYellow = Color.FromArgb(254, 222, 93);
		public static Color AccentBlue   = Color.FromArgb(46, 226, 250);
		public static Color AccentCyan   = Color.FromArgb(46, 226, 250);
		public static Color AccentPink   = Color.FromArgb(243, 114, 185);
		public static Color AccentPink2   = Color.FromArgb(197, 51, 131);

		public static Color GetDifficultyColor(float stars)
		{
			if (stars < 2)
				return DifficultyEasy;
			
			if (stars < 2.7)
				return DifficultyNormal;
			
			if (stars < 4)
				return DifficultyHard;
			
			if (stars < 5.3)
				return DifficultyInsane;
			
			if (stars < 6.5)
				return DifficultyExpert;
			
			return DifficultyExpertPlus;
		}
	}
}