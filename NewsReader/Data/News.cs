namespace NewsReader.Data
{
    public class News
    {
        public int Id { get; set; }
        public DateTime PublishDate { get; set; }
        public string Source { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
    }
}
