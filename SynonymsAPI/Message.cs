namespace SynonymsAPI
{
    public class Message
    {
        public virtual object Data { get; set; }
        public virtual bool IsValid { get; set; }

        public Message()
        {
        }

        public Message(bool isValid, object? data = null)
        {
            IsValid = isValid;
            Data = data;
        }
    }
}
