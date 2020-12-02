using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
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
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        private System.Windows.Forms.Timer aTimer;
        private int counter = 0;
        private void btnStartRecord_Click(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Minimized;
                //Thread.Sleep(3000);
                var time = txtTime.Text.To<int>();
                //SendKeys.Send("{F9}");
                //Process[] pname = Process.GetProcesses();
                //return;
                Process qrecord = Process.GetProcessesByName("QRecord").FirstOrDefault();
                Process chrome = Process.GetProcessesByName("chrome").FirstOrDefault();
                if (qrecord != null && chrome != null)
                {
                    IntPtr qrecordh = qrecord.MainWindowHandle;
                    IntPtr chromeh = chrome.MainWindowHandle;
                    SetForegroundWindow(qrecordh);
                    SendKeys.Send("{F6}");
                    SetForegroundWindow(chromeh);
                    counter = time;
                    if (counter > 0)
                    {
                        aTimer = new System.Windows.Forms.Timer();

                        aTimer.Tick += new EventHandler(aTimer_Tick);

                        aTimer.Interval = 1000; // 1 second

                        aTimer.Start();

                        lblCountDown.Text = counter.ToString();
                        //Thread.Sleep(time * 1000);                        
                    }
                }
                else
                {
                    MessageBox.Show("Chưa mở OBS hoặc Chrome", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //if (time > 0)
                //{
                //    Thread.Sleep(time * 1000);
                //    //SetForegroundWindow(h);
                //    SendKeys.Send("{F10}");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            Process qrecord = Process.GetProcessesByName("QRecord").FirstOrDefault();
            lblCountDown.Text = counter.ToString();
            if (counter == 0)
            {
                aTimer.Stop();
                if (qrecord != null)
                {
                    IntPtr qrecordh = qrecord.MainWindowHandle;
                    SetForegroundWindow(qrecordh);
                    SendKeys.Send("{F8}");
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
}
