namespace Toffe.Core.UnitTests
{
    internal interface ITestFixture<T>
    {
        T CreateSut();
    }
}