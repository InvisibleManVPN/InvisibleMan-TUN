using System;
using Microsoft.Win32;

namespace InvisibleManTUN.Handlers.Profiles
{
    public class WindowsProfile : IProfile
    {
        private const string NETWORK_PROFILE = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\NetworkList\Profiles";
        private const string PROFILE_NAME = "ProfileName";

        public void CleanupProfiles(string profileName)
        {
            try
            {
                RegistryKey baseKey = Registry.LocalMachine.OpenSubKey(
                    name: NETWORK_PROFILE, 
                    writable: true
                );

                if (!IsRegistryKeyExists(baseKey))
                    return;
                
                string[] subKeyNames = baseKey.GetSubKeyNames();

                foreach(string subKeyName in subKeyNames)
                {
                    RegistryKey subKey = baseKey.OpenSubKey(
                        name: subKeyName,
                        writable: true
                    );

                    if (!IsRegistryKeyExists(subKey))
                        continue;
                    
                    object profileValue = subKey.GetValue(PROFILE_NAME);

                    if (profileValue != null && profileValue is string prfileValueString)
                        if (ShouldRemoveProfile(prfileValueString))
                            baseKey.DeleteSubKey(subKeyName);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

            bool IsRegistryKeyExists(RegistryKey key) => key != null;

            bool ShouldRemoveProfile(string name) => name.StartsWith(profileName);
        }
    }
}