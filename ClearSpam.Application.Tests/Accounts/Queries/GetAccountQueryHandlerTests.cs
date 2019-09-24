using ClearSpam.Application.Accounts.Queries;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;

namespace ClearSpam.Application.Tests.Accounts.Queries
{
    [TestClass]
    public class GetAccountQueryHandlerTests : TestBase
    {
        private static GetAccountQueryHandler _getAccountQueryHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _getAccountQueryHandler = new GetAccountQueryHandler(RepositoryMock.Object, MapperMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_ReturnsAccount()
        {
            var account = CreateAccount();
            var accountDto = CreateAccountDto(account);

            RepositoryMock.Setup(x => x.Get<Account>(account.Id)).Returns(account);
            MapperMock.Setup(x => x.Map<AccountDto>(account)).Returns(accountDto);

            var request = new GetAccountQuery(account.Id);
            var result = _getAccountQueryHandler.Handle(request, new CancellationToken()).Result;

            RepositoryMock.Verify(x => x.Get<Account>(account.Id), Times.Once);
        }

        [TestMethod]
        public void Handle_InvalidId_ThrowsArgumentOutOfRangeException()
        {
            var request = new GetAccountQuery(0);

            AssertAggregateException(() => {
                var result = _getAccountQueryHandler
                             .Handle(request, new CancellationToken())
                             .Result;
            },
                                     typeof(ArgumentOutOfRangeException));
        }

        [TestMethod]
        public void Handle_AccountDoesNotExist_ThrowsNotFoundException()
        {
            const int id = 999;
            RepositoryMock.Setup(x => x.Get<Account>(id))
                          .Throws(new NotFoundException(nameof(Account), id));

            var request = new GetAccountQuery(999);

            AssertAggregateException(() => {
                var result = _getAccountQueryHandler
                             .Handle(request, new CancellationToken())
                             .Result;
            }, typeof(NotFoundException));
        }
    }
}
