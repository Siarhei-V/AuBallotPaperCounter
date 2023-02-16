using AuBallotPaperCounter.DAL.Models;

namespace AuBallotPaperCounter.DAL.Interfaces
{
    public interface ITestBlocksGetter
    {
        Task<AllTestBlocksModel> GetTestBlocks();
    }
}
