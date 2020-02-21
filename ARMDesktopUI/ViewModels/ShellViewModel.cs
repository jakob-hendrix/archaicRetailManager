using System;
using ARMDesktopUI.EventModels;
using Caliburn.Micro;

namespace ARMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private SalesViewModel _salesVM;
        private IEventAggregator _events;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesVM)
        {
            _salesVM = salesVM;

            _events = events;
            _events.Subscribe(this);

            // each request gets a new instance. The login view will go away upon activation of a new item
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void Handle(LogOnEvent message)
        {
            // we want to hold this in memory so we don't lose the cart state when changing context
            ActivateItem(_salesVM);
        }
    }
}