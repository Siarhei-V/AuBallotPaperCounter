using AuBallotPaperCounter.BLL;
using AuBallotPaperCounter.BLL.Interfaces;
using AuBallotPaperCounter.BLL.Models;
using AuBallotPaperCounter.UseCases.DTOs;
using AuBallotPaperCounter.UseCases.Interfaces;
using AuBallotPaperCounter.UseCases.Models;
using AutoMapper;

namespace AuBallotPaperCounter.UseCases
{
    public class Interactor : IInteractor
    {
        IInteractorOutputPort _outputPort;
        IDataService<AllTestBlocksDTO> _allTestBlocksService;
        ICandidateAllocator _candidateAllocator;
        ICandidateAllocatorFactory _candidateAllocatorFactory;
        IDataValidator<AllTestBlocksDTO> _dataValidator;

        AllTestBlocksDTO _allTestBlocksDTO;
        ResultsModel _resultsModel;
        AllCandidates _allCandidates;
        AllBallotPapers _allBallotPapers;

        public Interactor(IInteractorOutputPort outputPort, ICandidateAllocatorFactory candidateAllocatorFactory,
            IDataService<AllTestBlocksDTO> allTestBlocksService, IDataValidator<AllTestBlocksDTO> dataValidator,
            AllCandidates allCandidates, AllBallotPapers allBallotPapers, AllTestBlocksDTO allTestBlocksDTO,
            ResultsModel resultsModel)
        {
            _outputPort = outputPort;
            _candidateAllocatorFactory = candidateAllocatorFactory;
            _allTestBlocksService = allTestBlocksService;
            _dataValidator = dataValidator;
            
            _allCandidates = allCandidates;
            _allBallotPapers = allBallotPapers;
            _allTestBlocksDTO = allTestBlocksDTO;
            _resultsModel = resultsModel;
        }

        public async Task StartAllocationFacadeAsync()
        {
            await GetData();

            _dataValidator.ValidateInputData(_allTestBlocksDTO);

            await StartDataHandlingAsync();

            _outputPort.HandleResult(_resultsModel);
        }

        #region Private Methods
        private async Task GetData()
        {
            var testBlocksTask = _allTestBlocksService.GetData();

            try
            {
                _allTestBlocksDTO = await testBlocksTask;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task StartDataHandlingAsync()
        {
            List<Task<Results>> tasksList = new List<Task<Results>>();

            foreach (var item in _allTestBlocksDTO.TestBlocksList)
            {
                MapDtos(item);

                _candidateAllocator = _candidateAllocatorFactory.MakeAllocator();
                tasksList.Add(Task.Run(() => _candidateAllocator.AllocateCandidate(_allCandidates, _allBallotPapers)));
            }

            await Task.WhenAll(tasksList);

            foreach (var item in tasksList)
            {
                _resultsModel.ResultsList.Add(item.Result);
            }
        }
        #endregion

        #region Mappers
        private void MapDtos(TestBlockDTO item)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<AllCandidatesDTO, AllCandidates>()
                .ForMember(d => d.CandidatesList, e => e.MapFrom(s => MapCandidatesList(s.CandidatesList)))
            ).CreateMapper();

            _allCandidates = mapper.Map<AllCandidatesDTO, AllCandidates>(item.AllCandidates);

            mapper = new MapperConfiguration(c => c.CreateMap<AllBallotPapersDTO, AllBallotPapers>()
                .ForMember(d => d.BallotPapersList, e => e.MapFrom(s => MapBallotPaperList(s.BallotPapersList)))
            ).CreateMapper();

            _allBallotPapers = mapper.Map<AllBallotPapersDTO, AllBallotPapers>(item.AllBallotPapers);
        }

        private List<Candidate> MapCandidatesList(List<CandidateDTO> candidatesList)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<CandidateDTO, Candidate>()).CreateMapper();
            return mapper.Map<List<CandidateDTO>, List<Candidate>>(candidatesList);
        }

        private List<BallotPaper> MapBallotPaperList(List<BallotPaperDTO> ballotPapersList)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<BallotPaperDTO, BallotPaper>()).CreateMapper();
            return mapper.Map<List<BallotPaperDTO>, List<BallotPaper>>(ballotPapersList);
        }
        #endregion
    }
}