using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace WaveApp.Views;

public partial class LoginPage : ContentPage, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private string _email;
    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsEmailInvalid));
        }
    }

    private string _password;
    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsPasswordInvalid));
        }
    }

    public bool IsEmailInvalid => !string.IsNullOrWhiteSpace(Email) && !Email.Contains("@");
    public bool IsPasswordInvalid => !string.IsNullOrWhiteSpace(Password) && Password.Length < 6;

    private ICommand _loginCommand;
    public ICommand LoginCommand
    {
        get => _loginCommand;
        set
        {
            _loginCommand = value;
            OnPropertyChanged();
        }
    }
    private ICommand _googleLoginCommand;
    public ICommand GoogleLoginCommand
    {
        get => _googleLoginCommand;
        set
        {
            _googleLoginCommand = value;
            OnPropertyChanged();
        }
    }
    private ICommand _microsoftLoginCommand;
    public ICommand MicrosoftLoginCommand
    {
        get => _microsoftLoginCommand;
        set
        {
            _microsoftLoginCommand = value;
            OnPropertyChanged();
        }
    }
    private ICommand _appleLoginCommand;
    public ICommand AppleLoginCommand
    {
        get => _appleLoginCommand;
        set
        {
            _appleLoginCommand = value;
            OnPropertyChanged();
        }
    }
    public ICommand ForgotPasswordCommand { get; }
    public ICommand SignUpCommand { get; }

    public LoginPage()
    {
        InitializeComponent();
        BindingContext = this;

        LoginCommand = new Command(OnLoginClicked);
        GoogleLoginCommand = new Command(OnGoogleLoginClicked);
        MicrosoftLoginCommand = new Command(OnMicrosoftLoginClicked);
        AppleLoginCommand = new Command(OnAppleLoginClicked);
        ForgotPasswordCommand = new Command(OnForgotPasswordClicked);
        SignUpCommand = new Command(OnSignUpClicked);
    }

    private async void OnLoginClicked()
    {
        if (string.IsNullOrWhiteSpace(Email))
        {
            await DisplayAlert("Error", "Please enter your email", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(Password))
        {
            await DisplayAlert("Error", "Please enter your password", "OK");
            return;
        }
        await DisplayAlert("Success", $"Welcome back to WaveForce!\n\nEmail: {Email}", "Continue");
        Application.Current.MainPage = new AppShell();
    }

    private async void OnGoogleLoginClicked()
    {
        // Simulate Google login
        await DisplayAlert("Google Login", "You've chosen to login with Google", "OK");
    }

    private async void OnMicrosoftLoginClicked()
    {
        // Simulate Microsoft login
        await DisplayAlert("Microsoft Login", "You've chosen to login with Microsoft", "OK");
    }

    private async void OnAppleLoginClicked()
    {
        // Simulate Apple login
        await DisplayAlert("Apple Login", "You've chosen to login with Apple", "OK");
    }

    private async void OnForgotPasswordClicked()
    {
        await DisplayAlert("Password Reset", "A password reset link has been sent to your email", "OK");
    }

    private async void OnSignUpClicked()
    {
        await DisplayAlert("Sign Up", "You'll be redirected to the sign up page", "OK");
    }
}