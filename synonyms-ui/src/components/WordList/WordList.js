import React, { useEffect, useState } from "react";
import { getAllWords } from "../../services/synonymsApiService";
import Loading from "../Shared/Loading";
import TableWithSynonyms from "../Shared/TableWithSynonyms/TableWithSynonyms";
import "./wordList.css";

function WordList() {
  const [wordList, setWordList] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    getWords();
  }, []);

  async function getWords() {
    setIsLoading(true);
    try {
      let words = await getAllWords();
      setWordList(words);
    } catch {
      alert("Something went wrong.");
    } finally {
      setIsLoading(false);
    }
  }
  return (
    <>
      <p>Here is a list of existing words and their synonyms:</p>
      {isLoading && <Loading />}
      {!isLoading && <TableWithSynonyms wordList={wordList} />}
    </>
  );
}

export default WordList;
