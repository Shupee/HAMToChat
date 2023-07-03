namespace HR
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        public static bool IsConsole = false;
        [STAThread]
        static void Main(string[] args)
        {
            if(args.Contains("console"))
            {
                IsConsole = true;
                Logger.AllocConsole();
            }
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new ToChat());

        }
    }
}