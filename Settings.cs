using AL.Tools;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace AL.Tools
{
    public static class Settings
    {
        /// <summary>
        /// Property to get the first executable path.
        /// Gets the main path of the application.
        /// </summary>
        public static string AssemblyPath
        {
            get { return System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\"; }
        }

        /// <summary>
        /// Property to get the application path of a web application.
        /// </summary>
        public static string ApplicationPath { get { return AppDomain.CurrentDomain.BaseDirectory; } }

        /// <summary>
        /// Method to retreive the connection string from the App.Config or Web.Config
        /// </summary>
        /// <param name="ConnectionName"></param>
        /// <returns></returns>
        public static String ConnectionString(String ConnectionName)
        {
            //Check if is Web Application or Desktop Application
            var cs = HttpRuntime.AppDomainAppId.IsNull() ? ConfigurationManager.ConnectionStrings[ConnectionName] : WebConfigurationManager.ConnectionStrings[ConnectionName];

            if (cs.IsNull())
                return null;
            else
                return cs.ConnectionString;
        }

        /// <summary>
        /// Used to read a value from the appSettings section from either Web.config or App.config
        /// </summary>
        /// <param name="Key"></param>
        /// <returns>String</returns>
        public static String AppSettings(String Key)
        {
            //Check if is Web Application or Desktop Application
            if (HttpRuntime.AppDomainAppId.IsNull())
                return ConfigurationManager.AppSettings[Key]?.ToString();
            else
                return WebConfigurationManager.AppSettings[Key]?.ToString();
        }

        /// <summary>
        /// Used to save a value in the appSettings section from either Web.config or App.config
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public static void AppSettings(String Key, String Value)
        {
            //Check if is Web Application or Desktop Application
            if (HttpRuntime.AppDomainAppId.IsNull())
                ConfigurationManager.AppSettings[Key] = Value;
            else
                WebConfigurationManager.AppSettings[Key] = Value;
        }

        /// <summary>
        /// Returns version number in format X.X.X.X                                                                   
        /// </summary>
        /// <returns></returns>
        public static string Version
        {
            get
            {
                return Assembly.GetEntryAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// Returns the build number in format yyyyMMdd
        /// </summary>
        public static string BuildNumber
        {
            get
            {
                var ver = Assembly.GetEntryAssembly().GetName().Version;
                var StartDate = new DateTime(2000, 1, 1);

                //Starts with year 2000, then adds time span of one 'Build' per day + 2 Ticks/Second from 'Revision'
                var BuildTicks = (TimeSpan.TicksPerDay * ver.Build) + (TimeSpan.TicksPerSecond * 2 * ver.Revision);
                StartDate = StartDate.Add(new TimeSpan(BuildTicks));

                //We don't want time of day in build no.
                return StartDate.ToString("yyyyMMdd");
            }
        }
    }
}
