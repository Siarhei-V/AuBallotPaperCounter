namespace AuBallotPaperCounter.DAL.Models
{
    public class TestBlock
    {
        public int Id { get; set; }
        public AllCandidatesModel AllCandidatesModel { get; set; }
        public AllBallotPapersModel AllBallotPapersModel { get; set; }
    }

    public class AllTestBlocksModel
    {
        public int NumberOfBlocks { get; set; }
        public List<TestBlock> TestBlocksList { get; set; } = new List<TestBlock>();
    }
}
