using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using ARMDesktopUI.Library.Api;
using ARMDesktopUI.Library.Models;
using Caliburn.Micro;

namespace ARMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {
        private readonly IUserEndpoint _userEndpoint;
        private readonly StatusInfoViewModel _status;
        private readonly IWindowManager _windowManager;
        private BindingList<UserModel> _users;
        private UserModel _selectedUser;
        private string _selectedUserName;
        private BindingList<string> _userRoles = new BindingList<string>();
        private BindingList<string> _availableRoles = new BindingList<string>();
        private string _selectedUserRole;
        private string _selectedAvailableRole;

        public UserDisplayViewModel(IUserEndpoint userEndpoint, StatusInfoViewModel status, IWindowManager windowManager)
        {
            _userEndpoint = userEndpoint;
            _status = status;
            _windowManager = windowManager;
        }

        public BindingList<UserModel> Users
        {
            get => _users;
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }

        public UserModel SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                SelectedUserName = value.Email;

                var roles = value.Roles.Select(x => x.Value).ToList();
                UserRoles = new BindingList<string>(roles);
                LoadRemainingRoles();
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        public string SelectedUserName
        {
            get => _selectedUserName;
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }

        public BindingList<string> UserRoles
        {
            get => _userRoles;
            set
            {
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        public BindingList<string> AvailableRoles
        {
            get => _availableRoles;
            set
            {
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        public string SelectedUserRole
        {
            get => _selectedUserRole;
            set
            {
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
            }
        }

        public string SelectedAvailableRole
        {
            get => _selectedAvailableRole;
            set
            {
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }

        public async Task AddSelectedRole()
        {
            try
            {
                await _userEndpoint.AddRoleToUser(SelectedUser.Id, SelectedAvailableRole);
                UserRoles.Add(SelectedAvailableRole);
                AvailableRoles.Remove(SelectedAvailableRole);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task RemoveSelectedRole()
        {
            try
            {
                await _userEndpoint.RemoveRoleFromUser(SelectedUser.Id, SelectedUserRole);
                UserRoles.Remove(SelectedUserRole);
                AvailableRoles.Add(SelectedUserRole);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task LoadRemainingRoles()
        {
            // todo: better linq method?
            var roles = await _userEndpoint.GetAllRoles();
            foreach (var role in roles)
            {
                if (UserRoles?.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                ShowErrorDialog(ex);
                TryClose();
            }
        }

        private void ShowErrorDialog(Exception ex)
        {
            dynamic settings = new ExpandoObject();
            settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            settings.ResizeMode = ResizeMode.NoResize;
            settings.Title = "System Error";

            if (ex.Message == "Unauthorized")
            {
                _status.UpdateMessage("Unauthorized Access",
                    "You do not have the correct permission for this action.\n\nPlease contact your system administrator.");
                _windowManager.ShowDialog(_status, null, settings);
            }
            else
            {
                _status.UpdateMessage("Fatal Exception", $"{ex.Message}");
                _windowManager.ShowDialog(_status, null, settings);
            }
        }

        private async Task LoadUsers()
        {
            var userList = await _userEndpoint.GetAll();
            Users = new BindingList<UserModel>(userList);
        }
    }
}