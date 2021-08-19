using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Services
{
    class DataExport
    {
        public static void Display<T>(T obj) {
            var myType = obj.GetType();
            var properties = myType.GetProperties();
            foreach (var property in properties)
            {
                var s = property.GetValue(obj);
            }

        }

    }
}
