import React, { useEffect, useState } from "react";
import { InputGroup, Form, Table } from "react-bootstrap";
import "./wordSearch.css";
import { debounce } from "lodash";
import { getWordSynonyms } from "../../services/synonymsApiService";

function WordSearch(props) {
  const [searchTerm, setSearchTerm] = useState("");
  const [wordList, setWordList] = useState([]);

  useEffect(() => {
    if (searchTerm !== "") fetchData();
  }, [searchTerm]);

  const fetchData = async () => {
    let words = await getWordSynonyms(searchTerm);
    setWordList(words);
  };

  const handleChange = debounce((e) => {
    setSearchTerm(e.target.value);
  }, 500);

  return (
    <>
      <p>Find synonyms for word:</p>
      <InputGroup className="mb-3">
        <Form.Control
          placeholder="Search word"
          aria-label="word"
          aria-describedby="basic-addon1"
          onChange={handleChange}
        />
      </InputGroup>

      {wordList.length > 0 && (
        <>
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
      )}
    </>
  );
}

export default WordSearch;
