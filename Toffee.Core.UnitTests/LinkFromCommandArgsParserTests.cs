using NUnit.Framework;
using Shouldly;

namespace Toffee.Core.UnitTests
{
    [TestFixture]
    public class LinkFromCommandArgsParserTests
    {
        private LinkFromCommandArgsParserTestFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new LinkFromCommandArgsParserTestFixture();
        }

        [Test]
        public void CreatesSystemUnderTest()
        {
            var sut = _fixture.CreateSut();

            sut.ShouldNotBeNull();
            sut.ShouldBeAssignableTo<ICommandArgsParser<LinkFromCommandArgs>>();
        }
    }
}
