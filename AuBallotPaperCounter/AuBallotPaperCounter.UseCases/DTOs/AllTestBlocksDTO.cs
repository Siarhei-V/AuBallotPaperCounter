namespace AuBallotPaperCounter.UseCases.DTOs
{
    public class TestBlockDTO
    {
        public int Id { get; set; }
        public AllCandidatesDTO AllCandidates { get; set; }
        public AllBallotPapersDTO AllBallotPapers { get; set; }
    }

    public class AllTestBlocksDTO
    {
        public int NumberOfBlocks { get; set; }
        public List<TestBlockDTO> TestBlocksList { get; set; } = new List<TestBlockDTO>();
    }
}
