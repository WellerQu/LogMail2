using Poster.LogMail.Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel.LogMail.Business;

namespace View.LogMail.App
{
    static class Program
    {
        [System.STAThreadAttribute()]
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        static void Main(string[] args)
        {
            Console.WriteLine("Hello LogMail");

            if (args.Length > 0 && args[0] == "-p")
            {
                string password = args[1];
                DirectoryInfo dir = new DirectoryInfo(Path.Combine(FormViewModel.StartupPath, FormViewModel.ArchiveDirectory));

                Email mail = new Email(password);
                mail.Send(dir, true);
            }
            else
            {
                App app = new App();
                app.MainWindow = new MainWindow();
                app.MainWindow.Show();
                app.Run();
            };
        }
    }
}
