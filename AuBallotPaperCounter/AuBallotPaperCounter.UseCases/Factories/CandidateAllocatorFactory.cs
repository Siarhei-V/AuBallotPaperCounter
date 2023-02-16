using AuBallotPaperCounter.BLL;
using AuBallotPaperCounter.BLL.Interfaces;
using AuBallotPaperCounter.BLL.Models;
using AuBallotPaperCounter.UseCases.Interfaces;

namespace AuBallotPaperCounter.UseCases.Factories
{
    public class CandidateAllocatorFactory : ICandidateAllocatorFactory
    {
        public ICandidateAllocator MakeAllocator()
        {
            return new CandidateAllocator(new Results());
        }
    }
}
