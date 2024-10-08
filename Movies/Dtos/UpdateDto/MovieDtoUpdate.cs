namespace Movies.Dtos.UpdateDto
{
    public class MovieDtoUpdate
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string? Title { get; set; }

        public int Year { get; set; } = -1;

        public double Rate { get; set; } = -1;

        [MaxLength(2600)]
        public string? StoreLine { get; set; }

        public IFormFile? Poster { get; set; }


        public byte GenreId { get; set; } = 0;

    }
}
