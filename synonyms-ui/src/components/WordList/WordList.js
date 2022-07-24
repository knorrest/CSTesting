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

  function addSynonym(word) {
    let path = `/add-synonym?word=${word}`;
    navigate(path);
  }

  function addNewWord(word) {
    let path = `/add-word`;
    navigate(path);
  }

  return (
    <>
      <Button variant="primary" onClick={() => addNewWord()}>
        Add new Word
      </Button>
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
              <td>
                <Button
                  variant="primary"
                  onClick={() => addSynonym(word.wordString)}
                >
                  Add Synonym
                </Button>
              </td>
            </tr>
          ))}
        </tbody>
      </Table>
    </>
  );
}

export default WordList;
