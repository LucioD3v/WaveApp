using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WaveApp.Services;

namespace WaveApp.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        [ObservableProperty] private string username;
        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private string confirmPassword;
        [ObservableProperty] private string errorMessage;
        [ObservableProperty] private bool hasError;
        [ObservableProperty] private bool isBusy;

        public IRelayCommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            RegisterCommand = new AsyncRelayCommand(RegisterAsync, () => !IsBusy);
        }

        private async Task RegisterAsync()
        {
            HasError = false;
            IsBusy = true;

            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) ||
                string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                ErrorMessage = "All fields are required.";
                HasError = true;
                IsBusy = false;
                return;
            }

            if (Password != ConfirmPassword)
            {
                ErrorMessage = "The passwords do not match.";
                HasError = true;
                IsBusy = false;
                return;
            }

            var result = await AuthService.RegisterAsync(Username, Email, Password);
            if (result)
            {
                await Shell.Current.GoToAsync("//LoginPage");
            }
            else
            {
                ErrorMessage = "Error registering user. Please try again..";
                HasError = true;
            }

            IsBusy = false;
        }
    }
}
