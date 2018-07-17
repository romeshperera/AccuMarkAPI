using Microsoft.Extensions.Logging;
using System;

namespace AccuLogger
{
    public class CwLogger
    {
        private string logArea = string.Empty;

        public CwLogger(string LogArea)
        {
            logArea = LogArea;
        }

        public void Log(LogLevel logLevel,string message)
        {
            string level = String.Empty;
            switch(logLevel)
            {
                case LogLevel.None: level = "[NONE]"; break;
                case LogLevel.Trace: level = "[TRACE]"; break;
                case LogLevel.Debug: level = "[DEBUG]"; break;
                case LogLevel.Information: level = "[INFO]"; break;
                case LogLevel.Warning: level = "[WARN]"; break;
                case LogLevel.Error: level = "[ERROR]"; break;
                case LogLevel.Critical: level = "[CRTCL]"; break;
            }

            if(logLevel != LogLevel.None)
            {

            }
        }


    }
}
