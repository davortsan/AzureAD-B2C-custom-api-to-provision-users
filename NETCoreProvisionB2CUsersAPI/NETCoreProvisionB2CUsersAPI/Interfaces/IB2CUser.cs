using Microsoft.Graph;
using NETCoreProvisionB2CUsersAPI.Models;

namespace NETCoreProvisionB2CUsersAPI.Interfaces
{
    public interface IB2CUser
    {
        Task CreateB2CUser(B2CUserModel b2cUser);
        Task<IGraphServiceUsersCollectionPage> GetB2CUser(string mail);
        Task<bool> RemoveB2CUser(string mail);
        List<B2CUserModel> ListB2CUsers();
    }
}
