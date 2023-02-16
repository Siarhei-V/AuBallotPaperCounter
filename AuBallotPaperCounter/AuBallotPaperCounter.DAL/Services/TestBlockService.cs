using AuBallotPaperCounter.DAL.Interfaces;
using AuBallotPaperCounter.DAL.Models;
using AuBallotPaperCounter.UseCases.DTOs;
using AuBallotPaperCounter.UseCases.Interfaces;
using AutoMapper;

namespace AuBallotPaperCounter.DAL.Services
{
    public class TestBlockService : IDataService<AllTestBlocksDTO>
    {
        ITestBlocksGetter _testBlocksGetter;

        public TestBlockService(ITestBlocksGetter testBlocksGetter)
        {
            _testBlocksGetter = testBlocksGetter;
        }

        public async Task<AllTestBlocksDTO> GetData()
        {
            AllTestBlocksDTO allTestBlocks;

            try
            {
                var allTestBlocksModelTask = _testBlocksGetter.GetTestBlocks();

                var mapper = new MapperConfiguration(cfg => cfg.CreateMap<AllTestBlocksModel, AllTestBlocksDTO>()
                    .ForMember(dest => dest.TestBlocksList, exp => exp.MapFrom(src => MapTestBlocks(src.TestBlocksList)))
                ).CreateMapper();

                var allTestBlocksModel = await allTestBlocksModelTask;
                allTestBlocks = mapper.Map<AllTestBlocksModel, AllTestBlocksDTO>(allTestBlocksModel);

            }
            catch (Exception)
            {
                throw;
            }

            return allTestBlocks;
        }

        #region Private Methods
        private List<TestBlockDTO> MapTestBlocks(List<TestBlock> testBlocksList)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<TestBlock, TestBlockDTO>()
                .ForMember(dst => dst.AllCandidates, e => e.MapFrom(src => MapAllCandidates(src.AllCandidatesModel)))
                .ForMember(dst => dst.AllBallotPapers, e => e.MapFrom(src => MapAllBallotPapers(src.AllBallotPapersModel)))
            ).CreateMapper();
            return mapper.Map<List<TestBlock>, List<TestBlockDTO>>(testBlocksList);
        }

        private AllCandidatesDTO MapAllCandidates(AllCandidatesModel allCandidatesModel)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<AllCandidatesModel, AllCandidatesDTO>()
                .ForMember(d => d.CandidatesList, e => e.MapFrom(s => MapCandidatesList(s.CandidatesList)))
            ).CreateMapper();
            return mapper.Map<AllCandidatesModel, AllCandidatesDTO>(allCandidatesModel);
        }

        private List<CandidateDTO> MapCandidatesList(List<CandidateModel> candidatesList)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<CandidateModel, CandidateDTO>()).CreateMapper();
            return mapper.Map<List<CandidateModel>, List<CandidateDTO>>(candidatesList);
        }

        private AllBallotPapersDTO MapAllBallotPapers(AllBallotPapersModel allBallotPapersModel)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<AllBallotPapersModel, AllBallotPapersDTO>()
                .ForMember(d => d.BallotPapersList, e => e.MapFrom(s => MapBallotPapersList(s.BallotPapersList)))
            ).CreateMapper();

            return mapper.Map<AllBallotPapersModel, AllBallotPapersDTO>(allBallotPapersModel);
        }

        private List<BallotPaperDTO> MapBallotPapersList(List<BallotPaperModel> ballotPapersList)
        {
            var mapper = new MapperConfiguration(c => c.CreateMap<BallotPaperModel, BallotPaperDTO>()
                .ForMember(d => d.Votes, e => e.MapFrom(s => s.Votes))
            ).CreateMapper();

            return mapper.Map<List<BallotPaperModel>, List<BallotPaperDTO>>(ballotPapersList);
        }
        #endregion
    }
}
