using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Security;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Reflection;
using System.Security.Principal;

namespace RDPChecker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String machine = "."; // local machine
            String log = "security";
            EventLog aLog = new EventLog(log, machine);
            EventLogEntry entry;
            EventLogEntryCollection entries = aLog.Entries;
            Stack<EventLogEntry> stack = new Stack<EventLogEntry>();
            for (int i = 0; i < entries.Count; i++)
            {
                entry = entries[i];
                stack.Push(entry);
                listBox1.Items.Add(string.Format("{0}{1}", entry.TimeWritten, entry.Message));
            }
            entry = stack.Pop(); // only display the last record

            //listBox1.Items.Add(entry.Index);

            //Console.WriteLine("[Index]\t" + entry.Index +
            //                        "\n[EventID]\t" + entry.InstanceId +
            //                        "\n[TimeWritten]\t" + entry.TimeWritten +
            //                        "\n[MachineName]\t" + entry.MachineName +
            //                        "\n[Source]\t" + entry.Source +
            //                        "\n[UserName]\t" + entry.UserName +
            //                        "\n[Message]\t" + entry.Message +

        }



        private void Form1_Load(object sender, EventArgs e)
        {
            if (!IsRunningAsAdministrator())
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo(Assembly.GetEntryAssembly().CodeBase);
                {
                    var withBlock = processStartInfo;
                    withBlock.UseShellExecute = true;
                    withBlock.Verb = "runas";
                    Process.Start(processStartInfo);
                    Application.Exit();
                }
            }
            else
                this.Text += " " + "(관리자 권한)";
        }

        public bool IsRunningAsAdministrator()
        {
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(windowsIdentity);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }

    }
}
