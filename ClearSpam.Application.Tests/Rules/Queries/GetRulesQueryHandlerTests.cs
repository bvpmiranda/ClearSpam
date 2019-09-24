using ClearSpam.Application.Models;
using ClearSpam.Application.Rules.Queries;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace ClearSpam.Application.Tests.Rules.Queries
{
    [TestClass]
    public class GetRulesQueryHandlerTests : TestBase
    {
        private static GetRulesQueryHandler _getRulesQueryHandler;


        private static readonly Rule Rule1 = CreateRule(id: 1);
        private static readonly RuleDto RuleDto1 = CreateRuleDto(Rule1);
        private static readonly Rule Rule2 = CreateRule(id: 2);
        private static readonly RuleDto RuleDto2 = CreateRuleDto(Rule2);

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            MapperMock.Setup(x => x.Map<RuleDto>(Rule1)).Returns(RuleDto1);
            MapperMock.Setup(x => x.Map<RuleDto>(Rule2)).Returns(RuleDto2);

            _getRulesQueryHandler = new GetRulesQueryHandler(RepositoryMock.Object, MapperMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_ReturnsRules()
        {
            RepositoryMock.Setup(x => x.Get<Rule>(It.IsAny<Expression<Func<Rule, bool>>>())).Returns(new Rule[]
            {
                Rule1,
                Rule2
            });

            var request = new GetRulesQuery(accountId: 1);
            var result = _getRulesQueryHandler.Handle(request, new CancellationToken()).Result.ToList();

            var RuleDto1 = result.Single(x => x.Id == Rule1.Id);
            AssertRule(Rule1, RuleDto1);

            var RuleDto2 = result.Single(x => x.Id == Rule2.Id);
            AssertRule(Rule2, RuleDto2);
        }
    }
}
