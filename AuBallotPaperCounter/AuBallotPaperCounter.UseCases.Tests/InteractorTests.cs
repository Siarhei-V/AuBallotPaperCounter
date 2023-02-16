using AuBallotPaperCounter.BLL;
using AuBallotPaperCounter.BLL.Interfaces;
using AuBallotPaperCounter.BLL.Models;
using AuBallotPaperCounter.UseCases.DTOs;
using AuBallotPaperCounter.UseCases.Interfaces;
using AuBallotPaperCounter.UseCases.Models;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AuBallotPaperCounter.UseCases.Tests
{
    public class InteractorTests
    {
        IInteractor _interactor;

        Mock<IInteractorOutputPort> _outputPortMock;
        Mock<IDataService<AllTestBlocksDTO>> _allTestBlocksServiceMock;
        Mock<ICandidateAllocatorFactory> _candidateAllocatorFactoryMock;
        Mock<IDataValidator<AllTestBlocksDTO>> _dataValidatorMock;

        Mock<ICandidateAllocator> _candidateAllocatorMock;

        AllTestBlocksDTO _allTestBlocksDTO;
        ResultsModel _resultsModel;
        AllCandidates _allCandidates;
        AllBallotPapers _allBallotPapers;

        public InteractorTests()
        {
            _allTestBlocksDTO = new AllTestBlocksDTO();
            _resultsModel = new ResultsModel();
            _allCandidates = new AllCandidates();
            _allBallotPapers = new AllBallotPapers();

            _outputPortMock = new Mock<IInteractorOutputPort>();
            _allTestBlocksServiceMock = new Mock<IDataService<AllTestBlocksDTO>>();
            _candidateAllocatorFactoryMock = new Mock<ICandidateAllocatorFactory>();
            _dataValidatorMock = new Mock<IDataValidator<AllTestBlocksDTO>>();
            _candidateAllocatorMock = new Mock<ICandidateAllocator>();
        }

        [Fact]
        public async Task StartAllocationFacade_CheckMethodsColling()
        {
            // Arrange
            _allTestBlocksDTO.TestBlocksList.AddRange(new List<TestBlockDTO>() 
                {   
                    new TestBlockDTO(),
                    new TestBlockDTO(),
                    new TestBlockDTO(),
                }
            );

            _interactor = new Interactor(_outputPortMock.Object, _candidateAllocatorFactoryMock.Object,
                _allTestBlocksServiceMock.Object, _dataValidatorMock.Object,
                _allCandidates, _allBallotPapers, _allTestBlocksDTO, _resultsModel);

            _allTestBlocksServiceMock.Setup(m => m.GetData()).ReturnsAsync(_allTestBlocksDTO);
            _candidateAllocatorFactoryMock.Setup(m =>
                m.MakeAllocator()).Returns(_candidateAllocatorMock.Object);

            // Act
            await _interactor.StartAllocationFacadeAsync();

            // Assert
            _allTestBlocksServiceMock.Verify(m => m.GetData(), Times.Once);
            _dataValidatorMock.Verify(m => m.ValidateInputData(_allTestBlocksDTO), Times.Once);
            _candidateAllocatorMock.Verify(m => m.AllocateCandidate(It.IsAny<AllCandidates>(), It.IsAny<AllBallotPapers>()), 
                Times.Exactly(_allTestBlocksDTO.TestBlocksList.Count));
            _outputPortMock.Verify(m => m.HandleResult(It.IsAny<ResultsModel>()), Times.Once);
        }
    }
}