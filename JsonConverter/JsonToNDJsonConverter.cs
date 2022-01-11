using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace JsonConverter
{
    public static class JsonToNDJsonConverter
    {
        /// <summary>
        /// Convert Json file to NdJson 
        /// * The function requires a formated Json file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ConvertJsonToNdJson(string fileName)
        {
            return ConvertJsonToNdJson(fileName, string.Empty, string.Empty);
        }
        /// <summary>
        /// Convert Json file to NdJson 
        /// * The function requires a formated Json file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public static string ConvertJsonToNdJson(string fileName, string indexName, string typeName)
        {
            string result = string.Empty;

            string jsonFile = GetJsonFile(fileName);

            if (string.IsNullOrWhiteSpace(jsonFile))
                return result;

            JsonDocument jsonDoc = JsonDocument.Parse(jsonFile);

            if (jsonDoc == null)
                return result;

            StringBuilder ndJson = new StringBuilder();

            string ndJsonObj = string.Empty;
            string annotation = GetJsonAnnotation(indexName, typeName);

            switch (jsonDoc.RootElement.ValueKind)
            {
                case JsonValueKind.Object:

                    ndJsonObj = PrepareData(jsonDoc.RootElement);
                    ndJson.Append(annotation);
                    ndJson.Append("\n");
                    ndJson.Append(ndJsonObj);

                    break;

                case JsonValueKind.Array:

                    int docLength = jsonDoc.RootElement.GetArrayLength();

                    for (int i = 0; i <= docLength - 1; i++)
                    {
                        ndJsonObj = PrepareData(jsonDoc.RootElement[i]);
                        ndJson.Append(annotation);
                        ndJson.Append("\n"); 
                        ndJson.Append(ndJsonObj);
                        ndJson.Append("\n");
                    }

                    break;
            }

            ndJson.Append("\n");
            result = ndJson.ToString();

            return result;
        }

        /// <summary>
        /// prepare datga
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static string PrepareData(JsonElement jsonElement)
        {
            string ndJsonObj = string.Empty;

            string rowObject = jsonElement.EnumerateObject().SingleOrDefault().Value.GetRawText();

            string[] objItem = rowObject.Split('\n');
            ndJsonObj = ConvertArrayToString(objItem);

            return ndJsonObj;
        }

        /// <summary>
        /// return json file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetJsonFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new ArgumentNullException("File not found.");

            if (!Path.GetExtension(filePath).Equals(".json"))
                throw new FormatException("Invalid file format.");

            string file = File.ReadAllText(filePath);

            return file;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="indexName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        private static string GetJsonAnnotation(string indexName, string typeName)
        {
            if (string.IsNullOrWhiteSpace(indexName) && string.IsNullOrWhiteSpace(typeName))
                return @"{ ""index"":{ } }";

            if (!string.IsNullOrWhiteSpace(indexName) && !string.IsNullOrWhiteSpace(typeName))
            {
                return "{ \"index\":{ \"_index\": \"" + indexName + "\", \"_type\": \"" + typeName + "\" }}";
            }

            return string.Empty;
        }
        /// <summary>
        /// converts string array to plain string
        /// </summary>
        /// <param name="stringArray"></param>
        /// <returns></returns>
        public static string ConvertArrayToString(string[] stringArray)
        {
            StringBuilder stringValue = new StringBuilder();

            foreach (string stringLine in stringArray)
                stringValue.Append(stringLine.Trim());

            return stringValue.ToString();
        }
    }
}
