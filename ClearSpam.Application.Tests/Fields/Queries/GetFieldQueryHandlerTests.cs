using ClearSpam.Application.Fields.Queries;
using ClearSpam.Application.Models;
using ClearSpam.Domain.Entities;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;

namespace ClearSpam.Application.Tests.Fields.Queries
{
    [TestClass]
    public class GetFieldsQueryHandlerTests : TestBase
    {
        private static GetFieldsQueryHandler _getFieldsQueryHandler;

        private static readonly Field Field1 = CreateField(id: 1);
        private static readonly FieldDto FieldDto1 = CreateFieldDto(Field1);
        private static readonly Field Field2 = CreateField(id: 2);
        private static readonly FieldDto FieldDto2 = CreateFieldDto(Field2);

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            MapperMock.Setup(x => x.Map<FieldDto>(Field1)).Returns(FieldDto1);
            MapperMock.Setup(x => x.Map<FieldDto>(Field2)).Returns(FieldDto2);

            _getFieldsQueryHandler = new GetFieldsQueryHandler(RepositoryMock.Object, MapperMock.Object);
        }

        [TestMethod]
        public void Handle_HappyPath_ReturnsFields()
        {
            RepositoryMock.Setup(x => x.Get<Field>()).Returns(new Field[]
            {
                Field1,
                Field2
            });

            var request = new GetFieldsQuery();
            var result = _getFieldsQueryHandler.Handle(request, new CancellationToken()).Result.ToList();

            var FieldDto1 = result.Single(x => x.Id == Field1.Id);
            AssertField(Field1, FieldDto1);

            var FieldDto2 = result.Single(x => x.Id == Field2.Id);
            AssertField(Field2, FieldDto2);
        }
    }
}
