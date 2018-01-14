using System;
using System.Collections.Generic;
using System.Configuration;

namespace Snittlistan.Tool.Tasks
{
    public static class CommandLineTaskHelper
    {
        public static void ForAllConnectionStrings(Action<(ConnectionStringSettings, Uri)> action)
        {
            foreach (ConnectionStringSettings connectionStringSettings in ConfigurationManager.ConnectionStrings)
            {
                if (connectionStringSettings.ConnectionString.Contains("Snittlistan"))
                {
                    var url = ConfigurationManager.AppSettings[$"{connectionStringSettings.Name}-api"] ?? throw new Exception($"Specify url using appSettings key {connectionStringSettings.Name}-api");
                    action.Invoke((connectionStringSettings, new Uri(url)));
                }
            }
        }

        public static (ConnectionStringSettings, Uri)[] AllConnectionStrings()
        {
            var list = new List<(ConnectionStringSettings, Uri)>();
            ForAllConnectionStrings(x => list.Add(x));
            return list.ToArray();
        }

        public static void ForAllApiUrls(Action<Uri> action)
        {
            foreach (string name in ConfigurationManager.AppSettings)
            {
                if (name.Contains("Snittlistan-"))
                {
                    var url = ConfigurationManager.AppSettings[name];
                    action.Invoke(new Uri(url));
                }
            }
        }

        public static Uri[] AllApiUrls()
        {
            var list = new List<Uri>();
            ForAllApiUrls(x => list.Add(x));
            return list.ToArray();
        }
    }
}