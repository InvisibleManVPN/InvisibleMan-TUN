namespace InvisibleManTUN.Handlers
{
    using Profiles;

    public class ProfileHandler : Handler
    {
        private readonly IProfile profile;

        public ProfileHandler()
        {
            this.profile = LoadProfile();
        }

        public IProfile GetProfile() => profile;

        private IProfile LoadProfile()
        {
            WindowsProfile windowsProfile = new WindowsProfile();
            return windowsProfile;
        }
    }
}