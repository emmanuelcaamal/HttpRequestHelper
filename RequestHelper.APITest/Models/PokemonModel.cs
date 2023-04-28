namespace RequestHelper.APITest.Models
{
    public class PokemonResponseModel
    {
        public int Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
        public List<PokemonModel> Results { get; set; }
    }


    public class PokemonModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
