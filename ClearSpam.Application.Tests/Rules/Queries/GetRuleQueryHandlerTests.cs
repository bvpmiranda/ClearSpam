using ClearSpam.Application.Rules.Queries;
using ClearSpam.Application.Exceptions;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading;

namespace ClearSpam.Application.Tests.Rules.Queries
{
    [TestClass]
    public class GetRuleQueryHandlerTests : TestBase
    {
        private static GetRuleQueryHandler _getRuleQueryHandler;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            _getRuleQueryHandler = new GetRuleQueryHandler(RepositoryMock.Object, MapperMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_ReturnsRule()
        {
            const int id = 1;

            RepositoryMock.Setup(x => x.Get<Rule>(id)).Returns(new Rule());

            var request = new GetRuleQuery(id);
            var result = _getRuleQueryHandler.Handle(request, new CancellationToken()).Result;

            RepositoryMock.Verify(x => x.Get<Rule>(id), Times.Once);
        }

        [TestMethod]
        public void Handle_InvalidId_ThrowsArgumentOutOfRangeException()
        {
            var request = new GetRuleQuery(0);

            AssertAggregateException(() => {
                var result = _getRuleQueryHandler
                             .Handle(request, new CancellationToken())
                             .Result;
            },
                                     typeof(ArgumentOutOfRangeException));
        }

        [TestMethod]
        public void Handle_RuleDoesNotExist_ThrowsNotFoundException()
        {
            const int id = 999;
            RepositoryMock.Setup(x => x.Get<Rule>(id))
                          .Throws(new NotFoundException(nameof(Rule), id));

            var request = new GetRuleQuery(999);

            AssertAggregateException(() => {
                var result = _getRuleQueryHandler
                             .Handle(request, new CancellationToken())
                             .Result;
            }, typeof(NotFoundException));
        }
    }
}
