using AuBallotPaperCounter.BLL.Interfaces;

namespace AuBallotPaperCounter.UseCases.Interfaces
{
    public interface ICandidateAllocatorFactory
    {
        ICandidateAllocator MakeAllocator();
    }
}
