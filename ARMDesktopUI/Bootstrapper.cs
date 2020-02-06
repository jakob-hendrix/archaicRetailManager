using System;
using System.Collections.Generic;
using System.Linq;
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

            // Use Reflection to wire up our views and viewmodels
            GetType().Assembly
                .GetTypes()                                     // get every type in our application
                .Where(type => type.IsClass)                    // limit to classes
                .Where(type => type.Name.EndsWith("ViewModel")) // limit to those whose name ends with "ViewModel"
                .ToList()                                       // Make enumerable
                .ForEach(viewModelType =>                       // take my container and register per-request. Could be <Interface, Name-Key, Implementation> which may help with testing
                    _container.RegisterPerRequest(viewModelType, viewModelType.ToString(), viewModelType));
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