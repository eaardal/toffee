using Moq;
using Toffee.Core;
using Toffee.Core.Infrastructure;

namespace Toffe.Core.UnitTests
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