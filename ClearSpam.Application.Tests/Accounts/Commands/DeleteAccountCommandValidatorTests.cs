using ClearSpam.Application.Accounts.Commands;
using ClearSpam.TestsCommon;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClearSpam.Application.Tests.Accounts.Commands
{
    [TestClass]
    public class DeleteAccountCommandValidatorTests : TestBase
    {
        private static DeleteAccountCommandValidator _validator;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _validator = new DeleteAccountCommandValidator();
        }

        [TestMethod]
        public void Validate_HappyPath_ReturnsTrue()
        {
            var command = new DeleteAccountCommand
            {
                Id = 1
            };

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
        }

        [TestMethod]
        public void Validate_IdIsNotSet_ReturnsFalse()
        {
            var command = new DeleteAccountCommand();

            _validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }

        [TestMethod]
        public void Validate_IdIsInvalid_ReturnsFalse()
        {
            var command = new DeleteAccountCommand
            {
                Id = -1
            };

            _validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }
    }
}