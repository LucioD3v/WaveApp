namespace WaveApp.Services
{
    public static class AuthService
    {
        private static readonly string demoUser = "demo";
        private static readonly string demoPass = "1234";

        public static Task<bool> LoginAsync(string username, string password)
        {
            // Authentication simulation
            return Task.FromResult(username == demoUser && password == demoPass);
        }
    }
}
