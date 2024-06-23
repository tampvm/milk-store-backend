using Azure.Core;
using Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using System.Security.Claims;

namespace MilkStore.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Account> _userManager;

        public IndexModel(UserManager<Account> userManager)
        {
            _userManager = userManager;
        }

        public string UserName { get; set; }
        public string AvatarUrl { get; set; }

        public string GoogleToken { get; set; }

        public async Task OnGetAsync()
        {
            UserName = HttpContext.Session.GetString("UserName");
            AvatarUrl = HttpContext.Session.GetString("AvatarUrl");
            GoogleToken = HttpContext.Session.GetString("GoogleToken");
        }

    }
}
