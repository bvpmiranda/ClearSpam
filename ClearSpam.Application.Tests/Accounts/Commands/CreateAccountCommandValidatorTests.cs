using ClearSpam.Application.Accounts.Commands;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Configurations;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq.Expressions;

namespace ClearSpam.Application.Tests.Accounts.Commands
{
    [TestClass]
    public class CreateAccountCommandValidatorTests : TestBase
    {
        private static CreateAccountCommandValidator _validator;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _validator = new CreateAccountCommandValidator(MapperMock.Object);
        }

        [TestMethod]
        public void Validate_HappyPath_ReturnsTrue()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_ServerIsNull_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Server = null;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Server, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_ServerIsEmpty_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Server = "";

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Server, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_ServerExceedsLimit_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Server = NewGuid(AccountConfigurations.ServerMaxLength + 1);

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Server, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_PortIsNegative_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Port = -1;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Port, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_PortIsZero_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Port = 0;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Port, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_PortIsGreaterThan65535_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Port = 65536;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Port, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_LoginIsNull_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Login = null;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Login, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_LoginIsEmpty_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Login = "";

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Login, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_LoginExceedsLimit_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Login = NewGuid(AccountConfigurations.LoginMaxLength + 1);

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Login, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_PasswordIsNull_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Password = null;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Password, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_PasswordIsEmpty_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Password = "";

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Password, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }

        [TestMethod]
        public void Validate_PasswordExceedsLimit_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateCreateAccountCommand();
            command.Password = NewGuid(AccountConfigurations.PasswordMaxLength + 1);

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Password, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.TrashMailbox, command);
        }


        private CreateAccountCommand CreateCreateAccountCommand(int id = 1, string server = null, short port = 993, bool ssl = true, string login = null, string password = null, string WatchedMailbox = null, string trashMailbox = null)
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

            var account = new CreateAccountCommand
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
    }
}