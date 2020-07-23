using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace EFTest.Utils
{
    public class EnumUtil
    {
        public static string GetDescription(Enum enumVal)
        {
            if (enumVal == null) return null;
            var fieldInfo = enumVal.GetType().GetField(enumVal.ToString());
            if (fieldInfo == null)
            {
                return null;
            }
            Object[] obj = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (obj != null && obj.Length != 0)
            {
                DescriptionAttribute des = (DescriptionAttribute)obj[0];
                return des.Description;
            }
            return null;
        }
    }
}
