namespace SynonymsDB
{
    public class Word
    {
        public Word()
        {
            Synonyms = new HashSet<Synonym>();
        }
        public long Id { get; set; }
        public string WordString { get; set; }
        public virtual ICollection<Synonym> Synonyms { get; set; }
    }
}