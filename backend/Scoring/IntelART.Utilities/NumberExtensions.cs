using System;

namespace IntelART.Utilities
{
    public static class NumberExtensions
    {
        public static string ToWords(this decimal value)
        {
            return Convert.ToInt32(value).ToWords();
        }

        public static string ToWords(this int value)
        {
            string result = null;

            if (value > 0)
            {
                string amountString = value.ToString();
                int amountLength = amountString.Length;
                int padCount = 3 - (amountLength % 3);
                for (int i = 0; i < padCount; i++)
                {
                    amountString = "0" + amountString;
                }
                amountLength += padCount;
                int groupLength = amountLength / 3;
                for (int i = 0; i < groupLength; i++)
                {
                    string groupResult = GroupInWords(amountString.Substring(amountLength - 3 * (i + 1), 3));
                    if (groupResult != "")
                    {
                        switch (i)
                        {
                            case 0:
                                result = groupResult;
                                break;
                            case 1:
                                result = groupResult + " հազար" + (result == "" ? "" : " " + result);
                                break;
                            case 2:
                                result = groupResult + " միլիոն" + (result == "" ? "" : " " + result);
                                break;
                            case 3:
                                result = groupResult + " միլիարդ" + (result == "" ? "" : " " + result);
                                break;
                        }
                    }
                }
            }
            return (result == null ? "զրո" : result);
        }

        private static string GroupInWords(string groupNumber)
        {
            string result = "";

            if (groupNumber != null)
            {
                if (groupNumber.Length > 2)
                {
                    switch (groupNumber.Substring(2, 1))
                    {
                        case "1":
                            result = "մեկ";
                            break;
                        case "2":
                            result = "երկու";
                            break;
                        case "3":
                            result = "երեք";
                            break;
                        case "4":
                            result = "չորս";
                            break;
                        case "5":
                            result = "հինգ";
                            break;
                        case "6":
                            result = "վեց";
                            break;
                        case "7":
                            result = "յոթ";
                            break;
                        case "8":
                            result = "ութ";
                            break;
                        case "9":
                            result = "ինը";
                            break;
                    }
                }

                if (groupNumber.Length > 1)
                {
                    switch (groupNumber.Substring(1, 1))
                    {
                        case "1":
                            result = (result == "" ? "տասը" : "տասն" + result);
                            break;
                        case "2":
                            result = "քսան" + result;
                            break;
                        case "3":
                            result = "երեսուն" + result;
                            break;
                        case "4":
                            result = "քառասուն" + result;
                            break;
                        case "5":
                            result = "հիսուն" + result;
                            break;
                        case "6":
                            result = "վաթսուն" + result;
                            break;
                        case "7":
                            result = "յոթանասուն" + result;
                            break;
                        case "8":
                            result = "ութսուն" + result;
                            break;
                        case "9":
                            result = "իննսուն" + result;
                            break;
                    }
                }

                if (groupNumber.Length > 0)
                {
                    switch (groupNumber.Substring(0, 1))
                    {
                        case "1":
                            result = "հարյուր" + (result == "" ? "" : " " + result);
                            break;
                        case "2":
                            result = "երկու հարյուր" + (result == "" ? "" : " " + result);
                            break;
                        case "3":
                            result = "երեք հարյուր" + (result == "" ? "" : " " + result);
                            break;
                        case "4":
                            result = "չորս հարյուր" + (result == "" ? "" : " " + result);
                            break;
                        case "5":
                            result = "հինգ հարյուր" + (result == "" ? "" : " " + result);
                            break;
                        case "6":
                            result = "վեց հարյուր" + (result == "" ? "" : " " + result);
                            break;
                        case "7":
                            result = "յոթ հարյուր" + (result == "" ? "" : " " + result);
                            break;
                        case "8":
                            result = "ութ հարյուր" + (result == "" ? "" : " " + result);
                            break;
                        case "9":
                            result = "ինը հարյուր" + (result == "" ? "" : " " + result);
                            break;
                    }
                }
            }

            return result;
        }
    }
}
