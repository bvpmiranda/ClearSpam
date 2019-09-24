using ClearSpam.Application.Rules.Commands;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Configurations;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq.Expressions;

namespace ClearSpam.Application.Tests.Rules.Commands
{
    [TestClass]
    public class CreateRuleCommandValidatorTests : TestBase
    {
        private static CreateRuleCommandValidator _validator;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _validator = new CreateRuleCommandValidator(RepositoryMock.Object, MapperMock.Object);
        }

        [TestMethod]
        public void Validate_HappyPath_ReturnsTrue()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Rule, bool>>>()))
                          .Returns(false);
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Field, bool>>>()))
                          .Returns(true);

            var command = CreateCreateRuleCommand();

            MapperMock.Setup(x => x.Map<RuleDto>(command)).Returns(command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Field, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Content, command);
        }

        [TestMethod]
        public void Validate_FieldIsNull_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Rule, bool>>>()))
                          .Returns(false);

            var command = CreateCreateRuleCommand();
            command.Field = null;

            MapperMock.Setup(x => x.Map<RuleDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Field, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Content, command);
        }

        [TestMethod]
        public void Validate_FieldIsEmpty_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Rule, bool>>>()))
                          .Returns(false);

            var command = CreateCreateRuleCommand();
            command.Field = "";

            MapperMock.Setup(x => x.Map<RuleDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Field, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Content, command);
        }

        [TestMethod]
        public void Validate_FieldExceedsLimit_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Rule, bool>>>()))
                          .Returns(false);

            var command = CreateCreateRuleCommand();
            command.Field = NewGuid(RuleConfigurations.FieldMaxLength + 1);

            MapperMock.Setup(x => x.Map<RuleDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Field, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Content, command);
        }

        [TestMethod]
        public void Validate_FieldDoesNotExist_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Rule, bool>>>()))
                          .Returns(false);
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Field, bool>>>()))
                          .Returns(false);

            var command = CreateCreateRuleCommand();

            MapperMock.Setup(x => x.Map<RuleDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Field, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Content, command);
        }

        [TestMethod]
        public void Validate_ContentIsNull_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Rule, bool>>>()))
                          .Returns(false);
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Field, bool>>>()))
                          .Returns(true);

            var command = CreateCreateRuleCommand();
            command.Content = null;

            MapperMock.Setup(x => x.Map<RuleDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Content, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Field, command);
        }

        [TestMethod]
        public void Validate_ContentIsEmpty_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Rule, bool>>>()))
                          .Returns(false);
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Field, bool>>>()))
                          .Returns(true);

            var command = CreateCreateRuleCommand();
            command.Content = "";

            MapperMock.Setup(x => x.Map<RuleDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Content, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Field, command);
        }

        [TestMethod]
        public void Validate_ContentExceedsLimit_ReturnsFalse()
        {
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Rule, bool>>>()))
                          .Returns(false);
            RepositoryMock.Setup(x => x.Any(It.IsAny<Expression<Func<Field, bool>>>()))
                          .Returns(true);

            var command = CreateCreateRuleCommand();
            command.Content = NewGuid(RuleConfigurations.ContentMaxLength + 1);

            MapperMock.Setup(x => x.Map<RuleDto>(command)).Returns(command);

            _validator.ShouldHaveValidationErrorFor(x => x.Content, command);

            _validator.ShouldNotHaveValidationErrorFor(x => x.Id, command);
            _validator.ShouldNotHaveValidationErrorFor(x => x.Field, command);
        }

        private CreateRuleCommand CreateCreateRuleCommand(int id = 1, AccountDto accountDto = null, string field = null, string content = null)
        {
            if (accountDto == null)
                accountDto = CreateAccountDto();
            if (field == null)
                field = NewGuid(RuleConfigurations.FieldMaxLength);
            if (content == null)
                content = NewGuid(RuleConfigurations.ContentMaxLength);

            var rule = new CreateRuleCommand
            {
                Id = id,
                Account = accountDto,
                Field = field,
                Content = content
            };

            return rule;
        }
    }
}