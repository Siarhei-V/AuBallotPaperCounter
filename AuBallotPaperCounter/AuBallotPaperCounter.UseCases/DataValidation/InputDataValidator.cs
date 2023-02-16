using AuBallotPaperCounter.UseCases.DTOs;
using AuBallotPaperCounter.UseCases.Interfaces;

namespace AuBallotPaperCounter.UseCases.DataValidation
{
    public class InputDataValidator : IDataValidator<AllTestBlocksDTO>
    {
        public void ValidateInputData(AllTestBlocksDTO allTestBlocksDTO)    // TODO: перенести валидацию в BLL?
        {
            if (allTestBlocksDTO.NumberOfBlocks <= 0)
                throw new ArgumentException("Количество блоков должно быть положительным");

            foreach (var testBlockDTO in allTestBlocksDTO.TestBlocksList)
            {
                if (testBlockDTO.AllCandidates.NumberOFCandidates > 20)
                    throw new ArgumentException($"Число кандидатов в блоке {testBlockDTO.Id} превышает 20");

                var longName = testBlockDTO.AllCandidates.CandidatesList.FirstOrDefault(c => c.Name.Length >= 80);

                if (longName != null)
                    throw new ArgumentException($"Имя \"{longName.Name}\" слишком длинное");

                if (testBlockDTO.AllBallotPapers.NumberOFBallotPapers >= 1000)
                    throw new ArgumentException($"В блоке {testBlockDTO.Id} превышено количество бюллетеней");

                foreach (var ballotPaperDTO in testBlockDTO.AllBallotPapers.BallotPapersList)
                {
                    var res = ballotPaperDTO.Votes.Distinct().ToList();

                    if (res.Count() < testBlockDTO.AllCandidates.NumberOFCandidates)
                        throw new ArgumentException($"В блоке {testBlockDTO.Id} есть строка с повторяющимися " +
                            $"голосами");

                    if (ballotPaperDTO.Votes.Count != testBlockDTO.AllCandidates.NumberOFCandidates)
                        throw new ArgumentException($"В блоке {testBlockDTO.Id} есть строки с неправильным количеством голосов");

                    if (ballotPaperDTO.Votes
                        .FirstOrDefault(v => v < 1 || v > testBlockDTO.AllCandidates.NumberOFCandidates) != 0)
                        throw new ArgumentException($"В блоке {testBlockDTO.Id} есть строки с голосами за несуществующих " +
                            $"кандидатов");
                }
            }
        }
    }
}
