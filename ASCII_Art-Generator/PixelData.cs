using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ASCII_Art_Generator
{
    public partial class MainWindow : Window
    {
        

        private struct PixelColor
        {
            public byte Blue;
            public byte Green;
            public byte Red;
            public byte Alpha;
        }

        private ImageSource ConvertImageToGrayScaleImage(string path)

        {
            // Create an Image control
            Image grayImage = new Image();

            // Create a BitmapImage and sets its DecodePixelWidth and DecodePixelHeight
            BitmapImage bmpImage = new BitmapImage();

            bmpImage.BeginInit();
            bmpImage.UriSource = new Uri(path, UriKind.RelativeOrAbsolute);
            bmpImage.EndInit();

            // Create a new image using FormatConvertedBitmap and set DestinationFormat to GrayScale
            FormatConvertedBitmap grayBitmap = new FormatConvertedBitmap();

            grayBitmap.BeginInit();
            grayBitmap.Source = bmpImage;
            grayBitmap.DestinationFormat = PixelFormats.Gray8;
            grayBitmap.EndInit();

            // Set Source property of Image
            grayImage.Source = grayBitmap;

            return grayImage.Source;


            //// Get pixel Color data
            //GetPixelColorData(grayImage.Source);
            //LayoutRoot.Children.Add(grayImage);
        }

        private PixelColor[,] GetPixelColorData(ImageSource bmp)
        {
            //BitmapImage bmp = new BitmapImage(new Uri(path));
            BitmapSource source = (BitmapSource)bmp;

            if (source.Format != PixelFormats.Bgra32)
            {
                source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
            }

            int width = source.PixelWidth;
            int height = source.PixelHeight;
            PixelColor[,] pixels = new PixelColor[width, height];

            var pixelBytes = new byte[height * width * 4];
            source.CopyPixels(pixelBytes, width * 4, 0);

            int y0 = 0 / width;
            int x0 = 0 - width * y0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[x + x0, y + y0] = new PixelColor
                    {
                        Blue = pixelBytes[(y * width + x) * 4 + 0],
                        Green = pixelBytes[(y * width + x) * 4 + 1],
                        Red = pixelBytes[(y * width + x) * 4 + 2],
                        Alpha = pixelBytes[(y * width + x) * 4 + 3],
                    };
                }
            }
            return pixels;
        }
    }
}
