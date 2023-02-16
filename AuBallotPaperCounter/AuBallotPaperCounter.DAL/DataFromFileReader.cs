using AuBallotPaperCounter.DAL.Interfaces;

namespace AuBallotPaperCounter.DAL
{
    public class DataFromFileReader : IDataReader
    {
        readonly string _filePath;
        StreamReader _streamReader;
        List<string> _textFromFile = new List<string>();

        public DataFromFileReader(string filePath)
        {
            _filePath = filePath;
        }

        public List<string> ReadData()
        {
            try
            {
                _streamReader = new StreamReader(_filePath);
            }
            catch (Exception)
            {
                Console.WriteLine("Файл не найден!");
            }

            _textFromFile = _streamReader.ReadToEnd().Split("\r\n").ToList();
            _streamReader.Dispose();

            return _textFromFile;
        }
    }
}
