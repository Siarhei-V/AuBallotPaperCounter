namespace AuBallotPaperCounter.UseCases.Interfaces
{
    public interface IDataService<T> where T : class
    {
        Task<T> GetData();
    }
}
