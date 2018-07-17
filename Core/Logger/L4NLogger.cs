using log4net.Appender;
using log4net.Core;
using log4net.Layout;
using log4net.Repository;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;

namespace AccuLogger
{
    public class L4NLogger
    {
        private static string LOG_LEVEL = Environment.GetEnvironmentVariable("LOG_LEVEL");
        private static string LOG_PATTERN = Environment.GetEnvironmentVariable("LOG_PATTERN");

        private static Dictionary<Type, log4net.ILog> loggers = new Dictionary<Type, log4net.ILog>();
        private static object objLock = new object();
        public static log4net.ILog CreateLogger(Type type)
        {
            XmlDocument log4netConfig = new XmlDocument();
            log4netConfig.Load(System.IO.File.OpenRead("log4net.config"));
            ILoggerRepository repo = log4net.LogManager.CreateRepository(Assembly.GetEntryAssembly(), typeof(log4net.Repository.Hierarchy.Hierarchy));
            log4net.Config.XmlConfigurator.Configure(repo, log4netConfig["log4net"]);

            //set log pattern
            if (!string.IsNullOrEmpty(LOG_PATTERN))
            {
                ((((Hierarchy)repo).GetAppenders()[0] as ConsoleAppender).Layout as PatternLayout).ConversionPattern = LOG_PATTERN;
                ((((Hierarchy)repo).GetAppenders()[0] as ConsoleAppender).Layout as PatternLayout).ActivateOptions();
                //((((Hierarchy)repo).GetAppenders()[1] as RollingFileAppender).Layout as PatternLayout).ConversionPattern = "[%thread] %-5level %logger - %message%newline";
                //((((Hierarchy)repo).GetAppenders()[1] as RollingFileAppender).Layout as PatternLayout).ActivateOptions();
            }


            //set log level
            switch (LOG_LEVEL)
            {
                case "ALL": ((Hierarchy)repo).Root.Level = Level.All;break;
                case "DEBUG": ((Hierarchy)repo).Root.Level = Level.Debug; break;
                case "INFO": ((Hierarchy)repo).Root.Level = Level.Info; break;
                case "WARN": ((Hierarchy)repo).Root.Level = Level.Warn; break;
                case "ERROR": ((Hierarchy)repo).Root.Level = Level.Error; break;
                case "FATAL": ((Hierarchy)repo).Root.Level = Level.Fatal; break;
                case "OFF": ((Hierarchy)repo).Root.Level = Level.Off; break;
                default: ((Hierarchy)repo).Root.Level = Level.Info; break;
            }

            log4net.ILog logger = log4net.LogManager.GetLogger(type); //System.Reflection.MethodBase.GetCurrentMethod().DeclaringType
            logger.Info(string.Format("Logger INIT   LogLevel: {0}  LogPattern: {1}", LOG_LEVEL, LOG_PATTERN));

            loggers.Add(type, logger);
            return logger;
        }

        public static log4net.ILog GetLogger(Type type)
        {
            lock (objLock)
            {
                if (loggers.ContainsKey(type))
                {
                    return loggers[type];
                }
                else
                {
                    return CreateLogger(type);
                }
            }
        }
    }
}
