using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace ASCII_Art_Generator
{
    public struct PixelColor
    {
        public byte Blue;
        public byte Green;
        public byte Red;
        public byte Alpha;
    }

    public partial class MainWindow : Window
    {
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
        }

        private PixelColor[,] GetPixelColorData(ImageSource bmp)
        {
            BitmapSource source = (BitmapSource)bmp;

            if (source.Format != PixelFormats.Bgra32)
            {
                source = new FormatConvertedBitmap(source, PixelFormats.Bgra32, null, 0);
            }

            int width = source.PixelWidth;
            int height = source.PixelHeight;
            PixelColor[,] pixels = new PixelColor[width, height];

            source.CopyPixels2D( pixels, width * 4, 0);

            return pixels;
        }
    }
}
