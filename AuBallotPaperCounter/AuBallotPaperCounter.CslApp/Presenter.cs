using AuBallotPaperCounter.UseCases.Interfaces;
using AuBallotPaperCounter.UseCases.Models;
using System.Text;

namespace AuBallotPaperCounter.CslApp
{
    public class Presenter : IInteractorOutputPort
    {
        public void HandleResult(ResultsModel results)
        {
            foreach (var item in results.ResultsList)
            {
                Console.OutputEncoding = Encoding.UTF8;
                Console.WriteLine(item.WinnerName);
            }
        }
    }
}
