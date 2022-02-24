namespace NETCoreProvisionB2CUsersAPI.Models
{
    public class B2CUserModel
    {
        public string DisplayName { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public List<string> Identities { get; set; }
    }
}
