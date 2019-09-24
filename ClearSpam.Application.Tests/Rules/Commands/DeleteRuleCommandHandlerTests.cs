using ClearSpam.Application.Rules.Commands;
using ClearSpam.Application.Exceptions;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace ClearSpam.Application.Tests.Rules.Commands
{
    [TestClass]
    public class DeleteRuleCommandHandlerTests : TestBase
    {
        private static DeleteRuleCommandHandler _deleteRuleCommandHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _deleteRuleCommandHandler = new DeleteRuleCommandHandler(RepositoryMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_AddsRule()
        {
            const int id = 1;
            var Rule = CreateRule();

            RepositoryMock.Setup(x => x.Get<Rule>(id))
                          .Returns(Rule);

            var command = new DeleteRuleCommand(id);
            var result = _deleteRuleCommandHandler.Handle(command, new CancellationToken()).Result;

            RepositoryMock.Verify(x => x.Remove<Rule>(Rule), Times.Once);
            RepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Handle_RuleDoesNotExists_ThrowsException()
        {
            const int id = 999;

            RepositoryMock.Setup(x => x.Get<Rule>(id))
                          .Returns((Rule)null);

            var command = new DeleteRuleCommand(id);

            AssertAggregateException(() => {
                var result =
                    _deleteRuleCommandHandler
                        .Handle(command, new CancellationToken())
                        .Result;
            },
                                     typeof(NotFoundException));
        }
    }
}