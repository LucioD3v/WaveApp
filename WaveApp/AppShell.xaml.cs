namespace WaveApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            Preferences.Set("IsLoggedIn", false);
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
