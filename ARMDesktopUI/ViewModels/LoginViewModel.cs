using System;
using System.Threading.Tasks;
using ARMDesktopUI.EventModels;
using ARMDesktopUI.Library.Api;
using Caliburn.Micro;

namespace ARMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private readonly IApiHelper _apiHelper;
        private readonly IEventAggregator _events;
        private string _userName = "jakob.hendrix@la.gov";  // TODO: remove this test code
        private string _password = "Pwd12345.";  // TODO: remove this test code
        private string _errorMessage;

        public LoginViewModel(IApiHelper apiHelper, IEventAggregator events)
        {
            _apiHelper = apiHelper;
            _events = events;
        }

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
                NotifyOfPropertyChange(() => IsErrorVisible);
            }
        }

        public bool IsErrorVisible
        {
            get
            {
                bool output = false;
                if (ErrorMessage?.Length > 0)
                {
                    output = true;
                }
                return output;
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool output = false;

                // TODO: validate better
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task LogIn()
        {
            try
            {
                ErrorMessage = string.Empty;
                var result = await _apiHelper.Authenticate(UserName, Password);

                // Update our user data
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);

                // Let the program know someone has logged on
                _events.BeginPublishOnUIThread(new LogOnEvent());
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }
    }
}