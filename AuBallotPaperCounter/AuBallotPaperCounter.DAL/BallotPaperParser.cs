using AuBallotPaperCounter.DAL.Interfaces;
using AuBallotPaperCounter.DAL.Models;

namespace AuBallotPaperCounter.DAL
{
    public class BallotPaperParser : ITestBlocksGetter
    {
        AllTestBlocksModel _allTestBlocks;
        IDataReader _dataReader;
        List<string> _textFromFile = new List<string>();
        int _textLineIndex = 0;

        public BallotPaperParser(IDataReader dataReader, AllTestBlocksModel allTestBlocksModel)
        {
            _dataReader = dataReader;
            _allTestBlocks = allTestBlocksModel;
        }

        public async Task<AllTestBlocksModel> GetTestBlocks()
        {
            ParseBallotPaper();

            return await Task.Run(() => _allTestBlocks);    // TODO: ?
        }

        #region Private Methods

        /* TODO:
         * Видимо, нужно было сначала прочитать все блоки, а потом запускать их парсинг асинхронно.
         * Пока оставлю как есть
         */
        private void ParseBallotPaper()
        {
            _textFromFile = _dataReader.ReadData();
            ReadTestBlocks();
        }

        private void ReadTestBlocks()
        {
            try
            {
                _allTestBlocks.NumberOfBlocks = Convert.ToInt32(_textFromFile[_textLineIndex++]);
            }
            catch (Exception)
            {
                throw new ArgumentException("Первая строка в файле должна содержать целое число");
            }

            for (int i = 1; i <= _allTestBlocks.NumberOfBlocks; i++)
            {
                var allCandidatesModel = ReadCandidates();
                var allBallotPapersModel = ReadBallotPapers();

                _allTestBlocks.TestBlocksList.Add(new TestBlock() { 
                    Id = i, 
                    AllCandidatesModel = allCandidatesModel,
                    AllBallotPapersModel = allBallotPapersModel,
                });
            }
        }

        private AllCandidatesModel ReadCandidates()
        {
            var allCandidatesModel = new AllCandidatesModel();
            string candidateName;

            _textLineIndex++;

            try
            {
                allCandidatesModel.NumberOFCandidates = Convert.ToInt32(_textFromFile[_textLineIndex++]);
            }
            catch (Exception)
            {
                throw new ArgumentException("Количество кандидатов должно быть целым числом");
            }

            for (int i = 0; i < allCandidatesModel.NumberOFCandidates; i++)
            {
                candidateName = _textFromFile[_textLineIndex++];

                allCandidatesModel.CandidatesList.Add(new CandidateModel() { Id = i + 1, Name = candidateName });
            }

            return allCandidatesModel;
        }

        private AllBallotPapersModel ReadBallotPapers()
        {
            string? line;
            BallotPaperModel ballotPaperModel;
            var allBallotPapersModel = new AllBallotPapersModel();

            while (_textLineIndex < _textFromFile.Count
                && !string.IsNullOrEmpty(line = _textFromFile[_textLineIndex]))
            {
                var numbersArray = line.Split(' ');
                ballotPaperModel = new BallotPaperModel();

                foreach (var item in numbersArray)
                {
                    ballotPaperModel.Votes.Add(Convert.ToInt32(item.ToString()));
                }

                allBallotPapersModel.BallotPapersList.Add(ballotPaperModel);
                allBallotPapersModel.NumberOFBallotPapers++;

                _textLineIndex++;
            }

            return allBallotPapersModel;
        }
        #endregion
    }
}