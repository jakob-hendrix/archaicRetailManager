using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ARMDesktopUI.Helpers;
using ARMDesktopUI.Library.Api;
using ARMDesktopUI.Library.Helpers;
using ARMDesktopUI.Library.Models;
using ARMDesktopUI.Models;
using ARMDesktopUI.ViewModels;
using AutoMapper;
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

            //From: https://stackoverflow.com/questions/30631522/caliburn-micro-support-for-passwordbox
            ConventionManager.AddElementConvention<PasswordBox>(
                PasswordBoxHelper.BoundPasswordProperty,
                "Password",
                "PasswordChanged");
        }

        private IMapper ConfigureAutomapper()
        {
            // Setup automapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductModel, ProductDisplayModel>();
                cfg.CreateMap<CartItemModel, CartItemDisplayModel>();
            });

            // Create a mapper and insert into DI
            return config.CreateMapper();
        }

        protected override void Configure()
        {
            _container.Instance(ConfigureAutomapper());  // singleton

            // when asking for a SimpleContainer, this returns the instance of itself
            _container.Instance(_container)
                .PerRequest<IUserEndpoint, UserEndpoint>()
                .PerRequest<ISaleEndpoint, SaleEndpoint>()
                .PerRequest<IProductEndpoint, ProductEndpoint>();

            // Caliburn & Micro config.
            // Warning - avoid Singletons if possible. In general, PerRequest
            _container
                .Singleton<IWindowManager, WindowManager>()
                .Singleton<IEventAggregator, EventAggregator>()
                .Singleton<ILoggedInUserModel, LoggedInUserModel>()
                .Singleton<IConfigHelper, ConfigHelper>()
                .Singleton<IApiHelper, ApiHelper>();

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
        protected override object GetInstance(Type service, string key) => _container.GetInstance(service, key);

        protected override IEnumerable<object> GetAllInstances(Type service) => _container.GetAllInstances(service);

        protected override void BuildUp(object instance) => _container.BuildUp(instance);
    }
}