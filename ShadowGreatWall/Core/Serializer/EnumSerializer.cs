using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ShadowGreatWall.Core.Serializer
{
    public class EnumSerializer
    {
        public static string GetDescription<T>(Object value, String otherDesc = "", Boolean nameInstead = false)
        {
            var type = typeof(T);
            if (!type.IsEnum)
            {
                throw new ArgumentException("该对象不是一个枚举类型！");
            }
            string name = Enum.GetName(type, Convert.ToInt32(value));
            if (name == null)
            {
                return otherDesc;
            }
            FieldInfo field = type.GetField(name);
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
            if (attribute == null && nameInstead == true)
            {
                return name;
            }
            return attribute == null ? otherDesc : attribute.Description;
        }

        public static List<KeyValuePair<String, Int32>> EnumToDictionary<T>()
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("传入的参数必须是枚举类型！", "enumType");
            }
            Dictionary<String, Int32> enumDic = new Dictionary<string, int>();

            Array enumValues = Enum.GetValues(enumType);
            foreach (Enum enumValue in enumValues)
            {
                Int32 value = Convert.ToInt32(enumValue);
                String desc = GetDescription<T>(value, enumValue.ToString());
                enumDic.Add(desc, value);
            }
            return enumDic.ToList();
        }

    }
}