using WaveApp.Views;

namespace WaveApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        protected override void OnStart()
        {
            if (Preferences.Get("IsLoggedIn", false))
                MainPage = new AppShell();
            else
                MainPage = new NavigationPage(new LoginPage());
        }
    }
}