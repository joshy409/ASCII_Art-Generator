using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;



namespace ASCII_Art_Generator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int CAPACITY = 10;
        private const int DEFAULT_PIXEL = 5;
        private const float SCREEN_RATIO = 1.3f;
        private const int OUTPUT_TEXTBOX_COUNT = 7;
        private const int OFFSET_TEXTBOX = 2; //offset value so that slider value matches the index 
        private bool IsStart = false; // variable to preven slider value changed function from being called at InitializeComponent()

        StringBuilder[][] ASCIIArts = new StringBuilder[CAPACITY][];
        PixelColor[,] pixels = null;
        TextBox[] outputTextBoxes = new TextBox[OUTPUT_TEXTBOX_COUNT + OFFSET_TEXTBOX];

        public MainWindow()
        {
            InitializeComponent();

            for (int i = OFFSET_TEXTBOX; i < OUTPUT_TEXTBOX_COUNT + OFFSET_TEXTBOX; i++)
            {
                outputTextBoxes[i] = (TextBox)OutputTextBoxGrid.Children[i - OFFSET_TEXTBOX];
                outputTextBoxes[i].Height = SystemParameters.WorkArea.Height / SCREEN_RATIO;
            }
            this.Top = 0;
            this.Left = 0;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            inputPreview.Height = SystemParameters.WorkArea.Height / SCREEN_RATIO;
            resolutionSlider.Value = DEFAULT_PIXEL;

        }

        
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            dialog.Filter = "Image Files (*.png,*.jpg,*.jpeg)|(*.png;*.jpg;*.jpeg)";


            if (dialog.ShowDialog() == true)
            {
                foreach (var art in ASCIIArts)
                {
                    Array.Clear(ASCIIArts, 0, ASCIIArts.Length);
                }
                ImageSource grayImageSource = ConvertImageToGrayScaleImage(dialog.FileName);
                pixels = GetPixelColorData(grayImageSource);
                resolutionSlider.Value = DEFAULT_PIXEL;
                ClearTextBoxes(DEFAULT_PIXEL);
                ASCIIArts[DEFAULT_PIXEL] = ConvertPixelsToASCII(pixels, (int)resolutionSlider.Value);
                PrintASCIIArt(ASCIIArts[DEFAULT_PIXEL], DEFAULT_PIXEL);
                LoadPreview(dialog.FileName);
                EnableTextBoxes(5);
            }
        }

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
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                string output = string.Join<StringBuilder>("\r\n", ASCIIArts[(int)resolutionSlider.Value]);
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
            if (IsStart)
            {

                if (ASCIIArts[(int)resolutionSlider.Value] == null)
                {
                    ASCIIArts[(int)resolutionSlider.Value] = ConvertPixelsToASCII(pixels, (int)resolutionSlider.Value);
                    PrintASCIIArt(ASCIIArts[(int)resolutionSlider.Value], (int)resolutionSlider.Value);
                    EnableTextBoxes((int)resolutionSlider.Value);
                }
                else
                {
                    EnableTextBoxes((int)resolutionSlider.Value);
                }
                
            } else
            {
                IsStart = true;
            }
        }

        private void Font_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        { 
            TextElement.SetFontSize(OutputTextBoxGrid, (int)fontSlider.Value);
        }
    }
}
