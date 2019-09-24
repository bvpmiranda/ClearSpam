using ClearSpam.Application.Rules.Commands;
using ClearSpam.TestsCommon;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClearSpam.Application.Tests.Rules.Commands
{
    [TestClass]
    public class DeleteRuleCommandValidatorTests : TestBase
    {
        private static DeleteRuleCommandValidator _validator;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _validator = new DeleteRuleCommandValidator();
        }

        [TestMethod]
        public void Validate_HappyPath_ReturnsTrue()
        {
            var command = new DeleteRuleCommand(id: 1);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
        }

        [DataRow(-1)]
        [DataRow(0)]
        [TestMethod]
        public void Validate_IdIsInvalid_ReturnsFalse(int id)
        {
            var command = new DeleteRuleCommand(id);

            _validator.ShouldHaveValidationErrorFor(x => x.Id, command);
        }
    }
}