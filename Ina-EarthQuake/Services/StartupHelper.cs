//using IWshRuntimeLibrary;
//using System;
//using System.IO;

//public static class StartupHelper
//{
//    public static void EnableAutoStart(string appName)
//    {
//        string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
//        string shortcutPath = Path.Combine(startupFolder, $"{appName}.lnk");

//        if (!System.IO.File.Exists(shortcutPath))
//        {
//            string exePath = Environment.ProcessPath!;
//            string iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "AppIcon.ico");

//            var shell = new WshShell();
//            var shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
//            shortcut.Description = "Start Ina-EarthQuake on login";
//            shortcut.TargetPath = exePath;
//            shortcut.WorkingDirectory = Path.GetDirectoryName(exePath)!;
//            shortcut.IconLocation = iconPath;
//            shortcut.Save();
//        }
//    }


//    public static void DisableAutoStart(string appName)
//    {
//        string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
//        string shortcutPath = Path.Combine(startupFolder, $"{appName}.lnk");

//        if (System.IO.File.Exists(shortcutPath))
//        {
//            System.IO.File.Delete(shortcutPath);
//        }
//    }

//    public static bool IsAutoStartEnabled(string appName)
//    {
//        string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
//        string shortcutPath = Path.Combine(startupFolder, $"{appName}.lnk");

//        return System.IO.File.Exists(shortcutPath);
//    }
//}
