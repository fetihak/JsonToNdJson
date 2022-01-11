using System;
using System.IO;
using System.Text;

namespace JsonConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            //string fileName = @"D:\Documents\Personal\Exams\Back End Developer (2021)-20220105-062955362\Back End Developer (2021)\management.json";
            string fileName = @"D:\Documents\Personal\Exams\Back End Developer (2021)-20220105-062955362\Back End Developer (2021)\property.json";


            //string destFile = @"D:\ndmanagement2.json";
            string destFile = @"D:\ndproperty2.json";

            try
            {
                string fileNameWithExtension = Path.GetFileName(fileName);
                string name = fileNameWithExtension.Substring(0, fileNameWithExtension.LastIndexOf('.'));
                string ndJsonFile = JsonToNDJsonConverter.ConvertJsonToNdJson(fileName);// "realestate", name);

                if (string.IsNullOrWhiteSpace(ndJsonFile))
                    return;

                File.WriteAllText(destFile, ndJsonFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
