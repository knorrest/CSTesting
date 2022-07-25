import React, { useEffect, useState } from "react";
import { Table } from "react-bootstrap";
import { getAllWords } from "../../services/synonymsApiService";
import "./wordList.css";

function WordList() {
  const [wordList, setWordList] = useState([]);

  async function getWords() {
    let words = await getAllWords();
    setWordList(words);
  }

  useEffect(() => {
    getWords();
  }, []);

  return (
    <>
      <p>Here is a list of existing words and their synonyms.</p>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Word</th>
            <th>Synonyms</th>
          </tr>
        </thead>
        <tbody>
          {wordList.map((word) => (
            <tr key={word.id}>
              <td>{word.wordString}</td>
              <td>{word.synonyms.toString()}</td>
            </tr>
          ))}
        </tbody>
      </Table>
    </>
  );
}

export default WordList;
