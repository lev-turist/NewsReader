using NewsReader.Data;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsReader.Services
{
    public class NewsReaderServiceRSS : INewsReaderService
    {
        private readonly ApplicationDbContext _db;
        private SyndicationFeed _feed;
        private String _source;
        private int _newNewsCount = 0;

        public NewsReaderServiceRSS(ApplicationDbContext db)
        {
            _db = db;
        }

        public int Read(string rss)
        {
            _source = rss;
            load();
            saveToDb();
            return _newNewsCount;
        }

        private void load()
        {
            XmlReader reader = XmlReader.Create(_source);
            _feed = SyndicationFeed.Load(reader);
        }

        private void saveToDb()
        {
            foreach (SyndicationItem item in _feed.Items)
            {
                if (!_db.News.Any(n => n.PublishDate == item.PublishDate.DateTime && n.Title == item.Title.Text))
                {
                    ++_newNewsCount;
                    _db.News.Add(new News() { PublishDate = item.PublishDate.DateTime, Title = item.Title.Text, Summary = ((TextSyndicationContent)item.Summary).Text, Source = _source });
                }
            }
            _db.SaveChanges();

        }
    }
}
