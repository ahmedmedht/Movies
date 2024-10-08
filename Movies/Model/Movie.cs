namespace Movies.Model
{
    public class Movie
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Title { get; set; }
        
        public int Year {  get; set; }

        public double Rate { get; set; }

        [MaxLength(2600)]
        public string StoreLine { get; set; } 

        public Guid? ImageGuid { get; set; }

        public ImageInfo? ImageInfo {  get; set; }

        public byte GenreId { get; set; }  

        public Genre Genre { get; set; }
    }
}
