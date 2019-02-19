using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Launcher
{
    public static class GameInfo
    {
        static public string gamePath { get; }
        static public string gameExePath { get; }
        static public string gameExeName { get; }
        static public string gameUrl { get; }
        static public string gameResoureName { get; }
        static public string gameMediaPath { get; }
        static public string gameBackupPath { get; }
        static public string gameDataUrl { get; }
        static public string gameDataName { get; }
        static public string gameExePathFull { get { return (gamePath + @"\" + gameExePath); } }
        static public string gameExeFileFull { get { return (gamePath + @"\" + gameExePath + @"\" + gameExeName); } }
        static public string gameMediaPathFull { get { return (gamePath + @"\" + gameMediaPath); } }
        static public string gameMediaFileFull { get { return (gamePath + @"\" + gameMediaPath + @"\Default.txt"); } }
        static public string gameBackupPathAll { get { return (gameBackupPath + DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")); } }
        static GameInfo()
        {
            gamePath = ConfigurationManager.AppSettings.Get("gamePath");
            gameExePath = ConfigurationManager.AppSettings.Get("gameExePath");
            gameExeName = ConfigurationManager.AppSettings.Get("gameExeName");
            gameUrl = ConfigurationManager.AppSettings.Get("gameUrl");
            gameResoureName = ConfigurationManager.AppSettings.Get("gameResoureName");
            gameBackupPath = ConfigurationManager.AppSettings.Get("gameBackupPath");
            gameDataUrl = ConfigurationManager.AppSettings.Get("gameDataUrl");
            gameDataName = ConfigurationManager.AppSettings.Get("gameDataName");
            gameMediaPath = ConfigurationManager.AppSettings.Get("gameMediaPath");
        }
    }
}