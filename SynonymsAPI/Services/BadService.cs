using SynonymsDB;

namespace SynonymsAPI.Services
{
    public class BadService
    {
        public BadService()
        {

        }
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
                    if (word == "BA")
                    {

                    }
                    else if (word == "BB")
                    {

                    }
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
                    if (word == "BA")
                    {

                    }
                    else if (word == "BB")
                    {

                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Add(string word, string foo)
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

        public bool Add(string word, int foo)
        {
            try
            {
                if (word == "A")
                {
                    if (word == "A")
                    {

                    }
                    else if (word.Contains("B"))
                    {
                        if (word == "BA")
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
                        }
                        else if (word == "BB")
                        {

                        }
                    }
                    else if (word == "C")
                    {

                    }
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

        public bool Add(string word, List<string> synonyms)
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
                //Remove all empty strings, or synonyms that are equal to word, and get Distinct values
                synonyms.RemoveAll(s => string.IsNullOrWhiteSpace(s) || s == word);
                synonyms = synonyms.Distinct().ToList();

                if (word == null || synonyms.Count == 0) return false;

                //If the word that is a synonym already exists, get the ID. If not, add it


                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
