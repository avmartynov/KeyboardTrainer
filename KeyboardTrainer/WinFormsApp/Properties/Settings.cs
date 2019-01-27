using System;
using System.IO;
using Twidlle.Infrastructure;
using Twidlle.KeyboardTrainer.Core;

namespace Twidlle.KeyboardTrainer.WinFormsApp.Properties
{
    internal sealed partial class Settings
    {
        public Settings()
        {
            if (!String.IsNullOrEmpty(LastFile))
                return;

            var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var dir = Path.Combine(myDocs, ApplicationInfo.ProductName);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            LastFile = Path.Combine(dir, Workout.DefaultFileName);
        }
    }
}
