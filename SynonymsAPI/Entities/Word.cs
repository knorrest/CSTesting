namespace SynonymsDB
{
    public class Word
    {
        public Word()
        {
            SynonymIds = new List<long>();
        }
        public long Id { get; set; }
        public string WordString { get; set; }
        public virtual ICollection<long> SynonymIds { get; set; }
    }
}