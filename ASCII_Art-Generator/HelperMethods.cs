using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ASCII_Art_Generator
{
    public partial class MainWindow : Window
    {
        private void ClearTextBoxes(int index)
        {
            for (int i = OFFSET_TEXTBOX; i < outputTextBoxes.Length; i++)
            {
                outputTextBoxes[i].Clear();
                outputTextBoxes[i].Visibility = Visibility.Hidden;
                outputTextBoxes[i].IsEnabled = false;
            }
            outputTextBoxes[index].IsEnabled = true;
            outputTextBoxes[index].Visibility = Visibility.Visible;
        }

        private void EnableTextBoxes(int index)
        {
            for (int i = OFFSET_TEXTBOX; i < outputTextBoxes.Length; i++)
            {
                outputTextBoxes[i].Visibility = Visibility.Hidden;
                outputTextBoxes[i].IsEnabled = false;
            }
            outputTextBoxes[index].IsEnabled = true;
            outputTextBoxes[index].Visibility = Visibility.Visible;
        }

        private void LoadPreview(string fileName)
        {
            // Create source
            BitmapImage myBitmapImage = new BitmapImage();

            // BitmapImage.UriSource must be in a BeginInit/EndInit block
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(fileName);
            myBitmapImage.DecodePixelHeight = 800;
            myBitmapImage.EndInit();

            //set image source
            inputPreview.Source = myBitmapImage;
            Tabs.SelectedIndex = 0;
        }

        private void GetASCIIArt()
        {
            for (int i = OFFSET_TEXTBOX; i < OUTPUT_TEXTBOX_COUNT + OFFSET_TEXTBOX; i++)
            {
                ASCIIArts[i] = ConvertPixelsToASCII(pixels, i);
                PrintASCIIArt(ASCIIArts[i], i);
            }
        }
    }
}
