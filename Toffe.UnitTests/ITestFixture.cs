namespace Toffe.UnitTests
{
    internal interface ITestFixture<T>
    {
        T CreateSut();
    }
}