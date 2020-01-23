using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ClearSpam.TestsCommon
{
    public partial class TestBase
    {
        protected static void AssertAggregateException(Action action, Type expectedException)
        {
            try
            {
                action.Invoke();

                throw new AssertFailedException($"Exception of type '{expectedException.Name}' not thrown");
            }
            catch (Exception ex)
            {
                if (ex is AggregateException aggregateException &&
                    aggregateException.InnerException.GetType() != expectedException)
                    throw new
                        AssertFailedException($"Exception of type '{expectedException.Name}' not thrown. '{aggregateException.InnerException.GetType().Name}' thrown instead.'");
            }
        }

        protected void AssertAccount(Account expected, AccountDto actual, bool assertId = true)
        {
            if (assertId)
                Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Server, actual.Server);
            Assert.AreEqual(expected.Port, actual.Port);
            Assert.AreEqual(expected.Ssl, actual.Ssl);
            Assert.AreEqual(expected.Login, actual.Login);
            Assert.AreEqual(expected.Password, actual.Password);
            Assert.AreEqual(expected.WatchedMailbox, actual.WatchedMailbox);
        }

        protected void AssertRule(Rule expected, RuleDto actual, bool assertId = true)
        {
            if (assertId)
                Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Field, actual.Field);
            Assert.AreEqual(expected.Content, actual.Content);

            AssertAccount(expected.Account, actual.Account);
        }

        protected void AssertField(Field expected, FieldDto actual, bool assertId = true)
        {
            if (assertId)
                Assert.AreEqual(expected.Id, actual.Id);
            Assert.AreEqual(expected.Name, actual.Name);
        }

        protected static void VerifyExceptionMessage(Action action, string expectedMessage)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.Message.Contains(expectedMessage, StringComparison.OrdinalIgnoreCase), $"Exception does not contain \"{expectedMessage}\"");

                throw;
            }
        }
    }
}
