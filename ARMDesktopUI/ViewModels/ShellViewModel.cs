using System;
using ARMDesktopUI.EventModels;
using ARMDesktopUI.Library.Api;
using ARMDesktopUI.Library.Models;
using Caliburn.Micro;

namespace ARMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogOnEvent>
    {
        private readonly SalesViewModel _salesViewModel;
        private readonly ILoggedInUserModel _user;
        private readonly IApiHelper _apiHelper;
        private IEventAggregator _events;

        public ShellViewModel(IEventAggregator events, SalesViewModel salesViewModel,
            ILoggedInUserModel user, IApiHelper apiHelper)
        {
            _salesViewModel = salesViewModel;
            _user = user;
            _apiHelper = apiHelper;
            _events = events;

            _events.Subscribe(this);

            // each request gets a new instance. The login view will go away upon activation of a new item
            ActivateItem(IoC.Get<LoginViewModel>());
        }

        public void ExitApplication() => TryClose();

        public void UserManagement()
        {
            ActivateItem(IoC.Get<UserDisplayViewModel>());
        }

        public void SignIn()
        {
            _user.InitializeUserModel();
            _apiHelper.LogOffUser();
            ActivateItem(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsUserSignedOn);
        }

        public void SignOut() => SignIn();

        public bool IsUserSignedOn => !string.IsNullOrEmpty(_user.Token);

        public void Handle(LogOnEvent message)
        {
            // we want to hold this in memory so we don't lose the cart state when changing context
            ActivateItem(_salesViewModel);
            NotifyOfPropertyChange(() => IsUserSignedOn);
        }
    }
}