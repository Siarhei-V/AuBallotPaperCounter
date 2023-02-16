namespace AuBallotPaperCounter.DAL.Models
{
    public class CandidateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AllCandidatesModel
    {
        public int NumberOFCandidates { get; set; }
        public List<CandidateModel> CandidatesList { get; set; } = new List<CandidateModel>();
    }
}
