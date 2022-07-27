import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { getAllWords } from "../../services/synonymsApiService";
import Loading from "../Shared/Loading";
import "./wordList.css";

function WordList() {
  const [wordList, setWordList] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

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

  useEffect(() => {
    getWords();
  }, []);

  const getTableWithData = () => {
    return (
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Word</th>
            <th>Synonyms</th>
          </tr>
        </thead>
        {isLoading && <Loading />}
        <tbody>
          {wordList.map((word) => (
            <tr key={word.id}>
              <td>{word.wordString}</td>
              <td>{word.synonyms.toString()}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    );
  };
  return (
    <>
      <p>Here is a list of existing words and their synonyms:</p>
      {isLoading && <Loading />}
      {!isLoading && getTableWithData()}
    </>
  );
}

export default WordList;
