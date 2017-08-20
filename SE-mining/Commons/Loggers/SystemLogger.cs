using log4net;
using log4net.Config;

namespace SEMining.Commons.Loggers
{
    public static class SystemLogger
    {
        private static ILog log = LogManager.GetLogger("SYSTEM_LOGER");
        public static ILog Log
        {
            get { return log; }
        }

        public static void InitLogger()
        {
            XmlConfigurator.Configure();
        }
    }
}
