namespace Babadzaki_Domain.Mappings
{
   


    public class JsonToken
    {
        public int edition { get; set; }        
        public string description { get; set; }
        public string image { get; set; }
        public Attribute[] attributes { get; set; }
    }

    public class Attribute
    {
        public string trait_type { get; set; }
        public string value { get; set; }
    }

}
