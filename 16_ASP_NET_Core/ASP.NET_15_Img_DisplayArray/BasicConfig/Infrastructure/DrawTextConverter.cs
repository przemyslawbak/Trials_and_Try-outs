using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace BasicConfig.Infrastructure
{
    //credits: https://gist.github.com/naveedmurtuza/6600103
    public class DrawTextConverter
    {
        public MemoryStream DrawText(string text)
        {
            //setup
            Color textColor = Color.FromName("Black");
            Font font = new Font("Gill Sans", 14);
            int maxWidth = 350;
            Graphics drawing;
            SizeF textSize;
            StringFormat sf;
            Image img;

            //first, create a dummy bitmap just to get a graphics object
            using (img = new Bitmap(1, 1))
            using (drawing = Graphics.FromImage(img))
            {
                //measure the string to see how big the image needs to be
                textSize = drawing.MeasureString(text, font, maxWidth);

                //set the stringformat flags to rtl
                sf = new StringFormat();
                //uncomment the next line for right to left languages
                //sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
                sf.Trimming = StringTrimming.Word;
            }

            //create a new image of the right size
            using (img = new Bitmap((int)textSize.Width, (int)textSize.Height))
            {
                using (drawing = Graphics.FromImage(img))
                {
                    //Adjust for high quality
                    drawing.CompositingQuality = CompositingQuality.HighQuality;
                    drawing.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    drawing.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    drawing.SmoothingMode = SmoothingMode.HighQuality;
                    drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                    //paint the background
                    drawing.Clear(Color.Transparent);

                    //create a brush for the text
                    using (Brush textBrush = new SolidBrush(textColor))
                    {
                        drawing.DrawString(text, font, textBrush, new RectangleF(0, 0, textSize.Width, textSize.Height), sf);
                        drawing.Save();
                    }
                }

                //stream closed in controller after taking value
                MemoryStream ms = new MemoryStream();
                img.Save(ms, ImageFormat.Png);
                return ms;
            }
        }
    }
}
