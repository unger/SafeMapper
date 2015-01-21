namespace MapEverything.Tests
{
    using System.Globalization;

    using MapEverything.Tests.Model.Person;
    using MapEverything.Utils;

    using NUnit.Framework;

    [TestFixture]
    public class ConverterFactoryTests
    {
        [Test]
        public void CreateConverter_CallDelegateWithPrimitive_ShouldReturnInstanceOfToType()
        {
            var converter = ConverterFactory.Create<string, int>();

            var result = converter("10");

            Assert.IsInstanceOf<int>(result);
        }

        [Test]
        public void CreateConverter_CallDelegateWithClass_ShouldReturnInstanceOfToType()
        {
            var converter = ConverterFactory.Create<Person, PersonDto>();

            var result = converter(new Person());

            Assert.IsInstanceOf<PersonDto>(result);
        }

        [Test]
        public void CreateConverter_CallDelegateWithPrimitive_ShouldReturnCorrectValue()
        {
            var expected = 10;
            var converter = ConverterFactory.Create<string, int>();

            var result = converter(expected.ToString());

            Assert.AreEqual(expected, result);
        }
    }
}
