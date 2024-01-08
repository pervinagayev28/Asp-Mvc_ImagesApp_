namespace WebApplication1.Models
{
    public class Image : IImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool State { get; set; }
    }
}
