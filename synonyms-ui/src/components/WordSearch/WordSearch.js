import React, { useEffect, useState } from "react";
import { InputGroup, Form, Table, Spinner } from "react-bootstrap";
import "./wordSearch.css";
import { debounce } from "lodash";
import { getWordSynonyms } from "../../services/synonymsApiService";
import Loading from "../Shared/Loading/Loading";

function WordSearch(props) {
  const [searchTerm, setSearchTerm] = useState("");
  const [wordList, setWordList] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  useEffect(() => {
    if (searchTerm !== "") fetchData();
  }, [searchTerm]);

  const fetchData = async () => {
    setIsLoading(true);
    try {
      let words = await getWordSynonyms(searchTerm);
      setWordList(words);
    } catch {
      alert("Something went wrong.");
    } finally {
      setIsLoading(false);
    }
  };

  const handleChange = debounce((e) => {
    setSearchTerm(e.target.value);
  }, 500);

  const getTableWithData = () => {
    return (
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
    );
  };
  return (
    <>
      <p>Find synonyms for word:</p>
      <InputGroup className="mb-3">
        <Form.Control
          placeholder="Type in the word"
          aria-label="word"
          aria-describedby="basic-addon1"
          onChange={handleChange}
        />
      </InputGroup>
      {isLoading && <Loading />}
      {!isLoading && wordList.length > 0 && <>{getTableWithData()}</>}
      {!isLoading && searchTerm && wordList.length === 0 && (
        <div className="no-items">
          <p>Sorry, we found no synonyms for "{searchTerm}"</p>
        </div>
      )}
    </>
  );
}

export default WordSearch;
