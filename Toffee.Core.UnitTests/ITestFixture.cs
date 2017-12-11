namespace Toffee.Core.UnitTests
{
    internal interface ITestFixture<T>
    {
        T CreateSut();
    }
}