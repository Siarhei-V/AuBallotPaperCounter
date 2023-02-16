using AuBallotPaperCounter.DAL.Interfaces;
using AuBallotPaperCounter.DAL.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AuBallotPaperCounter.DAL.Tests
{
    public class BallotPaperParserTests
    {
        ITestBlocksGetter _dataParser;
        Mock<IDataReader> _dataReaderMock;

        AllTestBlocksModel _allTestBlocksModel;

        string _numberOfBlocks =
            "4\r\n" +
            "";

        string _firstBlock =
            "3\r\n" +
            "John Doe\r\n" +
            "Jane Smith\r\n" +
            "Jane Austen\r\n" +
            "1 2 3\r\n" +
            "2 1 3\r\n" +
            "2 3 1\r\n" +
            "1 2 3\r\n" +
            "3 1 2\r\n" +
            "";

        string _secondBlock =
            "4\r\n" +
            "Sam\r\n" +
            "Dean\r\n" +
            "Castiel\r\n" +
            "Crowley\r\n" +
            "1 2 3 4\r\n" +
            "1 2 4 3\r\n" +
            "1 3 2 4\r\n" +
            "2 1 3 4\r\n" +
            "2 3 1 4\r\n" +
            "2 3 4 1\r\n" +
            "3 1 2 4\r\n" +
            "3 2 1 4\r\n" +
            "4 1 2 3\r\n" +
            "4 3 1 2\r\n" +
            "";

        string _thirdBlock =
            "4\r\n" +
            "Ваня\r\n" +
            "Петя\r\n" +
            "Анатолий\r\n" +
            "Маша\r\n" +
            "1 2 3 4\r\n" +
            "1 2 4 3\r\n" +
            "1 3 2 4\r\n" +
            "2 1 3 4\r\n" +
            "2 3 1 4\r\n" +
            "2 3 4 1\r\n" +
            "3 1 2 4\r\n" +
            "3 2 1 4\r\n" +
            "4 2 1 3\r\n" +
            "4 3 1 2\r\n" +
            "";

        string _forthBlock =
            "11\r\n" + 
            "Анатолий 1\r\n" + 
            "Анатолий 2\r\n" + 
            "Анатолий 3\r\n" + 
            "Анатолий 4\r\n" + 
            "Анатолий 5\r\n" + 
            "Анатолий 6\r\n" + 
            "Анатолий 7\r\n" + 
            "Анатолий 8\r\n" +
            "Анатолий 9\r\n" +
            "Анатолий 10\r\n" +
            "Анатолий 11\r\n" + 
            "2 1 3 4 5 6 7 8 9 10 11\r\n" + 
            "2 3 4 5 6 7 8 9 10 11 1\r\n" + 
            "3 4 5 6 7 8 9 10 11 1 2\r\n" + 
            "4 5 6 7 8 9 10 11 1 2 3\r\n" + 
            "5 6 7 8 9 10 11 1 2 3 4\r\n" + 
            "6 7 8 9 10 11 1 2 3 4 5\r\n" + 
            "7 8 9 10 11 1 2 3 4 5 6\r\n" + 
            "8 9 10 11 1 2 3 4 5 6 7\r\n" + 
            "9 10 11 1 2 3 4 5 6 7 8\r\n" + 
            "10 11 1 2 3 4 5 6 7 8 9\r\n" + 
            "11 1 2 3 4 5 6 7 8 9 10\r\n" + 
            "1 2 3 4 5 6 7 8 9 10 11\r\n" + 
            "2 3 4 5 6 7 8 9 10 11 1\r\n" + 
            "3 4 5 6 7 8 9 10 11 1 2\r\n" + 
            "4 5 6 7 8 9 10 11 1 2 3\r\n" + 
            "5 6 7 8 9 10 11 1 2 3 4\r\n" + 
            "6 7 8 9 10 11 1 2 3 4 5\r\n" + 
            "7 8 9 10 11 1 2 3 4 5 6\r\n" + 
            "8 9 10 11 1 2 3 4 5 6 7\r\n" + 
            "9 10 11 1 2 3 4 5 6 7 8\r\n" + 
            "10 11 1 2 3 4 5 6 7 8 9\r\n" + 
            "11 1 2 3 4 5 6 7 8 9 10\r\n" + 
            "1 2 3 4 5 6 7 8 9 10 11\r\n" + 
            "2 3 4 5 6 7 8 9 10 11 1\r\n" +
            "3 4 5 6 7 8 9 10 11 1 2\r\n" + 
            "";

        List<string> _firstBlockList = new List<string>();
        List<string> _secondBlockList = new List<string>();
        List<string> _thirdBlockList = new List<string>();
        List<string> _forthBlockList = new List<string>();


        public BallotPaperParserTests()
        {
            _firstBlockList = _firstBlock.Split("\r\n").ToList();
            _secondBlockList = _secondBlock.Split("\r\n").ToList();
            _thirdBlockList = _thirdBlock.Split("\r\n").ToList();
            _forthBlockList = _forthBlock.Split("\r\n").ToList();

            _allTestBlocksModel = new AllTestBlocksModel();
            _dataReaderMock = new Mock<IDataReader>();

            _dataParser = new BallotPaperParser(_dataReaderMock.Object, _allTestBlocksModel);
        }

        [Fact]
        public async Task GetTestBlocks_CheckReturnedValue()
        {
            // Arrange
            var dataList = _numberOfBlocks.Split("\r\n")
                .Concat(_firstBlockList)
                .Concat(_secondBlockList)
                .Concat(_thirdBlockList)
                .Concat(_forthBlockList)
                .ToList();

            _dataReaderMock.Setup(m => m.ReadData()).Returns(dataList);

            // Act
            var result = await _dataParser.GetTestBlocks();

            // Assert
            Assert.Equal(4, result.NumberOfBlocks);

            Assert.Equal(1, result.TestBlocksList[0].Id);
            Assert.Equal(4, result.TestBlocksList[1].AllCandidatesModel.NumberOFCandidates);
            Assert.Equal("Петя", result.TestBlocksList[2].AllCandidatesModel.CandidatesList[1].Name);
            Assert.Equal(25, result.TestBlocksList[3].AllBallotPapersModel.NumberOFBallotPapers);
            Assert.Equal(3, result.TestBlocksList[0].AllBallotPapersModel.BallotPapersList[1].Votes[2]);
        }

        [Fact]
        public async Task GetTestBlocks_CheckFirstNumberIsNotNumberException()
        {
            // Arrange
            string badNumberOfBlocks =
                "A\r\n" +
                "";

            var dataList = badNumberOfBlocks.Split("\r\n").ToList();
            _dataReaderMock.Setup(m => m.ReadData()).Returns(dataList);

            // Act
            string notOkResult = string.Empty;
            
            try
            {
                await _dataParser.GetTestBlocks();
            }
            catch (Exception ex)
            {
                notOkResult = ex.Message;
            }

            // Assert
            Assert.Equal("Первая строка в файле должна содержать целое число", notOkResult);
        }

        [Fact]
        public async Task GetTestBlocks_CheckNumberOfCandidatesIsNotIntException()
        {
            // Arrange
            string badNumberOfCandidates =
                "4\r\n" +
                "" + 
                "A";

            var dataList = badNumberOfCandidates.Split("\r\n").ToList();
            _dataReaderMock.Setup(m => m.ReadData()).Returns(dataList);

            // Act
            string notOkResult = string.Empty;

            try
            {
                await _dataParser.GetTestBlocks();
            }
            catch (Exception ex)
            {
                notOkResult = ex.Message;
            }

            // Assert
            Assert.Equal("Количество кандидатов должно быть целым числом", notOkResult);
        }
    }
}