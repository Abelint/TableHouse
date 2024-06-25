using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace TableHouse
{
    public static class AdapterClass
    {
       public  static ImageBrush BackgroundImage {  get; set; }
       public static ImageBrush Brushing(Bitmap src)
        {
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            ImageBrush imBrush = new ImageBrush()
            {
                //ImageSource = new BitmapImage(new Uri(path))
                ImageSource = image
            };

            return imBrush;
        }
    }
}
