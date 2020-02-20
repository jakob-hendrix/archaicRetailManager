using System;
using System.Configuration;
using Microsoft.SqlServer.Server;

namespace ARMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public double GetTaxRate()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string taxRate = config.AppSettings.Settings["taxRate"].Value;

            //var configRate = config.AppSettings["taxRate"];

            //var keyList = ConfigurationManager.AppSettings.AllKeys;

            //foreach (var key in keyList)
            //{
            //    var keyValue = ConfigurationManager.AppSettings[key];
            //}

            bool isValidTaxRate = Double.TryParse(taxRate, out double output);
            if (!isValidTaxRate)
            {
                throw new ConfigurationErrorsException("The tax rate is not set up properly");
            }

            return output;
        }
    }
}