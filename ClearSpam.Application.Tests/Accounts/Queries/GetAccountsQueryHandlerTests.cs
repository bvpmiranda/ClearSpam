using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace ClearSpam.Application.Tests.Accounts.Queries
{
    [TestClass]
    public class GetAccountsQueryHandlerTests : TestBase
    {
        private static GetAccountsQueryHandler _getAccountsQueryHandler;

        private static string UserId = "abcde";
        private static readonly Account Account1 = CreateAccount(id: 1, userId: UserId);
        private static readonly AccountDto AccountDto1 = CreateAccountDto(Account1);
        private static readonly Account Account2 = CreateAccount(id: 2, userId: UserId);
        private static readonly AccountDto AccountDto2 = CreateAccountDto(Account2);

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            MapperMock.Setup(x => x.Map<AccountDto>(Account1)).Returns(AccountDto1);
            MapperMock.Setup(x => x.Map<AccountDto>(Account2)).Returns(AccountDto2);

            _getAccountsQueryHandler = new GetAccountsQueryHandler(RepositoryMock.Object, MapperMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_ReturnsAccounts()
        {
            RepositoryMock.Setup(x => x.Get<Account>(It.IsAny<Expression<Func<Account, bool>>>())).Returns(new Account[]
            {
                Account1,
                Account2
            });

            var request = new GetAccountsQuery(UserId);
            var result = _getAccountsQueryHandler.Handle(request, new CancellationToken()).Result.ToList();

            var AccountDto1 = result.Single(x => x.Id == Account1.Id);
            AssertAccount(Account1, AccountDto1);

            var AccountDto2 = result.Single(x => x.Id == Account2.Id);
            AssertAccount(Account2, AccountDto2);
        }
    }
}
