using Microsoft.AspNetCore.Mvc;
using NewsReader.Data;
using NewsReader.Services;
using System.ServiceModel.Syndication;
using System.Xml;

namespace NewsReader.Controller
{
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly INewsReaderService _newsReader;

        public NewsController(ApplicationDbContext context, INewsReaderService reader)
        {
            _newsReader = reader;
            _db = context;
        }

        /// <summary>
        /// Load all new news to db from rss-chanel.
        /// </summary>
        [HttpPost]
        [Route("news")]
        public ActionResult Post([FromBody]Source source)
        {
            int newNewsCount = _newsReader.Read(source.Rss);
            return new JsonResult(newNewsCount);
        }

        /// <summary>
        /// Get all news from db.
        /// </summary>
        [HttpGet]
        [Route("news")]
        public IEnumerable<News> Get()
        {
            return _db.News.OrderByDescending(n => n.PublishDate).ToList();
        }

        /// <summary>
        /// Get all news by title fragment from db.
        /// </summary>
        /// <param name="titleFragment"></param>
        [HttpGet]
        [Route("news/{titleFragment}")]
        public IEnumerable<News> Get(String titleFragment)
        {
            return _db.News.Where(n => n.Title.Contains(titleFragment)).OrderByDescending(n => n.PublishDate).ToList();
        }
    }
}