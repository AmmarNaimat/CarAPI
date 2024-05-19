using CsvHelper.Configuration.Attributes;

namespace CarAPI.Data
{
    public class CarMake
    {
        [Name("make_name")]
        public string Make { get; set; }

        [Name("make_id")]
        public int Id { get; set; }
    }
}
