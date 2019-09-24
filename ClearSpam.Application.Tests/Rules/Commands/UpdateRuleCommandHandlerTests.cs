using ClearSpam.Application.Rules.Commands;
using ClearSpam.Application.Exceptions;
using ClearSpam.Application.Interfaces;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading;

namespace ClearSpam.Application.Tests.Rules.Commands
{
    [TestClass]
    public class UpdateRuleCommandHandlerTests : TestBase
    {
        private static UpdateRuleCommandHandler _updateRuleCommandHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _updateRuleCommandHandler = new UpdateRuleCommandHandler(RepositoryMock.Object, MapperMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_UpdatesRule()
        {
            var rule = CreateRule();

            RepositoryMock.Setup(x => x.Get<Rule>(rule.Id))
                          .Returns(rule);

            var command = new UpdateRuleCommand { Id = rule.Id };

            MapperMock.Setup(x => x.Map<Rule>(command)).Returns(rule);

            var result = _updateRuleCommandHandler.Handle(command, new CancellationToken()).Result;

            MapperMock.Verify(x => x.Map<IEntityDto, Rule>(command, rule), Times.Once);

            RepositoryMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Handle_RuleDoesNotExists_ThrowsException()
        {
            const int id = 999;

            RepositoryMock.Setup(x => x.Get<Rule>(id))
                          .Returns((Rule)null);

            var command = new UpdateRuleCommand { Id = id };

            AssertAggregateException(() => {
                var result =
                    _updateRuleCommandHandler
                        .Handle(command, new CancellationToken())
                        .Result;
            },
                                     typeof(NotFoundException));
        }
    }
}