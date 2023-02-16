namespace AuBallotPaperCounter.DAL.Models
{
    public class BallotPaperModel
    {
        public List<int> Votes { get; set; } = new List<int>();
    }

    public class AllBallotPapersModel
    {
        public int NumberOFBallotPapers { get; set; }
        public List<BallotPaperModel> BallotPapersList { get; set; } = new List<BallotPaperModel>();
    }
}
