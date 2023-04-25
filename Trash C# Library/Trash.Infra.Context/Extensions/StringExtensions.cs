using System;
using System.Collections.Generic;
using System.Text;

namespace Trash.Infra.Context.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// make a string a native persion string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FixString(this string str)
        {
            return str.FixPersianChars().En2FaNumber();
        }
        ///<summary>
        /// to translate the english numbers into persion one
        ///</summary>
        public static string En2FaNumber(this string str)
        {
            return str.Replace("0", "۰")
               .Replace("1", "۱")
               .Replace("2", "۲")
               .Replace("3", "۳")
               .Replace("4", "۴")
               .Replace("5", "۵")
               .Replace("6", "۶")
               .Replace("7", "۷")
               .Replace("8", "۸")
               .Replace("9", "۹");
        }

        /// <summary>
        /// replace the persion numbers with the english one
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Fa2EnNumber(this string str)
        {
            return str.Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                //iphone numeric
                .Replace("٠", "0")
                .Replace("١", "1")
                .Replace("٢", "2")
                .Replace("٣", "3")
                .Replace("٤", "4")
                .Replace("٥", "5")
                .Replace("٦", "6")
                .Replace("٧", "7")
                .Replace("٨", "8")
                .Replace("٩", "9");
        }
        /// <summary>
        /// fixe persion characters by replacing arabic with native persion
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FixPersianChars(this string str)
        {
            return str.Replace("ﮎ", "ک")
                .Replace("ﮏ", "ک")
                .Replace("ﮐ", "ک")
                .Replace("ﮑ", "ک")
                .Replace("ك", "ک")
                .Replace("ي", "ی")
                .Replace(" ", " ")
                .Replace("‌", " ")
                .Replace("ھ", "ه");//.Replace("ئ", "ی");
        }
    }
}
