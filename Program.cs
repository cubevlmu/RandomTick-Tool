using Name3.ApplicationCommon;
using Name3.Models;
using Name3.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Name3
{
    static class Program
    {

        [STAThread]
        public static void Main()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            
            GC.Collect();
            GC.WaitForFullGCComplete();

            var timer = new Timer { Interval =  5 * 60 * 1000 };
            timer.Tick += (s, e) => Win32Import.CollectRam();
            timer.Start();

            var Config = AppConfigration.Get();
            Config.Init();

            Control.CheckForIllegalCrossThreadCalls = false;

            using (ApplicationInit initer = new ApplicationInit())
            {
                if (initer.Init())
                {
                    Application.VisualStyleState = System.Windows.Forms.VisualStyles.VisualStyleState.ClientAndNonClientAreasEnabled;
                    Application.EnableVisualStyles();
                    Application.Run(new Form1());
                } else
                {
                    MessageBox.Show(
                        "Error When Init Application \n Try To Start Later xD \n ",
                        "Error To Init App",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Hand
                        );
                }
            }
        }

        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string resourceName = "Name3.Library." + new AssemblyName(args.Name).Name + ".dll";

            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (stream != null)
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
                return null;
            }
        }

        public static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            string start = $"{DateTime.Now} \n Program Exception Error \n";

            var builder = new StringBuilder();
            builder.AppendLine(start)
                .AppendLine()
                .Append("Error Recieve From UI Thread")
                .AppendLine($"Error Type {e.Exception.Message}")
                .AppendLine($"Error Source {e.Exception.Source}")
                .AppendLine($"Error Trace {e.Exception.StackTrace}")
                .AppendLine()
                .AppendLine("Please REPORT THIS TO FLYBIRD STUDIO")
                .AppendLine("THANKS And The PROGRAM WILL BE REASTART!");

            MessageBox.Show(builder.ToString(), "PROGRAM ERROR TRACE", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception error = e.ExceptionObject as Exception;
            string start = $"{DateTime.Now} \n Program Exception Error \n";

            var builder = new StringBuilder();
            builder.AppendLine(start)
                .AppendLine()
                .AppendLine($"Error Type {error.Message}")
                .AppendLine($"Error Source {error.Source}")
                .AppendLine($"Error Trace {error.StackTrace}")
                .AppendLine()
                .AppendLine("Please REPORT THIS TO FLYBIRD STUDIO")
                .AppendLine("THANKS And The PROGRAM WILL BE REASTART!");

            MessageBox.Show(builder.ToString(), "PROGRAM ERROR TRACE", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
