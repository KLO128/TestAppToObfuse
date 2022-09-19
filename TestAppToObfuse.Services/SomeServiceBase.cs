namespace TestAppToObfuse.Services
{
    public abstract class SomeServiceBase : ISomeService
    {
        protected IDependencyService DependencyService { get; }

        private string? previousResult;

        public string? PreviousResult { get { return previousResult; } set { previousResult = value; } }

        public SomeServiceBase(IDependencyService dependencyService)
        {
            DependencyService = dependencyService;
        }

        public abstract bool DoIt(string what);
    }
}
