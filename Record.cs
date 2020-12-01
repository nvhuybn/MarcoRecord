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
        private void btnStartRecord_Click(object sender, EventArgs e)
        {
            try
            {
                Thread.Sleep(3000);
                var time = txtTime.Text.To<int>();
                //SendKeys.Send("{F9}");
                // Process[] pname = Process.GetProcesses();
                Process obs = Process.GetProcessesByName("obs64").FirstOrDefault();
                Process chrome = Process.GetProcessesByName("chrome").FirstOrDefault();

                if (obs != null && chrome != null)
                {
                    IntPtr obsh = obs.MainWindowHandle;
                    IntPtr chromeh = chrome.MainWindowHandle;
                    SetForegroundWindow(obsh);
                    SendKeys.Send("{F9}");
                    SetForegroundWindow(chromeh);
                    if (time > 0)
                    {
                        Thread.Sleep(time * 1000);
                        SetForegroundWindow(obsh);
                        SendKeys.Send("{F10}");
                    }
                    if (ckShutdown.Checked)
                    {
                        MessageBox.Show("Tắt máy", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //Process.Start("shutdown", "/s /t 0");
                    }
                    else
                    {
                        MessageBox.Show("Không tắt máy", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
                Process p = Process.GetProcessesByName("obs64").FirstOrDefault();
                if (p != null)
                {
                    IntPtr h = p.MainWindowHandle;
                    SetForegroundWindow(h);
                    SendKeys.Send("{F10}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
