using System;
using System.IO;
using System.Text;
using UnityEngine;

namespace GBG.GameToolkit.Editor.ConfigData
{
    public static class EditorCsvUtility
    {
        /// <summary>
        /// Re-saves all CSV files in the specified folder using UTF-8 with BOM encoding.
        /// </summary>
        /// <param name="folder">The folder path.</param>
        /// <param name="originalEncoding">The original encoding of the csv files.</param>
        public static void ResaveCsvFilesWithUtf8bom(string folder, Encoding originalEncoding = null)
        {
            if (!Directory.Exists(folder))
            {
                var msg = $"Folder path does not exist: {folder}";
                LogErrorMessage(msg);
                return;
            }

            try
            {
                string[] csvFiles = Directory.GetFiles(folder, "*.csv", SearchOption.TopDirectoryOnly);
                foreach (string csvFile in csvFiles)
                {
                    try
                    {
                        originalEncoding ??= Encoding.UTF8;
                        string originalContent = File.ReadAllText(csvFile, originalEncoding);

                        File.WriteAllText(csvFile, originalContent, new UTF8Encoding(true));
                        var msg = $"File resaved using UTF-8 with BOM encoding: {csvFile}";
                        LogMessage(msg);
                    }
                    catch (Exception ex)
                    {
                        var msg = $"Exception occurred while saving the file: {ex.Message}, file path: {csvFile}";
                        LogErrorMessage(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                var msg = $"Exception occurred while traversing the folder: {ex.Message}";
                LogErrorMessage(msg);
            }

            void LogMessage(string msg)
            {
#if UNITY_EDITOR || UNITY_5_6_OR_NEWER
                Debug.Log(msg);
#else
                Console.WriteLine(msg);
#endif
            }

            void LogErrorMessage(string msg)
            {
#if UNITY_EDITOR || UNITY_5_6_OR_NEWER
                Debug.LogError(msg);
#else
                var fgColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);
                Console.ForegroundColor = fgColor;
#endif
            }
        }
    }
}
