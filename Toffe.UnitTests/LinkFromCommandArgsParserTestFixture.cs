using Moq;
using Toffee;
using Toffee.Infrastructure;

namespace Toffe.UnitTests
{
    internal class LinkFromCommandArgsParserTestFixture : ITestFixture<LinkFromCommandArgsParser>
    {
        public Mock<IFilesystem> Filesystem { get; } = new Mock<IFilesystem>();

        public LinkFromCommandArgsParser CreateSut()
        {
            return new LinkFromCommandArgsParser(Filesystem.Object);
        }
    }
}