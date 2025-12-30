using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DbHelper
{
    public class NumberToText
    {
        private static string[] _ones =
        {
            "zero",
            "one",
            "two",
            "three",
            "four",
            "five",
            "six",
            "seven",
            "eight",
            "nine"
        };

        private static string[] _teens =
        {
            "ten",
            "eleven",
            "twelve",
            "thirteen",
            "fourteen",
            "fifteen",
            "sixteen",
            "seventeen",
            "eighteen",
            "nineteen"
        };

        private static string[] _tens =
        {
            "",
            "ten",
            "twenty",
            "thirty",
            "forty",
            "fifty",
            "sixty",
            "seventy",
            "eighty",
            "ninety"
        };

        // US Nnumbering:
        private static string[] _thousands =
        {
            "",
            "thousand",
            "million",
            "billion",
            "trillion",
            "quadrillion"
        };


        private static string Convert(decimal value)
        {
            string digits, temp;
            bool showThousands = false;
            bool allZeros = true;

            // Use StringBuilder to build result
            StringBuilder builder = new StringBuilder();
            // Convert integer portion of value to string
            digits = ((long)value).ToString();
            // Traverse characters in reverse order
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int ndigit = (int)(digits[i] - '0');
                int column = (digits.Length - (i + 1));

                // Determine if ones, tens, or hundreds column
                switch (column % 4)
                {
                    case 0:        // Ones position
                        showThousands = true;
                        if (i == 0)
                        {
                            // First digit in number (last in loop)
                            temp = String.Format("{0} ", _ones[ndigit]);
                        }
                        else if (digits[i - 1] == '1')
                        {
                            // This digit is part of "teen" value
                            temp = String.Format("{0} ", _teens[ndigit]);
                            // Skip tens position
                            i--;
                        }
                        else if (ndigit != 0)
                        {
                            // Any non-zero digit
                            temp = String.Format("{0} ", _ones[ndigit]);
                        }
                        else
                        {
                            // This digit is zero. If digit in tens and hundreds
                            // column are also zero, don't show "thousands"
                            temp = String.Empty;
                            // Test for non-zero digit in this grouping
                            if (digits[i - 1] != '0' || (i > 1 && digits[i - 2] != '0'))
                                showThousands = true;
                            else
                                showThousands = false;
                        }

                        // Show "thousands" if non-zero in grouping
                        if (showThousands)
                        {
                            if (column > 0)
                            {
                                temp = String.Format("{0}{1}{2}",
                                    temp,
                                    _thousands[column / 3],
                                    allZeros ? " " : ", ");
                            }
                            // Indicate non-zero digit encountered
                            allZeros = false;
                        }
                        builder.Insert(0, temp);
                        break;

                    case 1:        // Tens column
                        if (ndigit > 0)
                        {
                            temp = String.Format("{0}{1}",
                                _tens[ndigit],
                                (digits[i + 1] != '0') ? "-" : " ");
                            builder.Insert(0, temp);
                        }
                        break;

                    case 2:        // Hundreds column
                        if (ndigit > 0)
                        {
                            temp = String.Format("{0} hundred ", _ones[ndigit]);
                            builder.Insert(0, temp);
                        }
                        break;
                }
            }

            // Append fractional portion/cents
            //builder.AppendFormat("and {0:00}/100", (value - (long)value) * 100);

            // Capitalize first letter
            return String.Format("{0}{1}", Char.ToUpper(builder[0]), builder.ToString(1, builder.Length - 1));
        }

        public static string NumbersToWords(decimal Amt)
        {
            decimal RmVal = 0;
            decimal pAmt = 0;
            string InWords = "";

            //Crore
            if (Amt > 10000000)
                RmVal = Math.Truncate(Amt / 10000000);
            if (Amt == 10000000)
                RmVal = 1;

            if (RmVal == 1)
                InWords = InWords + NumberToText.Convert(RmVal) + " Crore";//+ towords(RmVal,0)
            else if (RmVal > 1)
                InWords = InWords + NumberToText.Convert(RmVal) + " Crores";// + towords(RmVal,0) 


            Amt = Amt - (RmVal * 10000000);

            //Lakhs
            if (Amt > 100000)
                RmVal = Math.Truncate(Amt / 100000);
            if (Amt == 100000)
                RmVal = 1;

            if (RmVal == 1)
                InWords = InWords + " " + NumberToText.Convert(RmVal) + " Lakhs";//+ towords(RmVal,0) 
            else if (RmVal > 1)
                InWords = InWords + " " + NumberToText.Convert(RmVal) + " Lakhs";//+ ToWords(RmVal,0) 

            Amt = Amt - (RmVal * 100000);

            //Thousand
            if (Amt > 1000)
                RmVal = Math.Truncate(Amt / 1000);
            if (Amt == 1000)
                RmVal = 1;

            if (RmVal == 1)
                InWords = InWords + " " + NumberToText.Convert(RmVal) + " Thousand";//+ towords(RmVal,0) 
            else if (RmVal > 1)
                InWords = InWords + " " + NumberToText.Convert(RmVal) + " Thousand";//+ ToWords(RmVal,0) 

            Amt = Amt - (RmVal * 1000);

            //Hundred
            if (Amt > 100)
                RmVal = Math.Truncate(Amt / 100);
            if (Amt == 100)
                RmVal = 1;
            if (Amt < 100)
                RmVal = 0;

            if (RmVal == 1)
                InWords = InWords + " " + NumberToText.Convert(RmVal) + " Hundred";//+ towords(RmVal,0) 
            else if (RmVal > 1)
                InWords = InWords + " " + NumberToText.Convert(RmVal) + " Hundred";//+ ToWords(RmVal,0) 

            Amt = Amt - (RmVal * 100);

            //Ten
            if (Amt > 0)
                InWords = InWords + " " + NumberToText.Convert(Math.Truncate(Amt));//+ towords(truncate(Amt),0)


            //Paise
            pAmt = (Amt - Math.Truncate(Amt)) * 100;

            if (pAmt > 0)
                InWords = InWords + " and " + NumberToText.Convert(Math.Truncate(pAmt)) + " Paise only";//+ towords(pAmt,0) 
            else
                InWords = InWords + " Only";
            return StringHelper.ToTitleCase(StringHelper.RemoveWhiteSpace(InWords.Trim()), TitleCase.First);
        }
    }
}
