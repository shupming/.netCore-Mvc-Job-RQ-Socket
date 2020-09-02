using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HospitalReport.Common
{
    public static class BinaryConversion
    {
        /// <summary>
        /// 转化32进制
        /// </summary>
        /// <param name="xx"></param>
        /// <returns></returns>
        public static string ToBinaryString32(this int xx)
        {
            string a = "";
            while (xx >= 1)
            {
                int index = Convert.ToInt16(xx - (xx / 32) * 32);
                a = Base64Code[index] + a;
                xx = xx / 32;
            }
            return a;
        }
        /// <summary>
        /// 二进制转10进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ConvertToTwo(this string value)
        {
            int temp = 0;
            for (int i = 0; i < value.Length; i++)
            {
                temp += ((int)Math.Pow(2, i) * Convert.ToInt16(value.Substring(value.Length - i - 1, 1)));
            }
            return temp;
        }

        /// <summary>
        /// 转化32进制（字符串不能含有字母）
        /// </summary>
        /// <param name="xx"></param>
        /// <returns></returns>
        public static string ToBinaryString32(this string xx)
        {
            string a = "";
            var res = long.Parse(xx);
            while (res >= 1)
            {
                int index = Convert.ToInt16(res - (res / 32) * 32);
                a = Base64Code[index] + a;
                res = res / 32;
            }
            return a;
        }
        /// <summary>
        /// 转化64进制（字符串不能含有字母）
        /// </summary>
        /// <param name="xx"></param>
        /// <returns></returns>
        public static string ToBinaryString64(this string xx)
        {
            string a = "";
            var res = long.Parse(xx);
            while (res >= 1)
            {
                int index = Convert.ToInt16(res - (res / 64) * 64);
                a = Base64Code[index] + a;
                res = res / 64;
            }
            return a;
        }
        /// <summary>
        /// 转化64进制
        /// </summary>
        /// <param name="xx"></param>
        /// <returns></returns>
        public static string ToBinaryString64(this int xx)
        {
            string a = "";

            while (xx >= 1)
            {
                int index = Convert.ToInt16(xx - (xx / 64) * 64);
                a = Base64Code[index] + a;
                xx = xx / 64;
            }
            return a;
        }
        private static Dictionary<int, string> Base64Code = new Dictionary<int, string>() {
            {   0  ,"z"}, {   1  ,"1"}, {   2  ,"2"}, {   3  ,"3"}, {   4  ,"4"}, {   5  ,"5"}, {   6  ,"6"}, {   7  ,"7"}, {   8  ,"8"}, {   9  ,"9"},
            {   10  ,"a"}, {   11  ,"b"}, {   12  ,"c"}, {   13  ,"d"}, {   14  ,"e"}, {   15  ,"f"}, {   16  ,"g"}, {   17  ,"h"}, {   18  ,"i"}, {   19  ,"j"},
            {   20  ,"k"}, {   21  ,"x"}, {   22  ,"m"}, {   23  ,"n"}, {   24  ,"y"}, {   25  ,"p"}, {   26  ,"q"}, {   27  ,"r"}, {   28  ,"s"}, {   29  ,"t"},
            {   30  ,"u"}, {   31  ,"v"}, {   32  ,"w"}, {   33  ,"x"}, {   34  ,"y"}, {   35  ,"z"}, {   36  ,"A"}, {   37  ,"B"}, {   38  ,"C"}, {   39  ,"D"},
            {   40  ,"E"}, {   41  ,"F"}, {   42  ,"G"}, {   43  ,"H"}, {   44  ,"I"}, {   45  ,"J"}, {   46  ,"K"}, {   47  ,"L"}, {   48  ,"M"}, {   49  ,"N"},
            {   50  ,"O"}, {   51  ,"P"}, {   52  ,"Q"}, {   53  ,"R"}, {   54  ,"S"}, {   55  ,"T"}, {   56  ,"U"}, {   57  ,"V"}, {   58  ,"W"}, {   59  ,"X"},
            {   60  ,"Y"}, {   61  ,"Z"}, {   62  ,"-"}, {   63  ,"_"},
        };
        private static Dictionary<string, int> _Base64Code
        {
            get
            {
                return Enumerable.Range(0, Base64Code.Count()).ToDictionary(i => Base64Code[i], i => i);
            }
        }
    }
}
