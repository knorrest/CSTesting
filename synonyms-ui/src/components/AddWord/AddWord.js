import React, { useState } from "react";
import { Button, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { addSynonym } from "../../services/synonymsApiService";
import Loading from "../Shared/Loading/Loading";
import "./addWord.css";

function AddWord() {
  const [word, setWord] = useState("");
  const [inputList, setInputList] = useState([""]);
  const [isLoading, setIsLoading] = useState(false);

  let navigate = useNavigate();

  function addNewSynonym() {
    let newInputList = [...inputList, ""];
    setInputList(newInputList);
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setIsLoading(true);
    try {
      var result = await addSynonym(word, inputList);
      if (result) navigate("/all");
      else alert("Something went wrong");
    } catch {
      alert("Something went wrong.");
    } finally {
      setIsLoading(false);
    }
  }

  function handleKeyDown(e) {
    if (e.key === " ") {
      e.preventDefault();
    }
  }

  function handleChange(index, event) {
    let data = [...inputList];
    data[index] = event.target.value;
    setInputList(data);
  }

  return (
    <>
      <p>
        Add a new word with synonyms. You can easily add multiple synonyms for
        the same word. If you add existing word, synonyms will merge with the
        existing one. Type one synonym in each box. Words are currently case
        sensitive.
      </p>
      {isLoading && <Loading />}
      {!isLoading && (
        <div className="add-word-form">
          <Form onSubmit={handleSubmit}>
            <Form.Group className="mb-3" controlId="formWord">
              <Form.Label>Word:</Form.Label>
              <Form.Control
                type="text"
                value={word}
                onChange={(e) => setWord(e.target.value)}
              ></Form.Control>
            </Form.Group>
            <Form.Group className="mb-3" controlId="formsynonyms">
              <Form.Label>Synonyms:</Form.Label>
              {inputList.map((input, index) => (
                <div key={index}>
                  <Form.Control
                    type="text"
                    value={input}
                    onKeyDown={handleKeyDown}
                    onChange={(e) => handleChange(index, e)}
                  ></Form.Control>
                  <br />
                </div>
              ))}
              <div className="add-synonym-button-wrapper">
                <Button variant="light" type="button" onClick={addNewSynonym}>
                  + Add new synonym
                </Button>
              </div>
            </Form.Group>
            <hr></hr>
            <div className="add-word-button-wrapper">
              <Button variant="primary" type="submit">
                Add Word
              </Button>
            </div>
          </Form>
        </div>
      )}
    </>
  );
}

export default AddWord;
