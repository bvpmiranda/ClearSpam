using ClearSpam.Application.Rules.Commands;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace ClearSpam.Application.Tests.Rules.Commands
{
    [TestClass]
    public class CreateRuleCommandHandlerTests : TestBase
    {
        private static CreateRuleCommandHandler _createRuleCommandHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _createRuleCommandHandler = new CreateRuleCommandHandler(RepositoryMock.Object, MapperMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_AddsRule()
        {
            var Rule = CreateRule();

            var command = new CreateRuleCommand();

            MapperMock.Setup(x => x.Map<Rule>(command)).Returns(Rule);

            var result = _createRuleCommandHandler.Handle(command, new CancellationToken()).Result;

            RepositoryMock.Verify(x => x.Add<Rule>(Rule), Times.Once);
            RepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}