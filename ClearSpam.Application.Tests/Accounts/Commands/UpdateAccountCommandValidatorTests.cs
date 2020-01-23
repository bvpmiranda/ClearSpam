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
    public class UpdateAccountCommandValidatorTests : TestBase
    {
        private static UpdateAccountCommandValidator _validator;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _validator = new UpdateAccountCommandValidator(MapperMock.Object);
        }

        [TestMethod]
        public void Validate_HappyPath_ReturnsTrue()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_ServerIsNull_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Server = null;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Server, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_ServerIsEmpty_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Server = "";

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Server, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_ServerExceedsLimit_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Server = NewGuid(AccountConfigurations.ServerMaxLength + 1);

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Server, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_PortIsNegative_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Port = -1;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Port, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_PortIsZero_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Port = 0;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Port, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_PortIsGreaterThan65535_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Port = 65536;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Port, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_LoginIsNull_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Login = null;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Login, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_LoginIsEmpty_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Login = "";

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Login, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_LoginExceedsLimit_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Login = NewGuid(AccountConfigurations.LoginMaxLength + 1);

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Login, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_PasswordIsNull_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Password = null;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Password, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_PasswordIsEmpty_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Password = "";

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Password, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_PasswordExceedsLimit_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.Password = NewGuid(AccountConfigurations.PasswordMaxLength + 1);

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Password, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.WatchedMailbox, command);
        }

        [TestMethod]
        public void Validate_WatchedMailboxIsNull_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.WatchedMailbox = null;

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.WatchedMailbox, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
        }

        [TestMethod]
        public void Validate_WatchedMailboxIsEmpty_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.WatchedMailbox = "";

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.WatchedMailbox, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
        }

        [TestMethod]
        public void Validate_WatchedMailboxExceedsLimit_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Account, bool>>>()))
                          .Returns(false);

            var command = CreateUpdateAccountCommand();
            command.WatchedMailbox = NewGuid(AccountConfigurations.WatchedMailboxMaxLength + 1);

            MapperMock.Setup(x => x.Map<AccountDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.WatchedMailbox, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Server, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Port, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Login, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Password, command);
        }

        private UpdateAccountCommand CreateUpdateAccountCommand(int id = 1, string server = null, short port = 993, bool ssl = true, string login = null, string password = null, string WatchedMailbox = null)
        {
            if (server == null)
                server = NewGuid(AccountConfigurations.ServerMaxLength);
            if (login == null)
                login = NewGuid(AccountConfigurations.LoginMaxLength);
            if (password == null)
                password = NewGuid(AccountConfigurations.PasswordMaxLength);
            if (WatchedMailbox == null)
                WatchedMailbox = NewGuid(AccountConfigurations.WatchedMailboxMaxLength);

            var account = new UpdateAccountCommand
            {
                Id = id,
                Server = server,
                Port = port,
                Ssl = ssl,
                Login = login,
                Password = password,
                WatchedMailbox = WatchedMailbox
            };

            return account;
        }

    }
}