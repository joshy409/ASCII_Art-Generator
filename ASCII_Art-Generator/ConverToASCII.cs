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
        
        private const string BLACK = "@";
        private const string CHARCOAL = "#";
        private const string DARKGRAY = "8";
        private const string MEDIUMGRAY = "&";
        private const string MEDIUM = "o";
        private const string GRAY = ":";
        private const string SLATEGRAY = "*";
        private const string LIGHTGRAY = ".";
        private const string WHITE = " ";
        
        //private const string BLACK = "A";
        //private const string CHARCOAL = "B";
        //private const string DARKGRAY = "C";
        //private const string MEDIUMGRAY = "D";
        //private const string MEDIUM = "E";
        //private const string GRAY = "R";
        //private const string SLATEGRAY = "G";
        //private const string LIGHTGRAY = "H";
        //private const string WHITE = "I";

        //test
        //IIIII
        //FFFFF

        private void ConvertPixelsToASCII (PixelColor[,] pixels)
        {
            var width = pixels.GetLength(0);
            var height = pixels.GetLength(1);

     
            List<StringBuilder> ASCIIArt = new List<StringBuilder>() ;

            for (int i = 0; i < (height/5)-1; i ++)
            {
                StringBuilder convertedString = new StringBuilder();
                for (int j = 0; j < (width/5)-1; j ++)
                {
                    int sum = 0;
                    for (int k = 0; k < 5; k++)
                    {
                        for (int l = 0; l < 5; l++)
                        {
                            sum += pixels[(j*5) + k, (i*5) + l].Blue;
                            var a = j + k;
                            var b = i + l;
                        }
                    }
                    sum /= 25;

                    if (sum <= 30)
                    {
                        convertedString.Append(BLACK);
                    } else if (sum <= 60)
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
                    } else
                    {
                        convertedString.Append(WHITE);
                    }
                }
                ASCIIArt.Add(convertedString);
            }

            foreach (var item in ASCIIArt)
            {
                Console.WriteLine(item);
            }
        }
    }
}
