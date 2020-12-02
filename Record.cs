using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MarcoRecord
{
    public partial class Record : Form
    {
        public Record()
        {
            InitializeComponent();
        }
        public class RecordResult
        {
            public int Status;
            public string Error;
        }
        public class LinkFilm
        {
            public int Time;
            public string Link;
        }
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        private System.Windows.Forms.Timer aTimer;
        private int counter = 0;
        private int counterAll = 0;
        //IWebDriver chromeDriver = new ChromeDriver(Path.Combine(Environment.CurrentDirectory, @"\chrome"));
        IWebDriver chromeDriver;
        private void btnStartRecord_Click(object sender, EventArgs e)
        {
            try
            {
                //this.WindowState = FormWindowState.Minimized;
                //Thread.Sleep(3000);
                //var time = txtLink1.Text.To<int>();
                //SendKeys.Send("{F9}");
                //Process[] pname = Process.GetProcesses();
                //return;
                var listFilm = new List<LinkFilm>();
                var link1 = txtLink1.Text;
                var time1 = txtTime1.Text.To<int>();
                var link2 = txtLink2.Text;
                var time2 = txtTime2.Text.To<int>();
                var link3 = txtLink3.Text;
                var time3 = txtTime3.Text.To<int>();
                var link4 = txtLink4.Text;
                var time4 = txtTime4.Text.To<int>();
                var link5 = txtLink5.Text;
                var time5 = txtTime5.Text.To<int>();
                listFilm.Add(new LinkFilm() { Link = link1, Time = time1 });
                listFilm.Add(new LinkFilm() { Link = link2, Time = time2 });
                listFilm.Add(new LinkFilm() { Link = link3, Time = time3 });
                listFilm.Add(new LinkFilm() { Link = link4, Time = time4 });
                listFilm.Add(new LinkFilm() { Link = link5, Time = time5 });
                foreach (var film in listFilm.Where(c => c.Time > 0 && !string.IsNullOrWhiteSpace(c.Link)))
                {
                    try
                    {
                        var result = Recording(film.Link, film.Time);
                        if (result.Status == 1)
                        {
                            chromeDriver.Close();
                            chromeDriver.Dispose();
                            //MessageBox.Show(result.Error, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception)
                    {
                        chromeDriver.Close();
                        chromeDriver.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                chromeDriver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public RecordResult Recording(string link, int time)
        {
            try
            {
                counter = time;
                counterAll += time;
                var options = new ChromeOptions();
                //options.AddExcludedArgument("enable-automation");
                //options.AddAdditionalCapability("useAutomationExtension", false);
                //webBrowser1.Navigate(link1);
                chromeDriver = new ChromeDriver(@"E:\MMO\Project\MarcoRecord\MarcoRecord\MarcoRecord\chrome");
                //chromeDriver = new FirefoxDriver(@"E:\MMO\Project\MarcoRecord\MarcoRecord\MarcoRecord\firefox");
                WebDriverWait wait = new WebDriverWait(chromeDriver, TimeSpan.FromSeconds(20));
                chromeDriver.Navigate().GoToUrl(link);
                while (!IsElementPresent(By.ClassName("jw-reset")))
                {
                    IsElementPresent(By.ClassName("jw-reset"));
                }
                chromeDriver.FindElement(By.ClassName("jw-reset")).Click();
                //Thread.Sleep(30000);
                //wait.Until(x => chromeDriver.FindElement(By.ClassName("jw-icon-fullscreen")));
                //chromeDriver.FindElement(By.ClassName("jw-icon-fullscreen")).Click();
                while (!IsElementPresent(By.ClassName("jw-skippable")))
                {
                    IsElementPresent(By.ClassName("jw-skippable"));
                }
                //wait.Until(x => chromeDriver.FindElement(By.ClassName("jw-skippable")));
                chromeDriver.FindElement(By.ClassName("jw-skippable")).Click();
                //Actions builder = new Actions(chromeDriver);
                //IAction seriesOfActions = builder
                //    .MoveToElement(chromeDriver.FindElement(By.ClassName("jw-skippable")))
                //    .Click()
                //    .Build();
                //seriesOfActions.Perform();
                //Thread.Sleep(10000);

                while (!IsElementPresent(By.ClassName("jw-reset")))
                {
                    IsElementPresent(By.ClassName("jw-reset"));
                }
                //wait.Until(x => chromeDriver.FindElement(By.ClassName("jw-icon-fullscreen")));
                //chromeDriver.FindElement(By.ClassName("jw-icon-fullscreen")).Click();
                chromeDriver.FindElement(By.ClassName("jw-reset")).Click();
                //IAction seriesOfActions2 = builder
                //    .MoveToElement(chromeDriver.FindElement(By.ClassName("jw-reset")))
                //    .Click()
                //    .Build();
                //seriesOfActions2.Perform();
                while (!IsElementPresent(By.ClassName("jw-icon-fullscreen")))
                {
                    IsElementPresent(By.ClassName("jw-icon-fullscreen"));
                }
                chromeDriver.FindElement(By.ClassName("jw-icon-fullscreen")).Click();
                Process qrecord = Process.GetProcessesByName("QRecord").FirstOrDefault();
                Process chrome = Process.GetProcessesByName("chrome").FirstOrDefault();
                if (qrecord != null && chrome != null)
                {
                    IntPtr qrecordh = qrecord.MainWindowHandle;
                    IntPtr chromeh = chrome.MainWindowHandle;
                    SetForegroundWindow(qrecordh);
                    SendKeys.Send("{F6}");
                    SetForegroundWindow(chromeh);
                    if (counter > 0)
                    {
                        aTimer = new System.Windows.Forms.Timer();
                        aTimer.Tick += new EventHandler(aTimer_Tick);
                        aTimer.Interval = 1000; // 1 second
                        aTimer.Start();
                        lblCountDown.Text = counterAll.ToString();
                    }
                    return new RecordResult()
                    {
                        Status = 0
                    };
                }
                else
                {
                    return new RecordResult()
                    {
                        Status = 1,
                        Error = "Chưa mở QRecord hoặc Chrome"
                    };
                }

            }
            catch (Exception ex)
            {
                return new RecordResult() { Status = 1, Error = ex.Message };
            }
        }
        public RecordResult Recording3(string link, int time)
        {
            try
            {
                counter = time;
                counterAll += time;
                Process.Start("chrome.exe",  link + " -new-window");
                Thread.Sleep(3000);
                SendKeys.Send("{TAB}");
                Thread.Sleep(1000);
                SendKeys.Send("{TAB}");
                Thread.Sleep(1000);
                SendKeys.Send("{ENTER}");
                Thread.Sleep(1000);
                Process qrecord = Process.GetProcessesByName("QRecord").FirstOrDefault();
                Process chrome = Process.GetProcessesByName("chrome").FirstOrDefault();
                if (qrecord != null && chrome != null)
                {
                    IntPtr qrecordh = qrecord.MainWindowHandle;
                    IntPtr chromeh = chrome.MainWindowHandle;
                    SetForegroundWindow(qrecordh);
                    SendKeys.Send("{F6}");
                    SetForegroundWindow(chromeh);
                    if (counter > 0)
                    {
                        aTimer = new System.Windows.Forms.Timer();
                        aTimer.Tick += new EventHandler(aTimer_Tick);
                        aTimer.Interval = 1000; // 1 second
                        aTimer.Start();
                        lblCountDown.Text = counterAll.ToString();
                    }
                    return new RecordResult()
                    {
                        Status = 0
                    };
                }
                else
                {
                    return new RecordResult()
                    {
                        Status = 1,
                        Error = "Chưa mở QRecord hoặc Chrome"
                    };
                }
                return new RecordResult()
                {
                    Status = 0
                };

            }
            catch (Exception ex)
            {
                return new RecordResult() { Status = 1, Error = ex.Message };
            }
        }

        private void btnStopRecord_Click(object sender, EventArgs e)
        {
            try
            {
                Process p = Process.GetProcessesByName("QRecord").FirstOrDefault();
                if (p != null)
                {
                    aTimer.Stop();
                    IntPtr h = p.MainWindowHandle;
                    SetForegroundWindow(h);
                    SendKeys.Send("{F8}");
                    chromeDriver.Close();
                    chromeDriver.Dispose();
                    MessageBox.Show("Đã thu xong", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                MessageBox.Show("Không tìm thấy QRecord", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void aTimer_Tick(object sender, EventArgs e)
        {
            counter = counter - 1;
            counterAll = counterAll - 1;
            Process qrecord = Process.GetProcessesByName("QRecord").FirstOrDefault();
            lblCountDown.Text = counterAll.ToString();
            if (counter == 0)
            {
                aTimer.Stop();
                if (qrecord != null)
                {
                    IntPtr qrecordh = qrecord.MainWindowHandle;
                    SetForegroundWindow(qrecordh);
                    SendKeys.Send("{F8}");
                    chromeDriver.Close();
                    chromeDriver.Dispose();
                }
            }
            if (counterAll == 0)
            {
                if (ckShutdown.Checked)
                {
                    //MessageBox.Show("Tắt máy", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Process.Start("shutdown", "-s -t 10");
                }
                else
                {
                    MessageBox.Show("Đã thu xong", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

        }
    }
}
