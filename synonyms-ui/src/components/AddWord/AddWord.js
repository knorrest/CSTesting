import React, { useState } from "react";
import { Button, Form } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import { addSynonym } from "../../services/synonymsApiService";
import "./addWord.css";

function AddWord() {
  const [word, setWord] = useState("");
  const [inputList, setInputList] = useState(["", ""]);
  let navigate = useNavigate();

  function addNewSynonym() {
    let newInputList = [...inputList, ""];
    setInputList(newInputList);
  }

  function handleSubmit(e) {
    e.preventDefault();
    addSynonym(word, inputList);
    navigate("/");
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
      <div className="add-word-form">
        <Form onSubmit={handleSubmit}>
          <Form.Group className="mb-3" controlId="formWord">
            <Form.Label>Word:</Form.Label>
            <Form.Control
              required
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
                  required
                  type="text"
                  value={input}
                  onKeyDown={handleKeyDown}
                  onChange={(e) => handleChange(index, e)}
                ></Form.Control>
                <br />
              </div>
            ))}
            <Button variant="light" type="button" onClick={addNewSynonym}>
              + Add new synonym
            </Button>
          </Form.Group>
          <hr></hr>
          <Button variant="primary" type="submit">
            Add Word
          </Button>
        </Form>
      </div>
    </>
  );
}

export default AddWord;
