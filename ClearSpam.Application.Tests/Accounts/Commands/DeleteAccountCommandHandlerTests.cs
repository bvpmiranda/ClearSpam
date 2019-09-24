using ClearSpam.Application.Accounts.Commands;
using ClearSpam.Application.Exceptions;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace ClearSpam.Application.Tests.Accounts.Commands
{
    [TestClass]
    public class DeleteAccountCommandHandlerTests : TestBase
    {
        private static DeleteAccountCommandHandler _deleteAccountCommandHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _deleteAccountCommandHandler = new DeleteAccountCommandHandler(RepositoryMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_AddsAccount()
        {
            const int id = 1;
            var Account = CreateAccount();

            RepositoryMock.Setup(x => x.Get<Account>(id))
                          .Returns(Account);

            var command = new DeleteAccountCommand { Id = id };
            var result = _deleteAccountCommandHandler.Handle(command, new CancellationToken()).Result;

            RepositoryMock.Verify(x => x.Remove<Account>(Account), Times.Once);
            RepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Handle_AccountDoesNotExists_ThrowsException()
        {
            const int id = 999;

            RepositoryMock.Setup(x => x.Get<Account>(id))
                          .Returns((Account)null);

            var command = new DeleteAccountCommand { Id = id };

            AssertAggregateException(() => {
                var result =
                    _deleteAccountCommandHandler
                        .Handle(command, new CancellationToken())
                        .Result;
            },
                                     typeof(NotFoundException));
        }
    }
}