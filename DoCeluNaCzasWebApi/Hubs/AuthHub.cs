using DoCeluNaCzasWebApi.Controllers;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace DoCeluNaCzasWebApi.Hubs
{
    public class AuthHub : Hub
    {
        readonly AccountController _accountController = new AccountController();

        public async Task<bool> CheckIfEmailExist(string email)
        {
            return await _accountController.CheckIfEmailExist(email);
        }
    }
}