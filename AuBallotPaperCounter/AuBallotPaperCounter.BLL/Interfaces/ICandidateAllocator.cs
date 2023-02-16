using AuBallotPaperCounter.BLL.Models;

namespace AuBallotPaperCounter.BLL.Interfaces
{
    public interface ICandidateAllocator
    {
        Results AllocateCandidate(AllCandidates allCandidates, AllBallotPapers allBallotPapers);
    }
}
