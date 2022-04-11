using System;

namespace BlogsConsole
{
    class Program
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Logger.Info("Program started");

            try
            {
                BloggingContext context = new BloggingContext(Logger);
            }
            catch (Exception exception)
            {
                Logger.Error(exception.Message);
            }
            
            Logger.Info("Program ended");
        }
    }
}