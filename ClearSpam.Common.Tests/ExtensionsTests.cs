using AutoMapper;
using ClearSpam.TestsCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace ClearSpam.Common.Tests
{
    [TestClass]
    public class ExtensionsTests : TestBase
    {
        [TestMethod]
        public void Compare_HappyPath()
        {
            var classA1 = new ClassA(1, NewGuid());
            var classA2 = new ClassA(2, NewGuid());

            var mapper = new Mapper(new MapperConfiguration(x => x.AddProfile(typeof(ClassBMappings))));

            var result = mapper.MapList<ClassA, ClassB>(new[] { classA1, classA2 });

            var classB1 = result.Single(x => x.Id == classA1.Id);
            Assert.AreEqual(classA1.Description, classB1.Description);

            var classB2 = result.Single(x => x.Id == classA2.Id);
            Assert.AreEqual(classA2.Description, classB2.Description);
        }
    }

    class ClassA
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public ClassA()
        {

        }

        public ClassA(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }

    class ClassB
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public ClassB()
        {

        }

        public ClassB(int id, string description)
        {
            Id = id;
            Description = description;
        }
    }

    class ClassBMappings : Profile
    {
        public ClassBMappings()
        {
            CreateMap<ClassA, ClassB>()
                .ReverseMap();
        }
    }
}
