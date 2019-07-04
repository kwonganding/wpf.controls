using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace System.Utility.Helper
{
    public static class QuotedPrintableHelper
    {
        /// <summary>
        /// QuotedPrintable解码函数
        /// </summary>
        /// <param name="input">需要解码的QuotedPrintable字符串</param>
        /// <param name="encoding"></param>
        /// <returns>解码后的字符串</returns>
        public static string Decode(string input, Encoding encoding)
        {
            StringBuilder result = new StringBuilder();
            StringReader sr = new StringReader(input);

            string line = sr.ReadLine();

            while (line != null)
            {
                bool addCRLF = true;
                byte[] bytes = System.Text.Encoding.ASCII.GetBytes(line.ToCharArray());

                for (int i = 0; i < bytes.Length; i++)
                {
                    if ((bytes[i] >= 33 && bytes[i] <= 60) || (bytes[i] >= 62 && bytes[i] <= 126) || bytes[i] == 9 || bytes[i] == 32)
                    {
                        result.Append(Convert.ToChar(bytes[i]));
                        continue;
                    }
                    else if (bytes[i] == 61)
                    {
                        if (i == bytes.Length - 1)
                        {
                            //eg. = soft line break;
                            addCRLF = false;
                            break;
                        }
                        else if (bytes[i + 1] == 51 && bytes[i + 2] == 68)
                        {
                            //eg. =3D
                            i++; i++;
                            result.Append("=");
                            continue;
                        }
                        else
                        {
                            //eg. =B7=D6
                            byte[] b = new byte[2];
                            b[0] = Convert.ToByte(Convert.ToChar(bytes[i + 1]).ToString() + Convert.ToChar(bytes[i + 2]).ToString(), 16);
                            i++; i++; i++;
                            b[1] = Convert.ToByte(Convert.ToChar(bytes[i + 1]).ToString() + Convert.ToChar(bytes[i + 2]).ToString(), 16);
                            i++; i++;

                            result.Append(encoding.GetString(b));
                            continue;
                        }

                    }
                }//end of for

                line = sr.ReadLine();
                if (line != null && addCRLF)
                    result.Append(" ");
            }//end of while
            return result.ToString();
        }

        public static string Encode(string input, Encoding encoding)
        {
            const int MAXLINELENGTH = 76;
            int currentLineLength = 0;
            byte[] bytes = encoding.GetBytes(input.ToCharArray());
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 10 || bytes[i] == 13)
                {
                    if (bytes[i] == 13 && GetNextByte(i, bytes, 1) == 10)
                    {
                        CheckLineLength(MAXLINELENGTH, ref currentLineLength, 0, result);
                        result.Append(" ");
                        currentLineLength = 0;
                        i++;
                        continue;
                    }

                    if (bytes[i] == 10)
                    {
                        CheckLineLength(MAXLINELENGTH, ref currentLineLength, 0, result);
                        result.Append(" ");
                        currentLineLength = 0;
                    }

                    if (bytes[i] == 13)
                    {
                        CheckLineLength(MAXLINELENGTH, ref currentLineLength, 3, result);
                        result.Append("=" + ConvertToHex(bytes[i]));
                    }
                }
                else
                {
                    if ((bytes[i] >= 33 && bytes[i] <= 60) || (bytes[i] >= 62 && bytes[i] <= 126))
                    {
                        CheckLineLength(MAXLINELENGTH, ref currentLineLength, 1, result);
                        result.Append(System.Convert.ToChar(bytes[i]));
                    }
                    else
                    {
                        if (bytes[i] == 9 || bytes[i] == 32)
                        {
                            CheckLineLength(MAXLINELENGTH, ref currentLineLength, 0, result);
                            result.Append(System.Convert.ToChar(bytes[i]));
                            currentLineLength++;
                        }
                        else
                        {
                            CheckLineLength(MAXLINELENGTH, ref currentLineLength, 3, result);
                            result.Append("=" + ConvertToHex(bytes[i]));
                        }
                    }
                }
            }

            return result.ToString();
        }

        private static void CheckLineLength(int maxLineLength, ref int currentLineLength, int newStringLength, StringBuilder sb)
        {
            if (currentLineLength + 1 == maxLineLength || currentLineLength + newStringLength + 1 >= maxLineLength)
            {
                sb.Append("= ");
                currentLineLength = 0 + newStringLength;
            }
            else
            {
                currentLineLength += newStringLength;
            }
        }

        private static int GetNextByte(int index, byte[] bytes, int shiftValue)
        {
            int newIndex = index + shiftValue;

            if (newIndex < 0 || newIndex > bytes.Length - 1 || bytes.Length == 0)
                return -1;
            else
                return bytes[newIndex];
        }

        private static string ConvertToHex(byte number)
        {
            string result = System.Convert.ToString(number, 16).ToUpper();
            return (result.Length == 2) ? result : "0" + result;
        }
    }
}
