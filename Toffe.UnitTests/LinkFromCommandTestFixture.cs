using Moq;
using Toffee;
using Toffee.Infrastructure;

namespace Toffe.UnitTests
{
    internal class LinkFromCommandTestFixture : ITestFixture<LinkFromCommand>
    {
        public Mock<ICommandArgsParser<LinkFromCommandArgs>> CommandArgsParser { get; } = new Mock<ICommandArgsParser<LinkFromCommandArgs>>();
        public Mock<ILinkRegistryFile> LinkRegistryFile { get; } = new Mock<ILinkRegistryFile>();
        public Mock<IUserInterface> UserInterface { get; } = new Mock<IUserInterface>();

        public LinkFromCommand CreateSut()
        {
            return new LinkFromCommand(CommandArgsParser.Object, LinkRegistryFile.Object, UserInterface.Object);
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