using System.Windows;
using ARMDesktopUI.ViewModels;
using Caliburn.Micro;

namespace ARMDesktopUI
{
    /// <summary>
    /// Used to set up Caliburn and Micro
    /// </summary>
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // On Startup, launch ShellViewModel as our base. This will launch the view.
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}