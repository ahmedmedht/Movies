 namespace Movies.Helper
{
    public class Response <T> where T : class
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
