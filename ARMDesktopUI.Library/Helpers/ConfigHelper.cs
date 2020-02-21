using System;
using System.Configuration;
using Microsoft.SqlServer.Server;

namespace ARMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        public double GetTaxRate() => Properties.Settings.Default.TaxRate;
    }
}