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
            try
            {
                Convert.ChangeType(value, ValidTypes[type]);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
