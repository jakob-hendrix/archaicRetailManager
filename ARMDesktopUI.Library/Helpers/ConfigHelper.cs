namespace ARMDesktopUI.Library.Helpers
{
    public class ConfigHelper : IConfigHelper
    {
        // TODO: move this to the api
        public double GetTaxRate() => Properties.Settings.Default.TaxRate;
    }
}