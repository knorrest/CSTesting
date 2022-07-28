import React, { useState } from "react";
import "./wordSearch.css";
import { debounce } from "lodash";

import { getWordSynonyms } from "../../services/synonymsApiService";
import Loading from "../Shared/Loading";
import TextField from "../Form/textField";
import TableWithSynonyms from "../Shared/TableWithSynonyms/TableWithSynonyms";

function WordSearch() {
  const [searchTerm, setSearchTerm] = useState("");
  const [wordList, setWordList] = useState([]);
  const [isLoading, setIsLoading] = useState(false);

  const fetchData = async (searchTerm) => {
    if (searchTerm === "") return;
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
  return (
    <>
      <div className="search-header">
        <p>Find synonyms for word:</p>
        <TextField
          className="search-field"
          type="text"
          id="word"
          name="word"
          value={searchTerm}
          onChange={handleChange}
        />
      </div>
      {isLoading && <Loading />}
      {!isLoading && searchTerm && wordList.length > 0 && (
        <TableWithSynonyms wordList={wordList} />
      )}
      {!isLoading && searchTerm && wordList.length === 0 && (
        <div className="no-items">
          <p>Sorry, we found no synonyms for "{searchTerm}"</p>
        </div>
      )}
    </>
  );
}

export default WordSearch;
