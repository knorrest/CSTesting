import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Table, Button } from "react-bootstrap";
import { getAllWords } from "../../services/synonymsApiService";
import "./wordList.css";

function WordList(props) {
  const [wordList, setWordList] = useState([]);
  let navigate = useNavigate();

  async function getWords() {
    let words = await getAllWords();
    setWordList(words);
  }

  useEffect(() => {
    getWords();
  }, []);
  function addNewWord() {
    let path = `/add-word`;
    navigate(path);
  }

  return (
    <>
      <h1>Words with synonyms</h1>
      <Button variant="primary" onClick={() => addNewWord()}>
        Add new Word
      </Button>

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
