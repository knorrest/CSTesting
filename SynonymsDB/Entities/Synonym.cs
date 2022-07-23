namespace SynonymsDB
{
    public class Synonym
    {
        public Synonym()
        {
            WordSynonyms = new HashSet<WordSynonym>();
        }

        public long Id { get; set; }
        public string SynonymString { get; set; }
        public virtual ICollection<WordSynonym> WordSynonyms { get; set; }
    }
}