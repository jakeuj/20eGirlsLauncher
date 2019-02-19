using LINQtoCSV;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Launcher
{
    class Main
    {
        //開始遊戲
        public string GameStart_Click()
        {
            if (!File.Exists(GameInfo.gameExeFileFull))
                return "找不到遊戲主程式，請先執行下載遊戲！" + Environment.NewLine;
            Process _exe = new Process();
            _exe.StartInfo.FileName = GameInfo.gameExeName;
            _exe.StartInfo.WorkingDirectory = GameInfo.gameExePathFull;
            _exe.Start();
            return "已執行遊戲！" + Environment.NewLine;
        }
        //備份遊戲
        public string GameBackup_Click()
        {
            if (!File.Exists(GameInfo.gameMediaFileFull))
                return "找不到遊戲設定檔，請先執行下載遊戲！" + Environment.NewLine;
            CopyFiles(GameInfo.gameDataName, GameInfo.gameMediaPathFull, GameInfo.gameBackupPathAll);
            return "已手動備份原始設定檔：" + Environment.NewLine + string.Format(@"{0}\{1} 至 {2}\{1}", GameInfo.gameMediaPathFull, GameInfo.gameDataName, GameInfo.gameBackupPathAll) + Environment.NewLine;
        }
        //更新遊戲
        public bool GameUpdate_Click()
        {
            return updateFromWeb(GameInfo.gameDataName);
        }
        public bool GameUpdate_Check()
        {
            return File.Exists(GameInfo.gameMediaFileFull);
        }
        public string GameUpdate_Backup()
        {
            int ErrorCount;
            string msg = string.Empty;
            if (File.Exists(GameInfo.gameDataName))
            {
                CopyFiles(GameInfo.gameDataName, GameInfo.gameMediaPathFull, GameInfo.gameBackupPathAll);
                msg += "已自動備份原始設定檔：" + Environment.NewLine;
                msg += string.Format(@"{0}\{1} 至 {2}\{1}", GameInfo.gameMediaPathFull, GameInfo.gameDataName, GameInfo.gameBackupPathAll) + Environment.NewLine;
                ErrorCount = ReadFromLocal(GameInfo.gameMediaPathFull + @"\" + GameInfo.gameDataName, GameInfo.gameDataName, GameInfo.gameBackupPathAll + @"\Old_Default.csv", GameInfo.gameBackupPathAll + @"\New_Default.txt");
                msg += string.Format(@"已自動更新遊戲設定檔：{0} 至 {1}\{0}", GameInfo.gameDataName, GameInfo.gameMediaPathFull) + Environment.NewLine;
                if (ErrorCount > 0)
                    msg += string.Format("更新過程發生錯誤次數：{0}", ErrorCount) + Environment.NewLine;
                msg += string.Format(@"已自動備份更新設定檔：{0}\New_Default.txt", GameInfo.gameBackupPathAll) + Environment.NewLine;
                msg += string.Format(@"已自動備份原始中繼檔：{0}\Old_Default.csv", GameInfo.gameBackupPathAll) + Environment.NewLine;
            }
            else
            {
                msg += "找不到更新檔案...請手動更新..." + Environment.NewLine;
            }
            return msg;
        }
        // 遊戲下載
        public bool GameDownload()
        {
            return downloadFromWeb(GameInfo.gameResoureName, GameInfo.gameUrl);
        }
        // 遊戲安裝
        public bool GameInstall()
        {
            if (File.Exists(GameInfo.gameResoureName))
            {
                Process _exe = new Process();
                _exe.StartInfo.FileName = GameInfo.gameResoureName;
                _exe.Start();
                return true;
            }
            else
                return false;
        }
        // 從google下載設定檔來更新資料
        bool updateFromWeb(string curFile)
        {
            string remoteUri = GameInfo.gameDataUrl + "&exportFormat=csv";
            WebClient myWebClient = new WebClient();
            bool Status = false;
            try
            {
                myWebClient.DownloadFile(remoteUri, curFile);
                Status = true;
            }
            catch
            {
                Status = false;
            }
            return Status;
        }
        //讀取其他設定並擷取上半部與下半部
        int ReadFromLocal(string OldTxtPath, string NewCsvPath, string OldCsvPath, string NewTxtPath)
        {
            string[] lines = File.ReadAllLines(OldTxtPath);
            StringBuilder keepData = new StringBuilder();
            StringBuilder keepDataEnd = new StringBuilder();
            StringBuilder procData = new StringBuilder();
            int stepProc = 0;
            List<Model.Leader> products2 = new List<Model.Leader>();
            Model.Leader currLeader = new Model.Leader();
            bool isProc = false;
            int ErrorCount = 0;
            foreach (string line in lines)
            {
                switch (stepProc)
                {
                    case 0:
                        if (line == "[LEADER]0")
                        {
                            stepProc = 1;
                            goto case 1;
                        }
                        else
                        {
                            keepData.AppendLine(line);
                            break;
                        }
                    case 1:
                        if (line == "[UNIT]0")
                        {
                            stepProc = 2;
                            goto case 2;
                        }
                        else
                        {
                            //處理人物資訊
                            procData.AppendLine(line);
                            //準備人物資料
                            string tmpData = string.Empty;
                            //人物資料是否已開啟
                            if (isProc)
                            {
                                if (line == "[/LEADER]")
                                {
                                    //填入人物資料
                                    products2.Add(currLeader);
                                    currLeader = new Model.Leader();
                                    isProc = false;
                                    break;
                                }
                                else
                                {
                                    string tmpLeaderData = line.Trim();
                                    int indexP = tmpLeaderData.IndexOf('=');
                                    string KeyName = tmpLeaderData.Substring(0, indexP);
                                    string KeyValue = tmpLeaderData.Substring(indexP + 1);
                                    int tmpIntP;
                                    switch (KeyName)
                                    {
                                        case "Name":
                                            currLeader.Name = KeyValue;
                                            break;
                                        case "Female":
                                            currLeader.Female = 1;
                                            break;
                                        case "Alias":
                                            currLeader.Alias = KeyValue;
                                            break;
                                        case "Corps":
                                            currLeader.Corps = KeyValue;
                                            break;
                                        case "Graph":
                                            currLeader.Graph = KeyValue;
                                            break;
                                        case "Status":
                                            currLeader.Status = KeyValue == "0" ? 0 : -1;
                                            break;
                                        case "Power":
                                            if (int.TryParse(KeyValue, out tmpIntP))
                                                currLeader.Power = tmpIntP;
                                            else
                                                currLeader.Power = 20;
                                            break;
                                        case "History":
                                            currLeader.History = KeyValue == "1" ? 1 : 0;
                                            break;
                                        case "Faction":
                                            currLeader.Faction = KeyValue;
                                            break;
                                        case "Will":
                                            if (int.TryParse(KeyValue, out tmpIntP))
                                                currLeader.Will = tmpIntP;
                                            else
                                                currLeader.Will = 1;
                                            break;
                                        case "Loyality":
                                            if (int.TryParse(KeyValue, out tmpIntP))
                                                currLeader.Loyality = tmpIntP;
                                            else
                                                currLeader.Loyality = 10;
                                            break;
                                        case "UnitIndex":
                                            if (int.TryParse(KeyValue, out tmpIntP))
                                                currLeader.UnitIndex = tmpIntP;
                                            else
                                                currLeader.UnitIndex = -10;
                                            break;
                                        case "Icon":
                                            if (int.TryParse(KeyValue, out tmpIntP))
                                                currLeader.Icon = tmpIntP;
                                            else
                                                currLeader.Icon = 0;
                                            break;
                                        case "RecruitType":
                                            if (int.TryParse(KeyValue, out tmpIntP))
                                                currLeader.RecruitType = tmpIntP;
                                            else
                                                currLeader.RecruitType = 0;
                                            break;
                                        case "RecruitValue":
                                            if (int.TryParse(KeyValue, out tmpIntP))
                                                currLeader.RecruitValue = tmpIntP;
                                            else
                                                currLeader.RecruitValue = 0;
                                            break;
                                        case "LeaderSkill0":
                                            currLeader.LeaderSkill0 = KeyValue;
                                            break;
                                        case "LeaderSkill1":
                                            currLeader.LeaderSkill1 = KeyValue;
                                            break;
                                        case "LeaderSkill2":
                                            currLeader.LeaderSkill2 = KeyValue;
                                            break;
                                        case "LeaderSkill3":
                                            currLeader.LeaderSkill3 = KeyValue;
                                            break;
                                        case "LeaderSkill4":
                                            currLeader.LeaderSkill4 = KeyValue;
                                            break;
                                        case "LeaderSkill5":
                                            currLeader.LeaderSkill5 = KeyValue;
                                            break;
                                        case "Description0":
                                            currLeader.Description0 = KeyValue;
                                            break;
                                        case "Description1":
                                            currLeader.Description1 = KeyValue;
                                            break;
                                        case "Description2":
                                            currLeader.Description2 = KeyValue;
                                            break;
                                        case "Description3":
                                            currLeader.Description3 = KeyValue;
                                            break;
                                        case "Cabinet":
                                            if (int.TryParse(KeyValue, out tmpIntP) && tmpIntP > 0)
                                                currLeader.Cabinet = tmpIntP;
                                            break;
                                        case "CDes":
                                            currLeader.CDes = KeyValue;
                                            break;
                                        case "CDes2":
                                            currLeader.CDes2 = KeyValue;
                                            break;
                                        case "Cond":
                                            if (string.IsNullOrEmpty(KeyValue))
                                            {
                                                if (currLeader.Cond1 < 1)
                                                    currLeader.Cond1 = 100;
                                            }
                                            else if (!KeyValue.Contains("-"))
                                            {
                                                if (currLeader.Cond1 < 1)
                                                    if (int.TryParse(KeyValue, out tmpIntP) && tmpIntP > 0)
                                                        currLeader.Cond1 = tmpIntP;
                                                    else
                                                        currLeader.Cond1 = 100;
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(currLeader.Cond2))
                                                    currLeader.Cond2 = KeyValue;
                                                else if (string.IsNullOrEmpty(currLeader.Cond3))
                                                    currLeader.Cond3 = KeyValue;
                                            }
                                            break;
                                        case "Feat":
                                            if (!string.IsNullOrEmpty(KeyValue))
                                            {
                                                if (string.IsNullOrEmpty(currLeader.Feat1))
                                                    currLeader.Feat1 = KeyValue;
                                                else if (string.IsNullOrEmpty(currLeader.Feat2))
                                                    currLeader.Feat2 = KeyValue;
                                                else if (string.IsNullOrEmpty(currLeader.Feat3))
                                                    currLeader.Feat3 = KeyValue;
                                            }
                                            break;
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                isProc = true;
                                tmpData = line.Replace("[LEADER]", "");
                                int LeaderID;
                                if (int.TryParse(tmpData, out LeaderID))
                                {
                                    currLeader.LeaderID = LeaderID;
                                    break;
                                }
                                else
                                {
                                    //錯誤處理，忽略此筆資料
                                    ErrorCount++;
                                    stepProc = 4;
                                    goto case 4;
                                }
                            }
                        }
                    case 2:
                        if (string.IsNullOrEmpty(line))
                        {
                            stepProc = 3;
                            goto case 3;
                        }
                        else
                        {
                            keepDataEnd.AppendLine(line);
                            break;
                        }
                    case 3:
                        break;
                    case 4:
                        if (line == "[/LEADER]")
                        {
                            isProc = false;
                            stepProc = 1;
                            break;
                        }
                        else
                        {
                            break;
                        }
                    default:
                        break;
                }
            }
            //INQtoCSV
            CsvFileDescription outputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',', // tab delimited
                FirstLineHasColumnNames = true, // no column names in first record
                FileCultureName = "zh-TW" // use formats used in The Netherlands
            };
            CsvContext cc = new CsvContext();
            cc.Write(products2, OldCsvPath, outputFileDescription);

            string NewData = ReadFromRemote(NewCsvPath);
            keepData.Append(NewData);
            keepData.Append(keepDataEnd);

            File.WriteAllText(OldTxtPath, keepData.ToString(), UTF8Encoding.UTF8);
            File.WriteAllText(NewTxtPath, keepData.ToString(), UTF8Encoding.UTF8);
            return ErrorCount;
        }
        //讀取人物設定並更新中間部分資料
        string ReadFromRemote(string file)
        {
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true
            };
            CsvContext cc = new CsvContext();
            IEnumerable<Model.Leader> products = cc.Read<Model.Leader>(file, inputFileDescription);

            //依照LeaderID排序來源資料
            products = from p in products orderby p.LeaderID select p;

            string ConText = "";
            int ErrorCount = 0;
            foreach (Model.Leader item in products)
            {
                string CurrData = TransferFormat(item);
                if (string.IsNullOrEmpty(CurrData))
                    ErrorCount++;
                ConText += CurrData;
            }
            return ConText;
        }
        //產生設定檔案
        string TransferFormat(Model.Leader item)
        {
            // LeaderID 必須大於等於0
            if (item.LeaderID < 0)
                return string.Empty;
            // Name 不能為空
            else if (string.IsNullOrEmpty(item.Name))
                return string.Empty;
            StringBuilder SB = new StringBuilder();
            SB.AppendFormat("[LEADER]{0}" + Environment.NewLine, item.LeaderID);
            SB.AppendFormat(" Name={0}" + Environment.NewLine, item.Name);
            // 女性為1，男性不用設定此項
            //if (item.Female == 1)
            //    SB.AppendFormat(" Female={0}", item.Female);
            // 萌國無雙人物必為女性
            SB.AppendLine(" Female=1");
            // 綽號為非必要項目，若為空值則省略
            if (!string.IsNullOrEmpty(item.Alias))
                SB.AppendFormat(" Alias={0}" + Environment.NewLine, item.Alias);
            // 部隊番號或者所在部門名稱，在野狀態時無此項設定，在職狀態時可有可無，為非必要項目，若為空值則省略
            if (!string.IsNullOrEmpty(item.Corps))
                SB.AppendFormat(" Corps={0}" + Environment.NewLine, item.Corps);
            // 歷史頭像編號為非必要項目，若為空值則省略
            if (!string.IsNullOrEmpty(item.Graph))
                SB.AppendFormat(" Graph={0}" + Environment.NewLine, item.Graph);
            // 狀態0為在職，其餘為在野-1
            SB.AppendFormat(" Status={0}" + Environment.NewLine, item.Status == 0 ? 0 : -1);
            //野望值值域為1至20，小於1則為1，大於20則為20
            //if (item.Power < 1)
            //    SB.AppendFormat(" Power={0}", 1);
            //else if (item.Power > 20)
            //    SB.AppendFormat(" Power={0}", 20);
            //else
            //    SB.AppendFormat(" Power={0}", item.Power);
            // 萌國無雙人物野望值最大化
            SB.AppendLine(" Power=20");
            //史實性，史實為1，其餘皆預設為虛構0
            SB.AppendFormat(" History={0}" + Environment.NewLine, item.History == 1 ? 1 : 0);
            //必要項目，預設值為[無]
            SB.AppendFormat(" Faction={0}" + Environment.NewLine, string.IsNullOrEmpty(item.Faction) ? "無" : item.Faction);
            //必要項目，需大於等於1
            SB.AppendFormat(" Will={0}" + Environment.NewLine, item.Will > 0 ? item.Will : 1);
            //現有忠誠度，必要項目，在野或小於0則預設為10，大於200則設為200
            if (item.Status != 0 || item.Loyality < 0)
                SB.AppendFormat(" Loyality={0}", 10);
            else if (item.Loyality > 200)
                SB.AppendFormat(" Loyality={0}", 200);
            else
                SB.AppendFormat(" Loyality={0}", item.Loyality);
            SB.AppendLine();
            //部隊編號，在野狀態為-10
            if (item.Status != 0)
                SB.AppendFormat(" UnitIndex={0}", -10);
            else
                SB.AppendFormat(" UnitIndex={0}", item.UnitIndex);
            SB.AppendLine();
            //大眾臉頭像編號，可供選擇編號為0-9共10張
            if (item.Icon < 0)
                SB.AppendFormat(" Icon={0}", 0);
            else if (item.Icon > 9)
                SB.AppendFormat(" Icon={0}", 9);
            else
                SB.AppendFormat(" Icon={0}", item.Icon);
            SB.AppendLine();
            //錄用方式，隨機0，勢力專屬1，親屬武將2，地方專屬3，不能聘用4
            if (item.RecruitType < 0 || item.RecruitType > 4)
                SB.AppendFormat(" RecruitType={0}", 0);
            else
                SB.AppendFormat(" RecruitType={0}", item.RecruitType);
            SB.AppendLine();
            //必要項目，錄用派生值，預設0
            SB.AppendFormat(" RecruitValue={0}" + Environment.NewLine, item.RecruitValue < 0 ? 0 : item.RecruitValue);
            //預設技能
            SB.AppendFormat(" LeaderSkill0={0}" + Environment.NewLine, string.IsNullOrEmpty(item.LeaderSkill0) ? "HP+10" : item.LeaderSkill0);
            SB.AppendFormat(" LeaderSkill1={0}" + Environment.NewLine, string.IsNullOrEmpty(item.LeaderSkill1) ? "I+1" : item.LeaderSkill1);
            SB.AppendFormat(" LeaderSkill2={0}" + Environment.NewLine, string.IsNullOrEmpty(item.LeaderSkill2) ? "BS+1" : item.LeaderSkill2);
            SB.AppendFormat(" LeaderSkill3={0}" + Environment.NewLine, string.IsNullOrEmpty(item.LeaderSkill3) ? "WS+1" : item.LeaderSkill3);
            SB.AppendFormat(" LeaderSkill4={0}" + Environment.NewLine, string.IsNullOrEmpty(item.LeaderSkill4) ? "Morale+1" : item.LeaderSkill4);
            SB.AppendFormat(" LeaderSkill5={0}" + Environment.NewLine, string.IsNullOrEmpty(item.LeaderSkill5) ? "Defenser" : item.LeaderSkill5);
            //預設人物說明
            SB.AppendFormat(" Description0={0}" + Environment.NewLine, string.IsNullOrEmpty(item.Description0) ? "出沒於萌國無雙的神秘人物之一。" : item.Description0);
            SB.AppendFormat(" Description1={0}" + Environment.NewLine, item.Description1);
            SB.AppendFormat(" Description2={0}" + Environment.NewLine, item.Description2);
            SB.AppendFormat(" Description3={0}" + Environment.NewLine, item.Description3);
            //內閣類型，1是可以內閣，2是只能內閣，純軍人則只設定到列傳
            if (item.Cabinet == 1 || item.Cabinet == 2)
            {
                //通設為可以為內閣
                SB.AppendFormat(" Cabinet={0}" + Environment.NewLine, 1);
                //預設內閣說明
                SB.AppendFormat(" CDes={0}" + Environment.NewLine, string.IsNullOrEmpty(item.CDes) ? "萌國無雙的神祕內閣技能之一。" : item.CDes);
                SB.AppendFormat(" CDes2={0}" + Environment.NewLine, item.CDes2);
                if (!string.IsNullOrEmpty(item.Feat1))
                    SB.AppendFormat(" Feat={0}" + Environment.NewLine, item.Feat1);
                if (!string.IsNullOrEmpty(item.Feat2))
                    SB.AppendFormat(" Feat={0}" + Environment.NewLine, item.Feat2);
                if (!string.IsNullOrEmpty(item.Feat3))
                    SB.AppendFormat(" Feat={0}" + Environment.NewLine, item.Feat3);
                //預設 [聘用為內閣時需要消耗的文化值] 為 [100]
                SB.AppendFormat(" Cond={0}" + Environment.NewLine, item.Cond1 > 0 ? item.Cond1 : 100);
                //預設 [特定條件下可以減少的文化值消耗] 為 [Localism -20] 與 [Warlord -20]
                SB.AppendFormat(" Cond={0}" + Environment.NewLine, string.IsNullOrEmpty(item.Cond2) ? "Localism -20" : item.Cond2);
                SB.AppendFormat(" Cond={0}" + Environment.NewLine, string.IsNullOrEmpty(item.Cond3) ? "Warlord -20" : item.Cond3);
            }
            SB.Append("[/LEADER]" + Environment.NewLine);
            return SB.ToString();
        }
        //複製檔案
        void CopyFiles(string fileName, string sourcePath, string targetPath)
        {
            // Use Path class to manipulate file and directory paths. 
            string sourceFile = Path.Combine(sourcePath, fileName);
            string destFile = Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location: 
            // Create a new target folder, if necessary. 
            if (!Directory.Exists(targetPath))
            {
                Directory.CreateDirectory(targetPath);
            }

            // To copy a file to another location and  
            // overwrite the destination file if it already exists.
        }
        // 下載檔案
        bool downloadFromWeb(string curFile, string remoteUri)
        {
            WebClient myWebClient = new WebClient();
            bool Status = false;
            try
            {
                myWebClient.DownloadFile(remoteUri, curFile);
                Status = true;
            }
            catch
            {
                Status = false;
            }
            return Status;
        }
    }
}
