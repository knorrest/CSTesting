namespace SynonymsDB
{
    public class Word
    {
        public Word()
        {
            WordSynonyms = new HashSet<WordSynonym>();
        }
        public long Id { get; set; }
        public string WordString { get; set; }
        public virtual ICollection<WordSynonym> WordSynonyms { get; set; }
    }
}