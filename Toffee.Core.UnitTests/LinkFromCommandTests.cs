using System;
using NUnit.Framework;
using Shouldly;

namespace Toffee.Core.UnitTests
{
    [TestFixture]
    public class LinkFromCommandTests
    {
        private LinkFromCommandTestFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new LinkFromCommandTestFixture();
        }

        [Test]
        public void CanHandle_WhenCommandIsLinkFrom_ReturnsTrue()
        {
            var sut = _fixture.CreateSut();

            sut.CanHandle("link-from").ShouldBeTrue();
        }

        [Test]
        public void Handle_WhenArgsAreInvalid_ReturnsErrorExitCode()
        {
            var args = new string[0];

            var sut = _fixture
                .SetupInvalidArgs()
                .CreateSut();

            var exitCode = sut.Handle(args);

            exitCode.ShouldBe(ExitCodes.Error);
        }
    }
}
