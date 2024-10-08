namespace Movies.Dtos.ShowData
{
    public class MovieDtoShow
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int Year { get; set; }

        public double Rate { get; set; }

        public string StoreLine { get; set; }

        public byte[]? ImageData { get; set; }  // Replacing ImageGuid and ImageInfo with Byte[]

        public string GenreName { get; set; }
    }
}
