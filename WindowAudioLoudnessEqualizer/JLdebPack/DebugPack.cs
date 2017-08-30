﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JLdebPack
{
    public static class DebugPack
    {
        private static object loggerLock = new object();
        private static DateTime now;
        private static string fileName;

        public static bool DebugMode = true;
        public static System.IO.DirectoryInfo workDirectory;
        public static System.IO.FileInfo file;


        public static void CM(string s) { ConsoleMessage(s); }
        public static void CML(string s) { ConsoleMessageLine(s); }
        public static void DM(string s) { DebugMessage(s); }
        public static void DML(string s) { DebugMessageLine(s); }

        public static void ConsoleMessage(string s) { System.Diagnostics.Debug.Write(s); }
        public static void ConsoleMessageLine(string s) { System.Diagnostics.Debug.WriteLine(s); }
        public static void DebugMessage(string s) { if (DebugMode) { System.Diagnostics.Debug.Write(s); } }
        public static void DebugMessageLine(string s) { if (DebugMode) { System.Diagnostics.Debug.WriteLine(s); } }


        public static void SetWorkDirectory(string workDirectory)
        {
            DebugPack.workDirectory = System.IO.Directory.CreateDirectory(workDirectory);
        }
        public static void Erase(int eraseDays)
        {
            if (now != null)
            {
                foreach (System.IO.FileInfo fi in workDirectory.EnumerateFiles())
                {
                    if (fi.Name.Contains(fileName))
                    {
                        string f = fi.Name;
                        int first = f.IndexOf("-"), last = f.Length - first;
                        if (f.EndsWith(".txt")) last = f.LastIndexOf(".txt") - first;
                        DateTime ftime;
                        bool success = DateTime.TryParse(f.Substring(first, last), out ftime);
                        if (success)
                        {
                            TimeSpan dif = ftime - now;
                            if (dif.Days > eraseDays) System.IO.File.Delete(f);
                        }
                    }
                }
            }
        }
        public static void Open(string name)
        {
            fileName = name;
            now = DateTime.Now;
            string fullName = string.Format(@"{0}\{1}-{2:d4}{3:d2}{4:d2}-{5:d2}{6:d2}{7:d2}.txt", workDirectory.FullName, fileName, now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            file = new System.IO.FileInfo(fullName);
        }
        public static void Log(string msg, bool newLine = true)
        {
            lock (loggerLock)
            {
                System.IO.StreamWriter writer = new System.IO.StreamWriter(path: file.FullName, append: true, encoding: System.Text.Encoding.UTF8);
                writer.Write($"{DateTime.Now.ToLocalTime()}: {msg}");
                if (newLine) writer.Write("\r\n");
                writer.Close();
                writer.Dispose();
            }
        }
    }
}