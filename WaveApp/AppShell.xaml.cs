using WaveApp.Views;
using System.Windows.Input;

namespace WaveApp
{
    public partial class AppShell : Shell
    {
        public ICommand LogoutCommand { get; }
      

        [Obsolete]
        public AppShell()
        {
            InitializeComponent();

            LogoutCommand = new Command(async () =>
            {
                Application.Current.MainPage = new NavigationPage(new LoginPage());

            });

           

            BindingContext = this;
        }
    }
}