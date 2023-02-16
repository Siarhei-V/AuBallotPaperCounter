namespace AuBallotPaperCounter.UseCases.Interfaces
{
    public interface IDataValidator<in T> where T : class
    {
        void ValidateInputData(T data);
    }
}
