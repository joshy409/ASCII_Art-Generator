using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Top = 0;
            this.Left = 0;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            inputPreview.Height = SystemParameters.WorkArea.Height / 1.5;
            outputTextBox.Height = SystemParameters.WorkArea.Height / 1.5;
        }

        List<List<StringBuilder>> ASCIIArts = new List<List<StringBuilder>>(10);
        List<StringBuilder> ASCIIArt = new List<StringBuilder>();
        PixelColor[,] pixels = null;
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            dialog.Filter = "PNG Files (*.png)|*.png|JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg";


            if (dialog.ShowDialog() == true)
            {
                ImageSource grayImageSource = ConvertImageToGrayScaleImage(dialog.FileName);
                pixels = GetPixelColorData(grayImageSource);
                outputTextBox.Clear();
                ASCIIArts[5] = ConvertPixelsToASCII(pixels, resolutionSlider.Value);
                LoadPreview(dialog.FileName);
            }
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
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                string output = string.Join("\r\n", ASCIIArt);
                File.WriteAllText(dialog.FileName, output);

                //open exported file
                if (File.Exists(dialog.FileName))
                {
                    Process.Start(dialog.FileName);
                }
            }
        }

        private void ResolutionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ASCIIArt.Clear();
            outputTextBox.Clear();
            ASCIIArt = ConvertPixelsToASCII(pixels, resolutionSlider.Value);
        }
    }
}
