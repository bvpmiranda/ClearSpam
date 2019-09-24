using ClearSpam.Application.Models;
using ClearSpam.Domain.Configurations;
using ClearSpam.Domain.Entities;

namespace ClearSpam.TestsCommon
{
    public partial class TestBase
    {
        protected static AccountDto CreateAccountDto(string server = null, short port = 0, bool ssl = true, string login = null, string password = null, string WatchedMailbox = null, string trashMailBox = null)
        {
            return CreateAccountDto(id: 1, server, port, ssl, login, password, WatchedMailbox, trashMailBox);
        }

        protected static AccountDto CreateAccountDto(int id, string server = null, short port = 0, bool ssl = true, string login = null, string password = null, string WatchedMailbox = null, string trashMailbox = null)
        {
            if (server == null)
                server = NewGuid(AccountConfigurations.ServerMaxLength);
            if (login == null)
                login = NewGuid(AccountConfigurations.LoginMaxLength);
            if (password == null)
                password = NewGuid(AccountConfigurations.PasswordMaxLength);
            if (WatchedMailbox == null)
                WatchedMailbox = NewGuid(AccountConfigurations.WatchedMailboxMaxLength);
            if (trashMailbox == null)
                trashMailbox = NewGuid(AccountConfigurations.TrashMailboxMaxLength);

            var account = new AccountDto
            {
                Id = id,
                Server = server,
                Port = port,
                Ssl = ssl,
                Login = login,
                Password = password,
                WatchedMailbox = WatchedMailbox,
                TrashMailbox = trashMailbox
            };

            return account;
        }

        protected static AccountDto CreateAccountDto(Account account)
        {
            return new AccountDto
            {
                Id = account.Id,
                Server = account.Server,
                Port = account.Port,
                Ssl = account.Ssl,
                Login = account.Login,
                Password = account.Password,
                WatchedMailbox = account.WatchedMailbox,
                TrashMailbox = account.TrashMailbox
            };
        }

        protected static FieldDto CreateFieldDto(string name = null)
        {
            return CreateFieldDto(id: 1, name);
        }

        protected static FieldDto CreateFieldDto(int id, string name = null)
        {
            if (name == null)
                name = NewGuid(FieldConfigurations.NameMaxLength);

            var field = new FieldDto
            {
                Id = id,
                Name = name
            };

            return field;
        }

        protected static FieldDto CreateFieldDto(Field field)
        {
            return new FieldDto
            {
                Id = field.Id,
                Name = field.Name
            };
        }

        protected static RuleDto CreateRuleDto(AccountDto accountDto = null, string field = null, string content = null)
        {
            return CreateRuleDto(id: 1, accountDto, field, content);
        }

        protected static RuleDto CreateRuleDto(int id, AccountDto accountDto = null, string field = null, string content = null)
        {
            if (accountDto == null)
                accountDto = CreateAccountDto();
            if (field == null)
                field = NewGuid(RuleConfigurations.FieldMaxLength);
            if (content == null)
                content = NewGuid(RuleConfigurations.ContentMaxLength);

            var rule = new RuleDto
            {
                Id = id,
                Account = accountDto,
                Field = field,
                Content = content
            };

            return rule;
        }

        protected static RuleDto CreateRuleDto(Rule rule)
        {
            return new RuleDto
            {
                Id = rule.Id,
                Account = CreateAccountDto(rule.Account),
                Field = rule.Field,
                Content = rule.Content
            };
        }
    }
}
