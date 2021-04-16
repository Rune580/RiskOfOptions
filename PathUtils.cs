using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RiskOfOptions
{
    internal static class PathUtils
    {
        internal static string GetIconsPath()
        {
            string path = GetMyGamesPath();

            if (!Directory.Exists($"{path}\\icons"))
                Directory.CreateDirectory($"{path}\\icons");

            return $"{path}icons\\";
        }

        internal static string GetStoragePath()
        {
            return "";
        }

        internal static string GetMyGamesPath()
        {
            string documents = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);

            if (!Directory.Exists($"{documents}\\My Games"))
                Directory.CreateDirectory($"{documents}\\My Games");

            if (!Directory.Exists($"{documents}\\My Games\\Risk Of Options"))
                Directory.CreateDirectory($"{documents}\\My Games\\Risk Of Options");

            return $"{documents}\\My Games\\Risk Of Options\\";
        }
    }
}
