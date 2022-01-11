// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using UseCase.Shared;
using UseCase.Users.CompleteRegistration;

namespace WebClient.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly UserCompleteRegistrationUseCase _userCompleteRegistrationUseCase;

        public ConfirmEmailModel(
            UserManager<IdentityUser> userManager,
            UserCompleteRegistrationUseCase userCompleteRegistrationUseCase)
        {
            _userManager = userManager;
            _userCompleteRegistrationUseCase = userCompleteRegistrationUseCase;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            if (result.Succeeded)
            {
                var command = new UserCompleteRegistrationCommand(
                    userSession: new UserSession(userId));
                await _userCompleteRegistrationUseCase.ExecuteAsync(command);
            }

            StatusMessage = result.Succeeded ? "メール確認できました。" : "メール確認できませんでした。";
            return Page();
        }
    }
}
