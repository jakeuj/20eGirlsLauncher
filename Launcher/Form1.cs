using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Launcher
{
    public partial class Form1 : Form
    {
        Main main;
        public Form1()
        {
            InitializeComponent();
            main = new Main();
        }
        #region 主功能區
        //開始遊戲
        private void GameStart_Click(object sender, EventArgs e)
        {
            textBox1.Text += main.GameStart_Click();
        }
        //備份遊戲
        private void GameBackup_Click(object sender, EventArgs e)
        {
            textBox1.Text += main.GameBackup_Click();
        }
        //更新遊戲
        private void GameUpdate_Click(object sender, EventArgs e)
        {
            if (main.GameUpdate_Check())
            {
                textBox1.Text += "遊戲更新資料下載開始：等待下載完成..." + Environment.NewLine;
                if (main.GameUpdate_Click())
                    textBox1.Text += "遊戲更新資料下載完成：準備更新遊戲資料..." + Environment.NewLine;
                else
                    textBox1.Text += "遊戲更新資料下載失敗：嘗試離線更新中..." + Environment.NewLine;
                textBox1.Text += main.GameUpdate_Backup();
            }
            else
                textBox1.Text += "找不到遊戲設定檔，請先執行下載遊戲！" + Environment.NewLine;
        }
        // 遊戲下載
        private void GameDL_Click(object sender, EventArgs e)
        {
            textBox1.Text += "遊戲主程式下載開始...等待下載完成..." + Environment.NewLine;
            if (main.GameDownload())
            {
                textBox1.Text += "遊戲主程式下載完成...等待安裝完成..." + Environment.NewLine;
                if (main.GameInstall())
                    textBox1.Text += "遊戲主程式安裝完成...可以開始遊戲..." + Environment.NewLine;
                else
                    textBox1.Text += "遊戲主程式安裝失敗...請手動安裝..." + GameInfo.gameResoureName + Environment.NewLine;
            }
            else
                textBox1.Text += "遊戲主程式下載失敗...請檢查網路環境..." + Environment.NewLine;
        }
        #endregion
        #region 超連結區
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://sites.google.com/site/jakeuj/home");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.facebook.com/MengGuoWuShuang");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.facebook.com/groups/MengGuoWuShuang/");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://drive.google.com/open?id=0B6KzAYjzN_vrbGF2RGZJOTFCN0E");
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://docs.google.com/spreadsheets/d/11RyHZFxdE3Eu2cpK_Np5Rt96I17ajPULWlD_gBUdZ4Y/edit?usp=sharing");
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:jakeuj@hotmail.com");
        }
        #endregion
    }
}
