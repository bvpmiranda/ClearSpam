using ClearSpam.Domain.Configurations;
using ClearSpam.Domain.Entities;

namespace ClearSpam.TestsCommon
{
    public partial class TestBase
    {
        protected static Account CreateAccount(string userId = null, string server = null, short port = 0, bool ssl = true, string login = null, string password = null, string WatchedMailbox = null)
        {
            return CreateAccount(id: 1, userId, server, port, ssl, login, password, WatchedMailbox);
        }

        protected static Account CreateAccount(int id, string userId = null, string server = null, short port = 0, bool ssl = true, string login = null, string password = null, string WatchedMailbox = null)
        {
            if (userId == null)
                userId = NewGuid();
            if (server == null)
                server = NewGuid(AccountConfigurations.ServerMaxLength);
            if (login == null)
                login = NewGuid(AccountConfigurations.LoginMaxLength);
            if (password == null)
                password = NewGuid(AccountConfigurations.PasswordMaxLength);
            if (WatchedMailbox == null)
                WatchedMailbox = NewGuid(AccountConfigurations.WatchedMailboxMaxLength);

            var account = new Account
            {
                Id = id,
                UserId = userId,
                Server = server,
                Port = port,
                Ssl = ssl,
                Login = login,
                Password = password,
                WatchedMailbox = WatchedMailbox
            };

            return account;
        }

        protected static Field CreateField(string name = null)
        {
            return CreateField(id: 1, name);
        }

        protected static Field CreateField(int id, string name = null)
        {
            if (name == null)
                name = NewGuid(FieldConfigurations.NameMaxLength);

            var field = new Field
            {
                Id = id,
                Name = name
            };

            return field;
        }

        protected static Rule CreateRule(Account account = null, string field = null, string content = null)
        {
            return CreateRule(id: 1, account, field, content);
        }

        protected static Rule CreateRule(int id, Account account = null, string field = null, string content = null)
        {
            if (account == null)
                account = CreateAccount();
            if (field == null)
                field = NewGuid(RuleConfigurations.FieldMaxLength);
            if (content == null)
                content = NewGuid(RuleConfigurations.ContentMaxLength);

            var rule = new Rule
            {
                Id = id,
                Account = account,
                Field = field,
                Content = content
            };

            return rule;
        }
    }
}
