namespace Peernet.Browser.Models.Domain
{
    public enum ProfileField
    {
        ProfileFieldName = 0, // Arbitrary username
        ProfileFieldEmail = 1, // Email address
        ProfileFieldWebsite = 2, // Website address
        ProfileFieldTwitter = 3, // Twitter account without the @
        ProfileFieldYouTube = 4, // YouTube channel URL
        ProfileFieldAddress = 5, // Physical address
        ProfilePicture = 6 // Profile picture, blob
    }
}