using System;
using ARMDesktopUI.EventModels;
using Caliburn.Micro;

namespace ARMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private LoginViewModel _loginVM;
        private SalesViewModel _salesVM;
        private SimpleContainer _container;
        private IEventAggregator _events;

        public ShellViewModel(IEventAggregator events, LoginViewModel loginVM, SalesViewModel salesVM, SimpleContainer container)
        {
            _loginVM = loginVM;
            _salesVM = salesVM;

            _container = container;

            _events = events;
            _events.Subscribe(this);

            ActivateItem(_loginVM);
        }

        public void Handle(LogOnEvent message)
        {
            // only 1 item can be open at a time
            ActivateItem(_salesVM);

            // Override the prior login VM
            _loginVM = _container.GetInstance<LoginViewModel>();
        }
    }
}