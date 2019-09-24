using ClearSpam.Application.Accounts.Commands;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace ClearSpam.Application.Tests.Accounts.Commands
{
    [TestClass]
    public class CreateAccountCommandHandlerTests : TestBase
    {
        private static CreateAccountCommandHandler _createAccountCommandHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _createAccountCommandHandler = new CreateAccountCommandHandler(RepositoryMock.Object, MapperMock.Object, CryptographyMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_AddsAccount()
        {
            var Account = CreateAccount();

            var command = new CreateAccountCommand();

            MapperMock.Setup(x => x.Map<Account>(command)).Returns(Account);

            var result = _createAccountCommandHandler.Handle(command, new CancellationToken()).Result;

            RepositoryMock.Verify(x => x.Add<Account>(Account), Times.Once);
            RepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}