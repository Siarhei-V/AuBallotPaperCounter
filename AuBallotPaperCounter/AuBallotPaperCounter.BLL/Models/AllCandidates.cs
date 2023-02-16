namespace AuBallotPaperCounter.BLL.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        internal int NumberOfVotes { get; set; }
        internal decimal VotePercentage { get; set; }
    }

    public class AllCandidates
    {
        public List<Candidate> CandidatesList { get; set; } = new List<Candidate>();
    }
}
