using System;
using System.Collections.Generic;
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
        private SimpleContainer _container = new SimpleContainer();

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            // when asking for a SimpleContainer, this returns the instance of itself
            _container.Instance(_container);

            // Caliburn & Micro config.
            // Warning - avoid Singletons if possible. In general, PerRequest
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            // On Startup, launch ShellViewModel as our base. This will launch the view.
            DisplayRootViewFor<ShellViewModel>();
        }

        // Pass in a Type and a name, and return an instance from the container
        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}