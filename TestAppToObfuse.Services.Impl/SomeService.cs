namespace TestAppToObfuse.Services.Impl
{
    public class SomeService : SomeServiceCore
    {
        protected bool LikeYouDoItToMe { get; set; } = false;

        public SomeService(IDependencyService dependencyService, bool likeYouDoItToMe) : base(dependencyService)
        {
            LikeYouDoItToMe = likeYouDoItToMe;
            Init();
        }

        protected bool DoItInner(string what)
        {
            var ret = what == PreviousResult;

            PreviousResult = LikeYouDoItToMe ? string.Concat(nameof(DoIt), ' ', nameof(LikeYouDoItToMe), ": ", what) : what;

            return ret;
        }

        internal void Init()
        {
            PreviousResult = nameof(DoIt);
        }

        public override bool DoIt(string what)
        {
            return DoItInner(what);
        }
    }
}
