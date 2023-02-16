using AuBallotPaperCounter.BLL.Interfaces;
using AuBallotPaperCounter.BLL.Models;
using System.Collections.Generic;
using Xunit;

namespace AuBallotPaperCounter.BLL.Tests
{
    public class CandidateAllocatorTests
    {
        ICandidateAllocator _candidateAllocator;
        Results _results;
        AllCandidates _allCandidates;
        AllBallotPapers _allBallotPapers;

        public CandidateAllocatorTests()
        {
            _results = new Results();
            _candidateAllocator = new CandidateAllocator(_results);
            _allCandidates = new AllCandidates();
            _allBallotPapers = new AllBallotPapers();
            
        }

        [Fact]
        public void AllocateCandidate_CheckReturnedWinner()
        {
            // Arrange
            _allCandidates.CandidatesList.AddRange(new List<Candidate>()
                {
                    new Candidate() { Id = 1, Name = "John Connor" },
                    new Candidate() { Id = 2, Name = "Sarah Connor" },
                    new Candidate() { Id = 3, Name = "T-1000" },
                    new Candidate() { Id = 4, Name = "Terminator" },
                }
            );

            _allBallotPapers.NumberOFBallotPapers = 10;
            _allBallotPapers.BallotPapersList.AddRange( new List<BallotPaper>()
                {
                    new BallotPaper() { Votes = new List<int>() { 1, 2, 3, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 1, 2, 4, 3 } },
                    new BallotPaper() { Votes = new List<int>() { 1, 3, 2, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 2, 1, 3, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 2, 3, 1, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 2, 3, 4, 1 } },
                    new BallotPaper() { Votes = new List<int>() { 3, 1, 2, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 3, 2, 1, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 4, 1, 2, 3 } },
                    new BallotPaper() { Votes = new List<int>() { 4, 3, 1, 2 } }
                }
            );

            // Act
            var result = _candidateAllocator.AllocateCandidate(_allCandidates, _allBallotPapers);

            // Assert
            Assert.Equal("John Connor", result.WinnerName);
        }

        [Fact]
        public void AllocateCandidate_CheckDraw()
        {
            // Arrange
            _allCandidates.CandidatesList.AddRange(new List<Candidate>()
                {
                    new Candidate() { Id = 1, Name = "Man 1" },
                    new Candidate() { Id = 2, Name = "Man 2" },
                    new Candidate() { Id = 3, Name = "Man 3" },
                    new Candidate() { Id = 4, Name = "Man 4" },
                }
            );

            _allBallotPapers.NumberOFBallotPapers = 10;
            _allBallotPapers.BallotPapersList.AddRange(new List<BallotPaper>()
                {
                    new BallotPaper() { Votes = new List<int>() { 1, 2, 3, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 1, 2, 4, 3 } },
                    new BallotPaper() { Votes = new List<int>() { 1, 3, 2, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 2, 1, 3, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 2, 3, 1, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 2, 3, 4, 1 } },
                    new BallotPaper() { Votes = new List<int>() { 3, 2, 1, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 3, 4, 2, 1 } },
                    new BallotPaper() { Votes = new List<int>() { 3, 1, 2, 4 } },
                    new BallotPaper() { Votes = new List<int>() { 4, 1, 2, 3 } },
                    new BallotPaper() { Votes = new List<int>() { 4, 1, 3, 2 } },
                    new BallotPaper() { Votes = new List<int>() { 4, 2, 1, 3 } },
                }
            );

            // Act
            var result = _candidateAllocator.AllocateCandidate(_allCandidates, _allBallotPapers);

            // Assert
            Assert.Equal("Победителя нет. Ничья", result.WinnerName);
        }
    }
}
