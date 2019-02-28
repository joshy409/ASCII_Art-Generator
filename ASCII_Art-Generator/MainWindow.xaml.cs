using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;


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
        private bool IsStart = false; // variable to prevent slider value changed function from being called at InitializeComponent()

        StringBuilder[][] ASCIIArts = null;
        PixelColor[,] pixels = null;
        TextBox[] outputTextBoxes = new TextBox[OUTPUT_TEXTBOX_COUNT + OFFSET_TEXTBOX];

        private double[] previousState = new double[4];

        public MainWindow()
        {
            InitializeComponent();

            for (int i = OFFSET_TEXTBOX; i < OUTPUT_TEXTBOX_COUNT + OFFSET_TEXTBOX; i++)
            {
                outputTextBoxes[i] = (TextBox)OutputTextBoxGrid.Children[i - OFFSET_TEXTBOX];
            }

            this.Top = 0;
            this.Left = 0;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;

        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            //dialog.InitialDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
            dialog.Filter = "Image Files (*.png,*.jpg,*.jpeg)|*.png;*.jpg;*.jpeg";

            if (dialog.ShowDialog() == true)
            {
                LoadPreview(dialog.FileName);

                ASCIIArts = new StringBuilder[CAPACITY][];

                ImageSource grayImageSource = ConvertImageToGrayScaleImage(dialog.FileName);
                pixels = GetPixelColorData(grayImageSource);

                resolutionSlider.Value = DEFAULT_PIXEL;
                ClearTextBoxes(DEFAULT_PIXEL);
                EnableTextBoxes(5);
                GetASCIIArt();
            }      
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt"
            };

            if (dialog.ShowDialog() == true)
            {
                string output = outputTextBoxes[(int)resolutionSlider.Value].Text;
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
            if (IsStart && pixels != null)
            {
                EnableTextBoxes((int)resolutionSlider.Value);
                ASCIIWidthTextBox.Text = (pixels.GetLength(0) / (int)resolutionSlider.Value).ToString();
                ASCIIHeightTextBox.Text = (pixels.GetLength(1) / (int)resolutionSlider.Value).ToString();
            } else
            {
                IsStart = true;
            }
        }

        private void Font_Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        { 
            TextElement.SetFontSize(OutputTextBoxGrid, (int)fontSlider.Value);
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            inputPreview.Height = this.Height / SCREEN_RATIO;
            for (int i = OFFSET_TEXTBOX; i < OUTPUT_TEXTBOX_COUNT + OFFSET_TEXTBOX; i++)
            {
                outputTextBoxes[i].Height = this.Height / SCREEN_RATIO;
            }
        }

        private void OnWindowStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                previousState[0] = this.Top;
                previousState[1] = this.Left;
                previousState[2] = this.Width;
                previousState[3] = this.Height;

                this.Top = 0;
                this.Left = 0;
                this.Width = SystemParameters.WorkArea.Width;
                this.Height = SystemParameters.WorkArea.Height;

                inputPreview.Height = this.Height / SCREEN_RATIO;
                for (int i = OFFSET_TEXTBOX; i < OUTPUT_TEXTBOX_COUNT + OFFSET_TEXTBOX; i++)
                {
                    outputTextBoxes[i].Height = this.Height / SCREEN_RATIO;
                }

            } else if ( WindowState == WindowState.Normal) {

                this.Top = previousState[0];
                this.Left = previousState[1];
                this.Width = previousState[2];
                this.Height = previousState[3];
            }
        }
    }
}
