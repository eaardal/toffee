using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using Toffee.Core.Infrastructure;

namespace Toffee.Core.UnitTests
{
    [TestClass]
    public class LinkFromCommandTests
    {
        private LinkFromCommandTestFixture _fixture;

        [TestMethod]
        public void SetUp()
        {
            _fixture = new LinkFromCommandTestFixture();
        }

        [TestMethod]
        public void CanHandle_WhenCommandIsLinkFrom_ReturnsTrue()
        {
            var sut = _fixture.CreateSut();

            sut.CanExecute("link-from").ShouldBeTrue();
        }

        [TestMethod]
        public void Handle_WhenArgsAreInvalid_ReturnsErrorExitCode()
        {
            var args = new string[0];

            var sut = _fixture
                .SetupInvalidArgs()
                .CreateSut();

            var exitCode = sut.Execute(args);

            exitCode.ShouldBe(ExitCodes.Error);
        }
    }
}
