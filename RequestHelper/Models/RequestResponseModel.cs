namespace RequestHelper.Models
{
    public class RequestResponseModel<T> where T : class
    {
        public int Status { get; set; }
        public string ReasonPhrase { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
}
