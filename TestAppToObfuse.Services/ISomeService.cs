namespace TestAppToObfuse.Services
{
    public interface ISomeService
    {
        string? PreviousResult { get; }

        /// <summary>
        /// Returns true if has done the same.
        /// </summary>
        /// <param name="what"></param>
        /// <returns></returns>
        bool DoIt(string what);
    }
}
