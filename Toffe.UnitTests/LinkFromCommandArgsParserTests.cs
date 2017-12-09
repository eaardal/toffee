using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using Shouldly;
using Toffee;

namespace Toffe.UnitTests
{
    [TestClass]
    public class LinkFromCommandArgsParserTests
    {
        private LinkFromCommandArgsParserTestFixture _fixture;

        [TestMethod]
        public void SetUp()
        {
            _fixture = new LinkFromCommandArgsParserTestFixture();
        }

        [TestMethod]
        public void CreatesSystemUnderTest()
        {
            var sut = _fixture.CreateSut();

            sut.ShouldNotBeNull();
            sut.ShouldBeAssignableTo<ICommandArgsParser<LinkFromCommandArgs>>();
        }
    }
}
