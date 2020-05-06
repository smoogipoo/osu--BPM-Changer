using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace osu_trainer.Controls
{
    public class SongDisplay : Control
    {
        private Font _difficultyFont;
        private Font _artistFont;
        private Font _titleFont;

        public string Artist { get; set; }

        public string Title { get; set; }

        public string Difficulty { get; set; }

        public string ErrorMessage { get; set; }

        private Image _cover;

        public SongDisplay()
        {
            updateFonts();
        }

        public Image Cover
        {
            get => _cover;
            set
            {
                _cover = value;
                Invalidate(false);
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            updateFonts();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            if (!string.IsNullOrWhiteSpace(ErrorMessage))
            {
                DrawErrorMessage(e.Graphics);
                return;
            }

            var height = Width / 4;

            if (Cover == null)
            {
                using (var brush = new SolidBrush(Colors.ReadOnlyBg))
                {
                    e.Graphics.FillRectangle(brush, 0, 0, Width, height);
                }
            }
            else
            {
                using (var background = prepareImage(Cover, Width, height))
                {
                    e.Graphics.DrawImage(background, 0, 0, background.Width, background.Height);
                }
            }

            using (var darkenBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0)))
            {
                e.Graphics.FillRectangle(darkenBrush, 0, 0, Width, height);
            }

            var bottom = height - 5;

            var shadowColor = Color.FromArgb(128, 0, 0, 0);
            using (var shadowBrush = new SolidBrush(shadowColor))
            {
                var artistHeight = (int)e.Graphics.MeasureString(Artist, _artistFont).Height;
                var artistY = bottom - artistHeight;
                e.Graphics.DrawString(Artist, _artistFont, shadowBrush, 5, artistY + 1);
                e.Graphics.DrawString(Artist, _artistFont, Brushes.White, 5, artistY);

                var titleFormat = new StringFormat()
                {
                    Trimming = StringTrimming.EllipsisCharacter
                };
                var titleHeight = e.Graphics.MeasureString(Title, _titleFont).Height;
                var titleY = artistY - titleHeight;
                var titleRectangle = new RectangleF(5, titleY, Width - 5, titleHeight);
                titleRectangle.Y++;
                e.Graphics.DrawString(Title, _titleFont, shadowBrush, titleRectangle, titleFormat);

                titleRectangle.Y--;
                e.Graphics.DrawString(Title, _titleFont, Brushes.White, titleRectangle, titleFormat);

                DrawDifficultyBadge(e.Graphics, shadowBrush);
            }
        }

        private void DrawErrorMessage(Graphics graphics)
        {
            using (var textBrush = new SolidBrush(Colors.AccentSalmon))
            {
                var rectangle = new RectangleF(0, 0, Width, Height);
                var format = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                graphics.DrawString(ErrorMessage, _titleFont, textBrush, rectangle, format);
            }
        }

        private void DrawDifficultyBadge(Graphics graphics, Brush shadowBrush)
        {
            if (string.IsNullOrWhiteSpace(Difficulty))
                return;

            var difficultySize = graphics.MeasureString(Difficulty, _difficultyFont);
            var difficultyWidth = difficultySize.Width + 30;
            var rectangle = new RectangleF(Width - 5 - difficultyWidth, 5, difficultyWidth, difficultySize.Height + 12);
            using (var path = JunUtils.RoundedRect(rectangle, 14))
            {
                graphics.FillPath(shadowBrush, path);
            }

            var difficultyFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            rectangle.Y++;
            graphics.DrawString(Difficulty, _difficultyFont, shadowBrush, rectangle, difficultyFormat);

            rectangle.Y--;
            graphics.DrawString(Difficulty, _difficultyFont, Brushes.White, rectangle, difficultyFormat);
        }

        private Image prepareImage(Image image, int width, int height)
        {
            var bitmap = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                var scaleFactor = width / (float)image.Width;
                var newHeight = image.Height * scaleFactor;
                graphics.DrawImage(image, 0, (height / 2) - (newHeight / 2), width, newHeight);
            }

            return bitmap;
        }

        private void updateFonts()
        {
            _difficultyFont = new Font(Font.FontFamily, 12, FontStyle.Bold, GraphicsUnit.Pixel);
            _artistFont = new Font(Font.FontFamily, 14, FontStyle.Bold, GraphicsUnit.Pixel);
            _titleFont = new Font(Font.FontFamily, 20.8f, FontStyle.Bold, GraphicsUnit.Pixel);
        }
    }
}