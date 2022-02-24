using Microsoft.AspNetCore.Mvc;
using NETCoreProvisionB2CUsersAPI.Interfaces;
using NETCoreProvisionB2CUsersAPI.Models;

namespace NETCoreProvisionB2CUsersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class B2CUserController : ControllerBase
    {
        #region Vars
        private IB2CUser B2CUser { get; set; }
        #endregion

        #region Public
        public B2CUserController(IB2CUser b2cUser) { this.B2CUser = b2cUser; }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] string mail)
        {
            try
            {
                var _b2cUser = await this.B2CUser.GetB2CUser(mail);

                if (_b2cUser == null) { return this.NotFound(mail + " is not registered"); }

                return this.Ok(_b2cUser.CurrentPage[0].DisplayName);
            }
            catch (Exception ex)
            { return this.BadRequest("Unable to get the user - " + ex.Message); }
        }

        [Route("List")]
        [HttpGet]
        public ActionResult<IEnumerable<B2CUserModel>> List()
        {
            try
            { return this.Ok(B2CUser.ListB2CUsers()); }
            catch (Exception ex)
            { return this.BadRequest("Unable to get all users - " + ex.Message); }
        }

        [HttpPost]
        public async Task<ActionResult<B2CUserModel>> AddUsers([FromBody] B2CUserModel b2cUser)
        {
            try
            {
                await this.B2CUser.CreateB2CUser(b2cUser);

                return this.Ok("The user was created successfully!");
            }
            catch (Exception ex)
            { return this.BadRequest("Unable to create the user - " + ex.Message); }
        }

        [HttpDelete("{mail}")]
        public async Task<ActionResult> Remove(string mail)
        {
            try
            {
                var _result = await this.B2CUser.RemoveB2CUser(mail);

                if (_result) { return this.Ok("The user was removed successfully!"); }

                return this.BadRequest("Unable to remove the user from the B2C tenant.");
            }
            catch (Exception ex)
            { return this.BadRequest("Unable to remove the user - " + ex.Message); }
        }
        #endregion

        #region Private
        #endregion
    }
}
