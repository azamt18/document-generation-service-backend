using log4net;
using log4net.Config;

namespace API;

public static class Logger
{
    public static void InitLogger()
    {
        XmlConfigurator.Configure();
    }
    
    private static ILog GetLogger(string loggerName)
    {
        return LogManager.GetLogger(loggerName);
    }
    
    public static ILog DefaultLog => GetLogger("DefaultLog");
}