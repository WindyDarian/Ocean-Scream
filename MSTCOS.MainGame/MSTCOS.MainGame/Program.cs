using System;
using MSTCOS.Settings;

namespace MSTCOS.MainGame
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        
        [STAThread]
        static void Main(string[] args)
        {
            SettingData data;
            try
            {
                data = Base.StorageManager.LoadData<SettingData>(@"Config.mstcos");
            }
            catch
            {
                data = new SettingData();
            }
            if (args.Length<=0)
            {
                args = new string[2];
                args[0] = data.IP;
                args[1] = data.Port;
            }
            using (Game1 game = new Game1(args,data.ResolutionX,data.ResolutionY,data.AmbientOn))
            {
                game.Run();
            }
            //Environment.Exit(1);
        }
    }
#endif
}