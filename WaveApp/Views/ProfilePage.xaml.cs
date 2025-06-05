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
            Name = "Vicente Guzman",
            Bio = "Software Developer",
            ProfilePictureUrl = "https://vicenteguzman.com/assets/icons/icon2.jpg",
            Phone = "+52 55 4348 4042",
            Email = "vicente.g.guzman@accenture.com"
        };

        LoadUserData();
    }

    private void LoadUserData()
    {
        ProfilePicture.Source = _user.ProfilePictureUrl;
        NameEntry.Text = _user.Name; // Updated to match the XAML Entry control
        BioEditor.Text = _user.Bio; // Updated to match the XAML Editor control
        PhoneEntry.Text = _user.Phone; // Updated to match the XAML Entry control
        EmailEntry.Text = _user.Email; // Updated to match the XAML Entry control
    }

    private async void OnEditPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select a profile picture",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                // Update the profile picture
                var stream = await result.OpenReadAsync();
                ProfilePicture.Source = ImageSource.FromStream(() => stream);

                // Optionally, save the new image path or stream to the user object
                _user.ProfilePictureUrl = result.FullPath;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while selecting the photo: {ex.Message}", "OK");
        }
    }

    private async void OnSaveChangesClicked(object sender, EventArgs e)
    {
        _user.Name = NameEntry.Text;
        _user.Bio = BioEditor.Text; 
        _user.Phone = PhoneEntry.Text;
        _user.Email = EmailEntry.Text;

        await DisplayAlert("Success", "Changes saved successfully!", "OK");
    }
}