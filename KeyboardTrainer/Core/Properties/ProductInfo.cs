using System;

namespace Twidlle.KeyboardTrainer
{
    public static class ProductInfo
    {
        public const string Name = "Twidlle Keyboard Trainer";
        public const string Version = "2.0.8.*";
        public const string Culture = "";
        public const string Year = "2018";
        public const string Copyright = "Copyright © " + Year + " " + CompanyInfo.Name;
#if Debug
        public const String Configuration = "Debug";
#elif Debug_Ru     
    public const String Configuration = "Debug_Ru";
#elif Release
    public const String Configuration = "Release";
#elif Release_Ru
    public const String Configuration = "Release_Ru";
#else
    #error Unknown configuration
#endif
    }
}
