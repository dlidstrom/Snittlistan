using System;
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
    }
}