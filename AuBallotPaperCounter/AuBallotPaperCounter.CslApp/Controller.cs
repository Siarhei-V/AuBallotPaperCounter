using AuBallotPaperCounter.CslApp.Interfaces;
using AuBallotPaperCounter.UseCases.Interfaces;

namespace AuBallotPaperCounter.CslApp
{
    public class Controller : IController
    {
        IInteractor _interactor;

        public Controller(IInteractor interactor)
        {
            _interactor = interactor;
        }

        public async Task StartAllocation()
        {
            try
            {
                await _interactor.StartAllocationFacadeAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка выполнения приложения");
                Console.WriteLine(ex.Message);
            }
        }
    }
}
