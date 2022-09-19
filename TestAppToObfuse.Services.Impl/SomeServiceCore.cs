namespace TestAppToObfuse.Services.Impl
{
    public abstract class SomeServiceCore : SomeServiceBase
    {
        protected SomeServiceCore(IDependencyService dependencyService) : base(dependencyService)
        {
        }
    }
}
