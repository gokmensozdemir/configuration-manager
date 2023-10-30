using System.Collections.Generic;
using System;

namespace ConfigurationManager.API.Utility
{
    public class TypeHelper
    {
        public static Dictionary<string, Type> ValidTypes = new Dictionary<string, Type> {
            { "String", typeof(String) },
            { "Int", typeof(Int32) },
            { "Double", typeof(Double) },
            { "Boolean", typeof(Boolean) }
        };

        public static bool IsValidType(string value, string type)
        {
            if (type == "Boolean")
            {
                return value == "1" || value == "0";
            }
            else if (type == "Double")
            {
                return double.TryParse(value, out _);
            }
            else if (type == "Int")
            {
                return int.TryParse(value, out _);
            }

            return true;
        }
    }
}
