using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace BankSystem.Services
{
    class DataExport
    {
        public void ExportProperties<T>(T obj, string path)
        {

            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj));
            }
            var myType = obj.GetType();
            var properties = myType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var fields = myType.GetFields(BindingFlags.Public | BindingFlags.Instance);
            string objData = "";
            string objFieldData = "";
            using (FileStream fileStream = new FileStream(path, FileMode.Truncate))
            {
                foreach (var property in properties)
                {
                    objData += $"{property.Name}: {property.GetValue(obj)} {Environment.NewLine}";
                    Console.WriteLine($"{property.Name}: {property.GetValue(obj)}");
                }

                foreach (var field in fields)
                {
                    objFieldData += $"{field.Name}: {field.GetValue(obj)}{Environment.NewLine}";
                    Console.WriteLine($"{field.Name}: {field.GetValue(obj)}");

                }
                objData += objFieldData;
                byte[] array = System.Text.Encoding.Default.GetBytes(objData);
                fileStream.Write(array, 0, array.Length);
            }
        }

    }
}
