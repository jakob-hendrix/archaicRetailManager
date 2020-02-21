using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARMDataManager.Library.Helpers
{
    public class ConfigHelper
    {
        // TODO: replace this along with changes to the UI library version
        public static double GetTaxRate() => Properties.Settings.Default.TaxRate;
    }
}