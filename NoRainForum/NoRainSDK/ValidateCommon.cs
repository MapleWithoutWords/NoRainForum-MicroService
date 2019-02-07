using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace NoRainSDK
{
    internal class ValidateCommon
    {
        public static bool IsValid(object obj, out string errorMsg)
        {
            StringBuilder sb = new StringBuilder();
            bool res = true;
            var objType = obj.GetType();
            var properties = objType.GetProperties();
            IEnumerable<Attribute> attris;
            foreach (var propertyInfo in properties)
            {
                object val = propertyInfo.GetValue(obj);
                attris = propertyInfo.GetCustomAttributes();
                foreach (var item in attris)
                {
                    if (item is ValidationAttribute)
                    {
                        ValidationAttribute validAttr = (ValidationAttribute)item;
                        if (!validAttr.IsValid(val))
                        {
                            res = false;
                            sb.AppendLine(validAttr.ErrorMessage);
                        }
                    }
                }
            }
            errorMsg = sb.ToString();
            return res;
        }
    }
}
