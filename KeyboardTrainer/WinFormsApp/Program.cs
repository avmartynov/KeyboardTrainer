using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

using Twidlle.Infrastructure;
using Twidlle.Infrastructure.CodeAnnotation;
using Twidlle.Infrastructure.WinForms;
using Twidlle.KeyboardTrainer.WinFormsApp.Forms;
using Twidlle.KeyboardTrainer.WinFormsApp.Properties;

namespace Twidlle.KeyboardTrainer.WinFormsApp
{
    public class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            try
            {
                SetUICulture();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                var mainForm = new MainForm(args.Length < 1 ? null : args[0]);

                Application.ThreadException += (s, a) => 
                    {
                        ShowExceptionDialog(mainForm, a.Exception);
                        Environment.Exit(1);
                    };

                Application.Run(mainForm);
            }
            catch (Exception x)
            {
                ShowExceptionDialog(null, x);
            }
        }


        private static void SetUICulture()
        {
            var envVarName = ApplicationInfo.ProductName.Replace(" ", "") + "Language";
            var culture    = Environment.GetEnvironmentVariable(envVarName);
            if (culture != null)
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
        }


        public static void ShowExceptionDialog(IWin32Window owner, [NotNull] Exception exception)
            => exception.ShowMessageBox(owner, Resources.ExceptionDialogMessageFormat, Resources.ExceptionDialogCaption);
    }
}
