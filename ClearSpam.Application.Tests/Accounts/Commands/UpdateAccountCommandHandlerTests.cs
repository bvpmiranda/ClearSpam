using ClearSpam.Application.Accounts.Commands;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Interfaces;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace ClearSpam.Application.Tests.Accounts.Commands
{
    [TestClass]
    public class UpdateAccountCommandHandlerTests : TestBase
    {
        private static UpdateAccountCommandHandler _updateAccountCommandHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _updateAccountCommandHandler = new UpdateAccountCommandHandler(RepositoryMock.Object, MapperMock.Object, CryptographyMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_UpdatesAccount()
        {
            var account = CreateAccount();

            RepositoryMock.Setup(x => x.Get<Account>(account.Id))
                          .Returns(account);

            var command = new UpdateAccountCommand { Id = account.Id };

            MapperMock.Setup(x => x.Map<Account>(command)).Returns(account);

            var result = _updateAccountCommandHandler.Handle(command, new CancellationToken()).Result;

            MapperMock.Verify(x => x.Map<IEntityDto, Account>(command, account), Times.Once);

            RepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Handle_AccountDoesNotExists_ThrowsException()
        {
            const int id = 999;

            RepositoryMock.Setup(x => x.Get<Account>(id))
                          .Returns((Account)null);

            var command = new UpdateAccountCommand { Id = id };

            AssertAggregateException(() => {
                var result =
                    _updateAccountCommandHandler
                        .Handle(command, new CancellationToken())
                        .Result;
            },
                                     typeof(NotFoundException));
        }
    }
}