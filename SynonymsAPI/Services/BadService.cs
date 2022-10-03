namespace SynonymsAPI.Services
{
    public class BadService
    {
        public bool Add(string word)
        {
            try
            {
                if (word == "A")
                {

                }
                else if (word.Contains("B"))
                {
                    if (word == "BA")
                    {

                    }
                    else if (word == "BB")
                    {

                    }
                }
                else if (word == "C")
                {

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Add(string word, bool foo)
        {
            try
            {
                if (word == "A")
                {

                }
                else if (word.Contains("B"))
                {
                    if (word == "BA")
                    {

                    }
                    else if (word == "BB")
                    {

                    }
                }
                else if (word == "C")
                {

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
