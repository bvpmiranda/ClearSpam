using ClearSpam.Entities;
using ClearSpam.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ClearSpam.Pages.Accounts
{
    [Authorize]
    public class EditAccount : PageModel
    {
        private readonly ClearSpamContext _context;
        private ICryptography _cryptography;
        private readonly IImapService _imapService;
        private readonly ILogger<EditAccount> _logger;

        public EditAccount(ClearSpamContext context, ICryptography cryptography, IImapService imapService, ILogger<EditAccount> logger)
        {
            _context = context ?? throw new System.ArgumentNullException(nameof(context));
            _cryptography = cryptography ?? throw new System.ArgumentNullException(nameof(cryptography));
            _imapService = imapService ?? throw new System.ArgumentNullException(nameof(imapService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [BindProperty]
        public Account Account { get; set; }
        public IEnumerable<string> Mailboxes { get; private set; }

        public IActionResult OnGet(int? id)
        {
            Account = _context.Accounts.Where(x => x.Id == id).FirstOrDefault();

            if (Account == null)
                return NotFound();

            Account.OriginalPassword = Account.Password;

            try
            {
                _imapService.Account = Account;
                Mailboxes = _imapService.GetMailboxesList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There was an error getting the list of mailboxes");

                Mailboxes = null;
            }

            return Page();
        }

        public IActionResult OnPostAsync()
        {
            if (Account.Password != Account.OriginalPassword)
                Account.Password = _cryptography.Encrypt(Account.Password);

            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
