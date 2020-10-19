namespace HangFire.Service.Interfaces
{
    public interface IScrapingJobService
    {
        void StartScraping(string urlToScraping);
        void ScanUrlsWithQueue();
        void ScanUrls();
    }
}