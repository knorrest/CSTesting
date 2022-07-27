import React, { useState } from "react";
import "./wordSearch.css";
import { debounce } from "lodash";
import { Table } from "react-bootstrap";

import { getWordSynonyms } from "../../services/synonymsApiService";
import Loading from "../Shared/Loading";
import TextField from "../Form/textField";

function WordSearch() {
  const [searchTerm, setSearchTerm] = useState("");
  const [wordList, setWordList] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  const fetchData = async (searchTerm) => {
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

  const debounceFn = React.useMemo(() => debounce(fetchData, 500), []);

  function handleChange(event) {
    setSearchTerm(event.target.value);
    debounceFn(event.target.value);
  }

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
      <TextField
        type="text"
        id="word"
        name="word"
        value={searchTerm}
        onChange={handleChange}
      />
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
