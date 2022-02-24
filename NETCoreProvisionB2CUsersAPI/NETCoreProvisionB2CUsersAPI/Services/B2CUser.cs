using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using NETCoreProvisionB2CUsersAPI.Interfaces;
using NETCoreProvisionB2CUsersAPI.Models;

namespace NETCoreProvisionB2CUsersAPI.Services
{
    public class B2CUser : IB2CUser
    {
        #region Vars
        private readonly GraphServiceClient graphClient;
        private readonly B2CTenantSettings b2cTenantSettings;
        #endregion

        #region Public
        public B2CUser(IOptions<B2CTenantSettings> userSettings)
        {
            this.b2cTenantSettings = userSettings.Value;

            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .Create(b2cTenantSettings.ClientId)
                .WithTenantId(b2cTenantSettings.Tenant)
                .WithClientSecret(b2cTenantSettings.ClientSecret)
                .Build();

            ClientCredentialProvider _authProvider = new ClientCredentialProvider(confidentialClientApplication);
            GraphServiceClient _graphClient = new GraphServiceClient(_authProvider);

            graphClient = _graphClient;
        }
        public async Task CreateB2CUser(B2CUserModel b2cUser)
        {
            try
            {
                var _result = await graphClient.Users
                                .Request()
                                .AddAsync(new User
                                {
                                    GivenName = b2cUser.GivenName,
                                    Surname = b2cUser.Surname,
                                    DisplayName = b2cUser.GivenName + " " + b2cUser.Surname,
                                    Identities = new List<ObjectIdentity>
                                    {
                                        new ObjectIdentity()
                                        {
                                            SignInType = "emailAddress",
                                            Issuer = b2cTenantSettings.Tenant,
                                            IssuerAssignedId = b2cUser.Identities[0]
                                        }
                                    },
                                    PasswordProfile = new PasswordProfile() { Password = CreatePassword(5, 5, 5) },
                                    PasswordPolicies = "DisablePasswordExpiration",
                                });
            }
            catch (Exception ex) { }
        }
        public async Task<IGraphServiceUsersCollectionPage> GetB2CUser(string mail)
        {
            try
            {
                var _result = await this.graphClient.Users
                                    .Request()
                                    .Filter($"identities/any(c:c/issuerAssignedId eq '{mail}' and c/issuer eq '{b2cTenantSettings.Tenant}')")
                                    .Select(e => new
                                    {
                                        e.DisplayName,
                                        e.Id,
                                        e.Identities
                                    })
                                    .GetAsync();

                if (_result != null) { return _result; }

                return null;
            }
            catch (Exception ex) { return null; }
        }
        public List<B2CUserModel> ListB2CUsers()
        {
            try
            {
                var _lstB2CUsers = new List<B2CUserModel>();

                var _result = this.graphClient.Users
                                    .Request()
                                    .Select(e => new
                                    {
                                        e.DisplayName,
                                        e.GivenName,
                                        e.Surname,
                                        e.Id,
                                        e.Identities
                                    })
                                    .GetAsync();

                _lstB2CUsers = ConvertToList(_result.Result.CurrentPage);

                if (_result.Result.CurrentPage != null) { return _lstB2CUsers; }

                return null;
            }
            catch (Exception ex) { return null; }
        }
        public async Task<bool> RemoveB2CUser(string mail)
        {
            var _user = await this.GetB2CUser(mail);
            var _userId = _user.CurrentPage[0].Id;

            try
            {
                // Delete user by object ID
                await graphClient.Users[_userId]
                   .Request()
                   .DeleteAsync();

                return true;
            }
            catch (Exception ex) { return false; }
        }
        #endregion

        #region Private
        public static string CreatePassword(int lowercase, int uppercase, int numerics)
        {
            string _password = "";
            string _lowers = "abcdefghijklmnopqrstuvwxyz";
            string _uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string _number = "0123456789";
            Random _random = new Random();


            for (int i = 1; i <= lowercase; i++)
            {
                int temp = _lowers.Length - 1;

                _password = _password.Insert(_random.Next(_password.Length), _lowers[_random.Next(temp)].ToString());
            }
            for (int i = 1; i <= uppercase; i++)
            {
                int temp = _uppers.Length - 1;

                _password = _password.Insert(_random.Next(_password.Length), _uppers[_random.Next(temp)].ToString());
            }

            for (int i = 1; i <= numerics; i++)
            {
                int temp = _number.Length - 1;

                _password = _password.Insert(_random.Next(_password.Length), _number[_random.Next(temp)].ToString());
            }

            return _password;
        }
        public List<B2CUserModel> ConvertToList(IList<User> users)
        {
            List<B2CUserModel> _return = new List<B2CUserModel>();

            foreach (User _current in users)
            {
                B2CUserModel _b2cUser = new B2CUserModel();

                _b2cUser.DisplayName = _current.DisplayName;
                _b2cUser.GivenName = _current.GivenName;
                _b2cUser.Surname = _current.Surname;
                _b2cUser.Identities = new List<string>();

                foreach (ObjectIdentity _actual in _current.Identities)
                {
                    if (_actual.IssuerAssignedId != null)
                    { _b2cUser.Identities.Add(_actual.IssuerAssignedId); }
                }

                _return.Add(_b2cUser);

                _b2cUser = null;
            }

            return _return;
        }
        #endregion
    }
}
