using WaveApp.Services;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WaveApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        [ObservableProperty] private string username;
        [ObservableProperty] private string password;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool hasError;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new AsyncRelayCommand(LoginAsync);
            RegisterCommand = new RelayCommand(Register);
        }

        private async Task LoginAsync()
        {
            HasError = false;
            var result = await AuthService.LoginAsync(Username, Password);
            if (result)
            {
                Preferences.Set("IsLoggedIn", true);
                await Shell.Current.GoToAsync("//DashboardPage");
            }
            else
            {
                ErrorMessage = "Incorrect username or password";
                HasError = true;
            }
        }

        private void Register()
        {
            if (Shell.Current != null)
            {
                Shell.Current.GoToAsync("RegisterPage");
            }
            else
            {
                
            }
        }
    }
}
