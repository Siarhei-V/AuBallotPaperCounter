using AuBallotPaperCounter.UseCases.DataValidation;
using AuBallotPaperCounter.UseCases.DTOs;
using AuBallotPaperCounter.UseCases.Interfaces;
using System;
using Xunit;

namespace AuBallotPaperCounter.UseCases.Tests
{
    public class InputDataValidatorTests
    {
        IDataValidator<AllTestBlocksDTO> _dataValidator = new InputDataValidator();

        AllTestBlocksDTO _allTestBlocksDTO = new AllTestBlocksDTO();
        TestBlockDTO _testBlockDTO = new TestBlockDTO();
        AllCandidatesDTO _allCandidatesDTO = new AllCandidatesDTO();
        CandidateDTO _candidateDTO = new CandidateDTO();
        AllBallotPapersDTO _allBallotPapersDTO = new AllBallotPapersDTO();
        BallotPaperDTO _ballotPaperDTO = new BallotPaperDTO();
        string _result = string.Empty;

        public InputDataValidatorTests()
        {
            _candidateDTO.Id = 1;
            _candidateDTO.Name = "Name";

            _allCandidatesDTO.NumberOFCandidates = 1;
            _allCandidatesDTO.CandidatesList.Add(_candidateDTO);

            _ballotPaperDTO.Votes.Add(1);

            _allBallotPapersDTO.NumberOFBallotPapers = 1;
            _allBallotPapersDTO.BallotPapersList.Add(_ballotPaperDTO);
            
            _testBlockDTO.Id = 1;
            _testBlockDTO.AllCandidates = _allCandidatesDTO;
            _testBlockDTO.AllBallotPapers = _allBallotPapersDTO;

            _allTestBlocksDTO.NumberOfBlocks = 1;
            _allTestBlocksDTO.TestBlocksList.Add(_testBlockDTO);
        }

        [Theory]
        [InlineData(-5, "Количество блоков должно быть положительным")]
        [InlineData(0, "Количество блоков должно быть положительным")]
        [InlineData(5, "")]
        public void ValidateInputData_NumberOfTestBlocksException(int numberOfBlocks, string exceptionMessage)
        {
            // Arrange
            _allTestBlocksDTO.NumberOfBlocks = numberOfBlocks;

            // Act
            try
            {
                _dataValidator.ValidateInputData(_allTestBlocksDTO);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            // Assert
            Assert.Equal(exceptionMessage, _result);
        }

        [Theory]
        [InlineData(25, "Число кандидатов в блоке 1 превышает 20")]
        [InlineData(1, "")]
        public void ValidateInputData_NumberOfCandidatesInBlockException(int numberOfCandidates, string exceptionMessage)
        {
            // Arrange
            _testBlockDTO.AllCandidates.NumberOFCandidates = numberOfCandidates;

            // Act
            try
            {
                _dataValidator.ValidateInputData(_allTestBlocksDTO);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            // Assert
            Assert.Equal(exceptionMessage, _result);
        }

        [Theory]
        [InlineData("123456789!123456789!123456789!123456789!123456789!123456789!123456789!123456789!",
            "Имя \"123456789!123456789!123456789!123456789!123456789!123456789!123456789!123456789!\" слишком длинное")]
        [InlineData("", "")]
        public void ValidateInputData_LongNameException(string name, string exceptionMessage)
        {
            // Arrange
            _candidateDTO.Name = name;

            // Act
            try
            {
                _dataValidator.ValidateInputData(_allTestBlocksDTO);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            // Assert
            Assert.Equal(exceptionMessage, _result);
        }

        [Theory]
        [InlineData(1000, "В блоке 1 превышено количество бюллетеней")]
        [InlineData(1, "")]
        public void ValidateInputData_NumberOfBallotPapersException(int numberOfBallotPapers, string exceptionMessage)
        {
            // Arrange
            _allBallotPapersDTO.NumberOFBallotPapers = numberOfBallotPapers;

            // Act
            try
            {
                _dataValidator.ValidateInputData(_allTestBlocksDTO);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            // Assert
            Assert.Equal(exceptionMessage, _result);
        }

        [Theory]
        [InlineData(1, "В блоке 1 есть строка с повторяющимися голосами")]
        [InlineData(2, "")]
        public void ValidateInputData_NotUniqueVotesException(int index, string exceptionMessage)
        {
            // Arrange
            _allCandidatesDTO.NumberOFCandidates = 4;

            switch (index)
            {
                case 1: 
                    _ballotPaperDTO.Votes.AddRange(new[] { 1, 2, 2, 4 });
                    break;
                case 2:
                    _ballotPaperDTO.Votes.Clear();
                    _ballotPaperDTO.Votes.AddRange(new[] { 1, 2, 3, 4 });
                    break;
                default:
                    break;
            }

            // Act
            try
            {
                _dataValidator.ValidateInputData(_allTestBlocksDTO);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            // Assert
            Assert.Equal(exceptionMessage, _result);
        }

        [Theory]
        [InlineData(1, "В блоке 1 есть строки с неправильным количеством голосов")]
        [InlineData(2, "")]
        public void ValidateInputData_NumberOfVotesException(int index, string exceptionMessage)
        {
            // Arrange
            _allCandidatesDTO.NumberOFCandidates = 4;

            switch (index)
            {
                case 1:
                    _ballotPaperDTO.Votes.AddRange(new[] { 1, 2, 3, 4, 5 });
                    break;
                case 2:
                    _ballotPaperDTO.Votes.Clear();
                    _ballotPaperDTO.Votes.AddRange(new[] { 1, 2, 3, 4 });
                    break;
                default:
                    break;
            }

            // Act
            try
            {
                _dataValidator.ValidateInputData(_allTestBlocksDTO);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            // Assert
            Assert.Equal(exceptionMessage, _result);
        }

        [Theory]
        [InlineData(1, "В блоке 1 есть строки с голосами за несуществующих кандидатов")]
        [InlineData(2, "")]
        public void ValidateInputData_VoteForNonExistentCandidateException(int index, string exceptionMessage)
        {
            // Arrange
            _allCandidatesDTO.NumberOFCandidates = 4;

            switch (index)
            {
                case 1:
                    _ballotPaperDTO.Votes.Clear();
                    _ballotPaperDTO.Votes.AddRange(new[] { 1, 2, 3, 5 });
                    break;
                case 2:
                    _ballotPaperDTO.Votes.Clear();
                    _ballotPaperDTO.Votes.AddRange(new[] { 1, 2, 3, 4 });
                    break;
                default:
                    break;
            }

            // Act
            try
            {
                _dataValidator.ValidateInputData(_allTestBlocksDTO);
            }
            catch (Exception ex)
            {
                _result = ex.Message;
            }

            // Assert
            Assert.Equal(exceptionMessage, _result);
        }
    }
}
