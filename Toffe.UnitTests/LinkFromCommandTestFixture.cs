using Moq;
using Serilog;
using Toffee;
using Toffee.Infrastructure;

namespace Toffe.UnitTests
{
    internal class LinkFromCommandTestFixture : ITestFixture<LinkFromCommand>
    {
        public Mock<ICommandArgsParser<LinkFromCommandArgs>> CommandArgsParser { get; } = new Mock<ICommandArgsParser<LinkFromCommandArgs>>();
        public Mock<ILinkRegistryFile> LinkRegistryFile { get; } = new Mock<ILinkRegistryFile>();
        public Mock<IUserInterface> UserInterface { get; } = new Mock<IUserInterface>();
        public Mock<ILogger> Logger { get; } = new Mock<ILogger>();

        public LinkFromCommand CreateSut()
        {
            return new LinkFromCommand(CommandArgsParser.Object, LinkRegistryFile.Object, UserInterface.Object, Logger.Object);
        }

        public LinkFromCommandTestFixture SetupInvalidArgs()
        {
            CommandArgsParser
                .Setup(x => x.IsValid(It.IsAny<string[]>()))
                .Returns((false, "Test says no"));

            return this;
        }
    }
}