namespace SynonymsDB
{
    public class WordSynonym
    {
        public long WordId { get; set; }
        public Word Word { get; set; }
        public long SynonymId { get; set; }
        public Synonym Synonym { get; set; }
    }
}