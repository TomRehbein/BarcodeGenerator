using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodeGenerator.Model
{
    ///EAN8 has 8 numbers
    ///The first 7 are the core numbers
    ///and number 8 is a checksum
    ///so EAN8 reach from 0000 000 - 9999 999
    class EAN8
    {
        public EAN8(string num) // num must be 7 chars
        {
            if (num.Length == 7)

            {
                CreateBitmap(ConvertNumToBinary(GetNumWithCheckNum(num), DelimiterDictionary()), GetNumWithCheckNum(num));
            }
            else
            {
                Console.WriteLine("The DeviceSn is not valid");
                Console.ReadKey();
            }
        }

        private IDictionary<int, string> DelimiterDictionary()
        {
            IDictionary<int, string> delimiter = new Dictionary<int, string>()
            {
                {0, "101"},
                {1, "01010"}
            };

            return delimiter;
        }

        private IDictionary<int, string> FirstPartDictionary()
        {
            IDictionary<int, string> firstPartDictionary = new Dictionary<int, string>()
            {
                {0, "0001101"},
                {1, "0011001"},
                {2, "0010011"},
                {3, "0111101"},
                {4, "0100011"},
                {5, "0110001"},
                {6, "0101111"},
                {7, "0111011"},
                {8, "0110111"},
                {9, "0001011"}
            };

            return firstPartDictionary;
        }

        private IDictionary<int, string> SecondPartDictionary()
        {
            IDictionary<int, string> secondPartDictionary = new Dictionary<int, string>()
            {
                {0, "1110010"},
                {1, "1100110"},
                {2, "1101100"},
                {3, "1000010"},
                {4, "1011100"},
                {5, "1001110"},
                {6, "1010000"},
                {7, "1000100"},
                {8, "1001000"},
                {9, "1110100"}
            };

            return secondPartDictionary;
        }

        private string ConvertNumToBinary(string num, IDictionary<int, string> delimiter)
        {
            string binary = "";
            binary += delimiter[0];
            binary += ConvertFourNumsToBinaryString(num.Substring(0, 4), FirstPartDictionary());
            binary += delimiter[1];
            binary += ConvertFourNumsToBinaryString(num.Substring(4, 4), SecondPartDictionary());
            binary += delimiter[0];

            return binary;
        }

        private string GetNumWithCheckNum(string num)
        {
            int odd = 0;
            int even = 0;
            int temp;

            for (int i = 1; i <= num.Length; i++)
            {
                temp = Convert.ToInt32(num.Substring(i - 1, 1));

                if (i % 2 == 0) even += temp;
                else odd += temp;
            }

            num += Convert.ToString((10 - ((3 * odd + even) % 10)) % 10); // the last "% 10" is for the case CheckNum = 0

            return num;
        }

        private string ConvertFourNumsToBinaryString(string num, IDictionary<int, string> Dictionary) // num must be 4 chars
        {
            string binary = "";

            for (int i = 0; i < num.Length; i++)
            {
                binary += Dictionary[Convert.ToInt32(num.Substring(i, 1))];
            }

            return binary;
        }

        private void CreateBitmap(string binary, string num)
        {
            Bitmap myBitmap = new Bitmap(174, 130);
            Pen pen = new Pen(Color.Black, 2);
            Font drawFont = new Font("Arial", 10);
            SolidBrush drawBrush = new SolidBrush(Color.Black);
            Graphics g = Graphics.FromImage(myBitmap);
            int count = 20;
            int heitgh = 110;

            g.Clear(Color.White);

            for (int i = 0; i < binary.Length; i++)
            {
                if (Convert.ToInt32(binary.Substring(i, 1)) == 1)
                {
                    if (i == 0 || i == 2 || i == 32 || i == 34 || i == 64 || i == 66) heitgh = 125;

                    g.DrawLine(pen, count, 10, count, heitgh);
                }
                count += 2;
                heitgh = 110;
            }

            g.DrawString(num.Substring(0, 4), drawFont, drawBrush, 39, 110);
            g.DrawString(num.Substring(4, 4), drawFont, drawBrush, 101, 110);

            myBitmap.Save("C:\\temp\\" + num + ".jpg");

            g.Dispose();
            pen.Dispose();
            myBitmap.Dispose();
        }
    }
}