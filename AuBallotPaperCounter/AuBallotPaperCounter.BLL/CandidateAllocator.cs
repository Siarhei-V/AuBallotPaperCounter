using AuBallotPaperCounter.BLL.Interfaces;
using AuBallotPaperCounter.BLL.Models;

namespace AuBallotPaperCounter.BLL
{
    public class CandidateAllocator : ICandidateAllocator
    {
        AllCandidates _allCandidates;
        AllBallotPapers _allBallotPapers;
        Results _results;

        public CandidateAllocator(Results results)
        {
            _results = results;
        }

        public Results AllocateCandidate(AllCandidates allCandidates, AllBallotPapers allBallotPapers)
        {
            _results.WinnerName = null;
            _allCandidates = allCandidates;
            _allBallotPapers = allBallotPapers;

            CalculateFirstPreferences();

            if (_results.WinnerName != null)
            {
                return _results;
            }

            while(_results.WinnerName == null)
            {
                CalculateOtherPreferences();
            }

            return _results;
        }

        #region Private Methods
        private void CalculateFirstPreferences()
        {
            int ballotPaperIndex = 0;

            for (int i = 0; i < _allBallotPapers.BallotPapersList.Count; i++)
            {
                ballotPaperIndex = _allBallotPapers.BallotPapersList[i].Votes.FirstOrDefault();
                _allCandidates.CandidatesList[ballotPaperIndex - 1].NumberOfVotes++;
            }

            CheckResult();
        }

        private void CheckResult()
        {
            foreach (var item in _allCandidates.CandidatesList)
            {
                item.VotePercentage = (decimal)item.NumberOfVotes * 100 / _allBallotPapers.NumberOFBallotPapers;

                if (item.VotePercentage > 50)
                {
                    _results.WinnerName = item.Name;
                    return;
                }
            }

            CheckVoteDraw();
        }

        private void CheckVoteDraw()
        {
            var votePercentage = _allCandidates.CandidatesList.FirstOrDefault().VotePercentage;
            bool isVoteDraw = false;

            isVoteDraw = _allCandidates.CandidatesList.All(c => c.VotePercentage == votePercentage);

            if (isVoteDraw)
            {
                _results.WinnerName = "Победителя нет. Ничья";
            }
        }

        private void CalculateOtherPreferences()
        {
            var minimumVotesNumber = _allCandidates.CandidatesList.Min(c => c.VotePercentage);

            // Запрос для получения айдишников кандидатов с наименьшим количество мголосов
            var loserIds = _allCandidates.CandidatesList
                .Where(c => c.VotePercentage == minimumVotesNumber)
                .Select(c => c.Id).ToList();

            List<BallotPaper> loserBallotPapers = new List<BallotPaper>();

            // Находим бюллетени кандидатов с наименьшим количество голосов
            foreach (var item in loserIds)
            {
                loserBallotPapers.AddRange(_allBallotPapers.BallotPapersList.Where(b => b.Votes.FirstOrDefault() == item).ToList());

                // Бюллетени, которые будут удалены, помечаем как готовые к удалению
                foreach (var value in _allBallotPapers.BallotPapersList)
                {
                    if (value.Votes.FirstOrDefault() == item)
                    {
                        value.IsDeleteNeeded = true;
                    }
                }
            }

            // Удаляем из бюллетеней голоса за выбывающих кандидатов
            foreach (var item in _allBallotPapers.BallotPapersList)
            {
                foreach (var value in loserIds)
                {
                    item.Votes.Remove(value);
                }
            }

            // Голоса за выбывающих кандидатов отдаем следующим кандидатам в порядке предпочтения
            foreach (var item in loserBallotPapers)
            {
                var nextCandidateId = item.Votes.FirstOrDefault();
                _allCandidates.CandidatesList.FirstOrDefault(c => c.Id == nextCandidateId).NumberOfVotes++;
            }

            // Из списка кандидатов удаляем выбывших
            foreach (var item in loserIds)
            {
                _allCandidates.CandidatesList.RemoveAll(c => c.Id == item);
            }

            // Из списка бюллетеней удаляем бюллетени, в которых выбывшие кандидаты были первыми по порядку предпочтения
            _allBallotPapers.BallotPapersList.RemoveAll(bp => bp.IsDeleteNeeded);

            CheckResult();
        }
        #endregion
    }
}