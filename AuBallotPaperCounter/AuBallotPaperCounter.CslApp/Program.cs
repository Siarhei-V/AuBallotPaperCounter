using AuBallotPaperCounter.BLL;
using AuBallotPaperCounter.BLL.Interfaces;
using AuBallotPaperCounter.BLL.Models;
using AuBallotPaperCounter.CslApp.Interfaces;
using AuBallotPaperCounter.DAL;
using AuBallotPaperCounter.DAL.Interfaces;
using AuBallotPaperCounter.DAL.Models;
using AuBallotPaperCounter.DAL.Services;
using AuBallotPaperCounter.UseCases;
using AuBallotPaperCounter.UseCases.DataValidation;
using AuBallotPaperCounter.UseCases.DTOs;
using AuBallotPaperCounter.UseCases.Factories;
using AuBallotPaperCounter.UseCases.Interfaces;
using AuBallotPaperCounter.UseCases.Models;
using Ninject;

namespace AuBallotPaperCounter.CslApp
{
    class Program
    {
        static IKernel _kernel;
        static string filePath;

        static async Task Main(string[] args)
        {
            filePath = args.FirstOrDefault();

            InjectDependencies();

            await _kernel.Get<IController>().StartAllocation();

            Console.WriteLine("\nДля выхода из приложения нажмите клавишу 'Ввод'");
            Console.ReadLine();
        }

        static void InjectDependencies()
        {
            _kernel = new StandardKernel();

            _kernel.Bind<BallotPaper>().ToSelf().InTransientScope();
            _kernel.Bind<AllCandidates>().ToSelf().InTransientScope();
            _kernel.Bind<Candidate>().ToSelf().InTransientScope();
            _kernel.Bind<ResultsModel>().ToSelf().InTransientScope();
            _kernel.Bind<AllTestBlocksModel>().ToSelf().InTransientScope();

            _kernel.Bind<IController>().To<Controller>().InSingletonScope();
            _kernel.Bind<IInteractor>().To<Interactor>().InTransientScope();

            _kernel.Bind<IInteractorOutputPort>().To<Presenter>().InSingletonScope();

            _kernel.Bind<IDataService<AllTestBlocksDTO>>().To<TestBlockService>().InTransientScope();
            _kernel.Bind<ICandidateAllocatorFactory>().To<CandidateAllocatorFactory>().InThreadScope();
            _kernel.Bind<IDataValidator<AllTestBlocksDTO>>().To<InputDataValidator>().InThreadScope();

            _kernel.Bind<ITestBlocksGetter>().To<BallotPaperParser>().InTransientScope();
            _kernel.Bind<IDataReader>().To<DataFromFileReader>().InTransientScope().WithConstructorArgument(filePath);
        }
    }
}

