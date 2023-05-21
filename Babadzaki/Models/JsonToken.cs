namespace Babadzaki.Models
{
   


    public class JsonToken
    {
        //[Newtonsoft.Json.JsonProperty("")]
        public string dna { get; set; }
        public string name { get; set; }
        public string season_collection { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public int edition { get; set; }
        public long date { get; set; }
        public Attribute[] attributes { get; set; }
    }

    public class Attribute
    {
        public string trait_type { get; set; }
        public string value { get; set; }
    }

}
