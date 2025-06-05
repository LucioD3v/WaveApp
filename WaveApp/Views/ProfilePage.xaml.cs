using WaveApp.Models;

namespace WaveApp.Views;

public partial class ProfilePage : ContentPage
{
    private User _user;

    public ProfilePage()
    {
        InitializeComponent();

        // Simulate loading user data
        _user = new User
        {
            Name = "John",
            LastName = "Doe",
            ProfilePictureUrl = "https://example.com/profile.jpg",
            Phone = "123-456-7890",
            Email = "john.doe@example.com"
        };

        LoadUserData();
    }

    private void LoadUserData()
    {
        ProfilePicture.Source = _user.ProfilePictureUrl;
        NameLabel.Text = _user.Name;
        LastNameLabel.Text = _user.LastName;
        PhoneLabel.Text = _user.Phone;
        EmailLabel.Text = _user.Email;
    }

    private async void OnEditPhotoClicked(object sender, EventArgs e)
    {
        // Logic to edit profile picture
        await DisplayAlert("Edit Photo", "Feature to edit photo coming soon!", "OK");
    }

    private async void OnEditNameClicked(object sender, EventArgs e)
    {
        var newName = await DisplayPromptAsync("Edit Name", "Enter your new name:", initialValue: _user.Name);
        if (!string.IsNullOrWhiteSpace(newName))
        {
            _user.Name = newName;
            NameLabel.Text = newName;
        }
    }

    private async void OnEditLastNameClicked(object sender, EventArgs e)
    {
        var newLastName = await DisplayPromptAsync("Edit Last Name", "Enter your new last name:", initialValue: _user.LastName);
        if (!string.IsNullOrWhiteSpace(newLastName))
        {
            _user.LastName = newLastName;
            LastNameLabel.Text = newLastName;
        }
    }

    private async void OnEditPhoneClicked(object sender, EventArgs e)
    {
        var newPhone = await DisplayPromptAsync("Edit Phone", "Enter your new phone number:", initialValue: _user.Phone);
        if (!string.IsNullOrWhiteSpace(newPhone))
        {
            _user.Phone = newPhone;
            PhoneLabel.Text = newPhone;
        }
    }

    private async void OnEditEmailClicked(object sender, EventArgs e)
    {
        var newEmail = await DisplayPromptAsync("Edit Email", "Enter your new email address:", initialValue: _user.Email);
        if (!string.IsNullOrWhiteSpace(newEmail))
        {
            _user.Email = newEmail;
            EmailLabel.Text = newEmail;
        }
    }
}