namespace AuBallotPaperCounter.UseCases.DTOs
{
    public class BallotPaperDTO
    {
        public List<int> Votes { get; set; } = new List<int>();
    }

    public class AllBallotPapersDTO
    {
        public int NumberOFBallotPapers { get; set; }
        public List<BallotPaperDTO> BallotPapersList { get; set; } = new List<BallotPaperDTO>();
    }
}
