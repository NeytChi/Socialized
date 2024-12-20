using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebAPI.Responses;
using UseCases.Admins;
using UseCases.Admins.Commands;
using WebAPI.Middleware;

namespace WebAPI.Controllers
{
    public class AdminsController : ControllerResponseBase
    {
        private IAdminManager AdminManager;
        private IJwtTokenManager JwtTokenManager;

        public AdminsController(IAdminManager adminManager, IJwtTokenManager jwtTokenManager)
        {
            AdminManager = adminManager;
        }
        [HttpPost]
        [Authorize]
        public ActionResult<DataResponse> Create(CreateAdminCommand command)
        {
            var admin = AdminManager.Create(command);

            return new DataResponse(true, admin);
        }
        [HttpPost]
        [ActionName("Authentication")]
        public ActionResult<DataResponse> Authentication(AuthenticationCommand command)
        {
            var result = AdminManager.Authentication(command);

            var token = JwtTokenManager.Authenticate(result);

            return new DataResponse(true, new { AdminToken = token });
        }
        [HttpPost]
        [ActionName("SetupPassword")]
        public ActionResult<dynamic> SetupPassword(SetupPasswordCommand command)
        {
            AdminManager.SetupPassword(command);

            return new SuccessResponse(true);
        }
        [HttpDelete]
        [Authorize]
        public ActionResult<dynamic> Delete(DeleteAdminCommand command)
        {
            AdminManager.Delete(command);

            return new SuccessResponse(true);
        }
        [HttpPost]
        [ActionName("RecoveryPassword")]
        public ActionResult<dynamic> RecoveryPassword(string adminEmail)
        {
            AdminManager.CreateCodeForRecoveryPassword(adminEmail);

            return new SuccessResponse(true);
        }
        [HttpPost]
        [ActionName("ChangePassword")]
        public ActionResult<dynamic> ChangePassword(ChangePasswordCommand command)
        {
            AdminManager.ChangePassword(command);

            return new SuccessResponse(true);
        }
        [HttpGet]
        [Authorize]
        [ActionName("Admins")]
        public ActionResult<dynamic> GetAdmins([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            long adminId = GetAdminIdByToken();

            var result = AdminManager.GetAdmins(adminId, since, count);

            return new DataResponse(true, result);
        }
        [HttpGet]
        [Authorize]
        [ActionName("Users")]
        public ActionResult<dynamic> GetUsers([FromQuery] int since = 0, [FromQuery] int count = 10)
        {
            var result = AdminManager.GetUsers(since, count);

            return new DataResponse(true, result);
        }
    }
}