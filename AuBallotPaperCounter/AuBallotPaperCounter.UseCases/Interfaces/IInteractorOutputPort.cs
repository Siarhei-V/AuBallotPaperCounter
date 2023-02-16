using AuBallotPaperCounter.UseCases.Models;

namespace AuBallotPaperCounter.UseCases.Interfaces
{
    public interface IInteractorOutputPort
    {
        void HandleResult(ResultsModel results);
    }
}
