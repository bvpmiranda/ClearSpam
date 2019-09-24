using AutoMapper;
using ClearSpam.Application.Interfaces;
using ClearSpam.Application.Models.Mappings;
using ClearSpam.Domain.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Text;

namespace ClearSpam.TestsCommon
{
    public partial class TestBase
    {
        protected static Mock<IRepository> RepositoryMock = new Mock<IRepository>();
        protected static Mock<IMapper> MapperMock = new Mock<IMapper>();
        protected static Mock<ICryptography> CryptographyMock = new Mock<ICryptography>();
        protected static Mock<IImapService> ImapServiceMock = new Mock<IImapService>();

        protected static IRepository Repository;
        protected static IMapper Mapper;


        [TestInitialize]
        public void TestInitialize()
        {
            RepositoryMock.Invocations.Clear();
            MapperMock.Invocations.Clear();
            CryptographyMock.Invocations.Clear();
            ImapServiceMock.Invocations.Clear();
        }

        protected static void InitializeAutoMapper()
        {
            var profiles = typeof(AccountMappings).Assembly
                                             .GetTypes()
                                             .Where(t => t.BaseType == typeof(Profile))
                                             .ToList();
            Mapper = new Mapper(new MapperConfiguration(x => profiles.ForEach(x.AddProfile)));
        }

        protected static string NewGuid(int length = 36)
        {
            const int guidLength = 36;
            var count = 1;

            if (length > guidLength)
            {
                count = (int)Math.Ceiling(length / (decimal)guidLength);
                length -= (count - 1) * guidLength;
            }

            if (count == 1)
                return Guid.NewGuid().ToString().Substring(0, length);

            var sb = new StringBuilder();
            for (var i = 0; i < count - 1; i++)
                sb.Append(Guid.NewGuid().ToString());
            sb.Append(Guid.NewGuid().ToString().Substring(0, length));
            return sb.ToString();
        }

        //private static void InitializeSgcContext()
        //{
        //    var connection = new SqliteConnection("DataSource=:memory:");
        //    connection.Open();

        //    var optionsSgc = new DbContextOptionsBuilder<SgcContext>()
        //                     .UseSqlite(connection)
        //                     .Options;
        //    SgcContext = new SgcContext(optionsSgc);
        //    SgcContext.Database.EnsureCreated();
        //}
    }
}
