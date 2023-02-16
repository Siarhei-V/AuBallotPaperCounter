namespace AuBallotPaperCounter.UseCases.DTOs
{
    public class CandidateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class AllCandidatesDTO
    {
        public int NumberOFCandidates { get; set; }
        public List<CandidateDTO> CandidatesList { get; set; } = new List<CandidateDTO>();
    }
}
