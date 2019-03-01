using Microsoft.Win32;
using System;
using System.ComponentModel;
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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private const int DEFAULT_PIXEL_KERNEL_WIDTH = 5;
        private const float SCREEN_RATIO = 1.3f; //ratio used to calculate privew and textbox height to fit the screen
        private const int OUTPUT_TEXTBOX_COUNT = 7;
        private const int OFFSET_TEXTBOX = 2; //offset value so that slider value matches the index 
        private bool IsStart = false; // variable to prevent slider value changed function from being called at InitializeComponent()

        StringBuilder[][] ASCIIArts = null;
        PixelColor[,] pixels = null;
        TextBox[] outputTextBoxes = new TextBox[OUTPUT_TEXTBOX_COUNT + OFFSET_TEXTBOX];

        private double[] previousPosition = new double[4];

        private int _kernel;
        public event PropertyChangedEventHandler PropertyChanged;
        public int Kernel
        {
            get { return _kernel; }
            set { _kernel = value * value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Kernel")); }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
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
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog
            {
                // Set filter for file extension and default file extension 
                Filter = "Image Files (*.png,*.jpg,*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (dialog.ShowDialog() == true)
            {
                ShowImportedImage(dialog.FileName);

                ASCIIArts = new StringBuilder[OUTPUT_TEXTBOX_COUNT + OFFSET_TEXTBOX][];

                ImageSource grayImageSource = ConvertImageToGrayScaleImage(dialog.FileName);
                pixels = GetPixelColorData(grayImageSource);

                resolutionSlider.Value = DEFAULT_PIXEL_KERNEL_WIDTH;
                ClearTextBoxes();
                EnableTextBoxes(DEFAULT_PIXEL_KERNEL_WIDTH);
                PrintASCIIArtToTextBox();
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
            Kernel = (int)resolutionSlider.Value;
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
                previousPosition[0] = this.Top;
                previousPosition[1] = this.Left;
                previousPosition[2] = this.Width;
                previousPosition[3] = this.Height;

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

                this.Top = previousPosition[0];
                this.Left = previousPosition[1];
                this.Width = previousPosition[2];
                this.Height = previousPosition[3];
            }
        }
    }
}
