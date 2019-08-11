using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.utils
{
    public class StringUtils
    {
        /// <summary>
        /// 将字符串转换为数字
        /// </summary>
        /// <param name="Str">待转换字符串</param>
        /// <returns>转换后的数字</returns>
        public static int GetInt(string Str)
        {
            if (String.IsNullOrEmpty(Str)) return 0;
            try
            {
                if (Str.IndexOf('.') > -1)
                {
                    return Convert.ToInt32(Convert.ToSingle(Str));
                }
                else
                {
                    return Convert.ToInt32(Str);
                }
            }
            catch { return 0; }
        }

        /// <summary>
        /// 将字符串转换为数字
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public static int GetInt(object Obj)
        {
            if (Obj == null)
            {
                return 0;
            }
            return GetInt(Obj.ToString());
        }
    }
}
