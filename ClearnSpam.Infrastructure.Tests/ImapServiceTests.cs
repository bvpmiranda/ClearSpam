//using ClearSpam.Domain.Entities;
//using ClearSpam.Infrastructure;
//using ClearSpam.TestsCommon;
//using ImapX;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using System;
//using System.Linq;

//namespace ClearnSpam.Infrastructure.Tests
//{
//    [TestClass]
//    public class ImapServiceTests : TestBase
//    {
//        private static Mock<ImapClient> _imapClientMock;

//        [ClassInitialize]
//        public static void ClassInitialize(TestContext context)
//        {
//            _imapClientMock = new Mock<ImapClient>();
//        }

//        [TestCleanup]
//        public void TestCleanup()
//        {
//            _imapClientMock.Invocations.Clear();
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void Constructor_NullCryptography_ThrowsException()
//        {
//            VerifyExceptionMessage(() => _ = new ImapService(null, null), "cryptography");
//        }

//        [TestMethod]
//        [ExpectedException(typeof(ArgumentNullException))]
//        public void Constructor_NullLogger_ThrowsException()
//        {
//            VerifyExceptionMessage(() => _ = new ImapService(CryptographyMock.Object, null), "logger");
//        }

//        [TestMethod]
//        public void GetMailboxesList_HappyPath_CallsService()
//        {
//            var account = new Account();
//            var service = new ImapService(CryptographyMock.Object, LoggerMock.Object);
//            service.ImapClient = _imapClientMock.Object;

//            _imapClientMock.Setup(x => x.Folders).Returns(new ImapX.Collections.CommonFolderCollection(_imapClientMock.Object));

//            service.GetMailboxesList();

//            _imapClientMock.Verify(x => x.Folders, Times.Once);
//        }

//        [TestMethod]
//        public void GetMailboxesList_HappyPath_ReturnsCorrectInformation()
//        {
//            //var mailbox1 = NewGuid();
//            //var mailbox2 = NewGuid();

//            //var account = new Account();
//            //var service = new ImapService(CryptographyMock.Object, LoggerMock.Object);
//            //service.ImapClient = _imapClientMock.Object;

//            //_imapClientMock.Setup(x => x.Folders).Returns(new string[] {
//            //    mailbox1,
//            //    mailbox2
//            //});

//            //var result = service.GetMailboxesList().ToList();

//            //Assert.AreEqual(2, result.Count);
//            //Assert.IsTrue(result.Contains(mailbox1));
//            //Assert.IsTrue(result.Contains(mailbox2));
//        }
//    }
//}
