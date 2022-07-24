namespace SynonymsAPI
{
    public class WordDto
    {
        public WordDto()
        {
            Synonyms = new List<string>();
        }
        public string WordString { get; set; }
        public virtual ICollection<string> Synonyms { get; set; }
    }
}
