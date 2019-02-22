using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;


namespace ASCII_Art_Generator
{
    public partial class MainWindow : Window
    {
        private const string BLACK = "@";
        private const string CHARCOAL = "#";
        private const string DARKGRAY = "8";
        private const string MEDIUMGRAY = "&";
        private const string MEDIUM = "o";
        private const string GRAY = ":";
        private const string SLATEGRAY = "*";
        private const string LIGHTGRAY = ".";
        private const string WHITE = " ";
        
        private List<StringBuilder> ConvertPixelsToASCII (PixelColor[,] pixels, double dResolution)
        {
            List<StringBuilder> ASCIIArt = new List<StringBuilder>();
            if (pixels != null)
            {
                var width = pixels.GetLength(0);
                var height = pixels.GetLength(1);
                int resolution = (int)dResolution;


                for (int i = 0; i < (height / resolution) - 1; i++)
                {
                    StringBuilder convertedString = new StringBuilder();
                    for (int j = 0; j < (width / resolution) - 1; j++)
                    {
                        int sum = 0;
                        for (int k = 0; k < resolution; k++)
                        {
                            for (int l = 0; l < resolution; l++)
                            {
                                sum += pixels[(j * resolution) + k, (i * resolution) + l].Blue;
                            }
                        }
                        sum /= 25;
                        AppendASCII(convertedString, sum);
                    }
                    ASCIIArt.Add(convertedString);
                    outputTextBox.Text += (convertedString.ToString() + "\n");
                }
            }
            return ASCIIArt;
        }

        private static void AppendASCII(StringBuilder convertedString, int sum)
        {
            if (sum <= 30)
            {
                convertedString.Append(BLACK);
            }
            else if (sum <= 60)
            {
                convertedString.Append(CHARCOAL);
            }
            else if (sum <= 90)
            {
                convertedString.Append(DARKGRAY);
            }
            else if (sum <= 120)
            {
                convertedString.Append(MEDIUMGRAY);
            }
            else if (sum <= 150)
            {
                convertedString.Append(MEDIUM);
            }
            else if (sum <= 180)
            {
                convertedString.Append(GRAY);
            }
            else if (sum <= 210)
            {
                convertedString.Append(SLATEGRAY);
            }
            else if (sum <= 240)
            {
                convertedString.Append(LIGHTGRAY);
            }
            else
            {
                convertedString.Append(WHITE);
            }
        }
    }
}
