using Moq;
using Toffee;

namespace Toffe.UnitTests
{
    internal class LinkFromCommandTestFixture : ITestFixture<LinkFromCommand>
    {
        public Mock<ICommandArgsParser<LinkFromCommandArgs>> CommandArgsParser { get; } = new Mock<ICommandArgsParser<LinkFromCommandArgs>>();

        public LinkFromCommand CreateSut()
        {
            return new LinkFromCommand(CommandArgsParser.Object);
        }

        public LinkFromCommandTestFixture SetupInvalidArgs()
        {
            CommandArgsParser
                .Setup(x => x.IsValid(It.IsAny<string[]>()))
                .Returns(false);

            return this;
        }
    }
}