namespace AuBallotPaperCounter.BLL.Models
{
    public class BallotPaper
    {
        public List<int> Votes { get; set; } = new List<int>();
        internal bool IsDeleteNeeded { get; set; }
    }

    public class AllBallotPapers
    {
        public int NumberOFBallotPapers { get; set; }
        public List<BallotPaper> BallotPapersList { get; set; } = new List<BallotPaper>();
    }
}
