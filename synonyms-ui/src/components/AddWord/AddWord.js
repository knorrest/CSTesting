import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./addWord.css";

import { Button } from "react-bootstrap";
import Loading from "../Shared/Loading";

import { addSynonym } from "../../services/synonymsApiService";
import { Formik, Form, Field, FieldArray } from "formik";
import TextField from "../Form/textField";

function AddWord() {
  const [isLoading, setIsLoading] = useState(false);

  let navigate = useNavigate();

  async function handleSubmit(values) {
    setIsLoading(true);
    try {
      var result = await addSynonym(values.word, values.synonyms);
      if (result) navigate("/all");
      else alert("Something went wrong");
    } catch {
      alert("Something went wrong.");
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <>
      {isLoading && <Loading />}
      {!isLoading && (
        <div className="add-word-form">
          <Formik
            onSubmit={handleSubmit}
            initialValues={{
              word: "",
              synonyms: [""],
            }}
            validate={(values) => {
              let errors = {};
              if (values.synonyms.length === 0)
                errors.synonyms = "You must enter at least one synonym";

              if (values.synonyms.some((x) => x.length < 2))
                errors.synonyms =
                  "All synonyms must have at least two characters";

              if (
                values.word !== "" &&
                values.synonyms.some((x) => x === values.word)
              )
                errors.synonyms = "Synonym should not be the same as word";

              if (values.word.length < 2)
                errors.word = "Word must have at least two characters";
              return errors;
            }}
          >
            {({ values, errors, touched, handleBlur, handleChange }) => (
              <Form>
                <p>
                  Add a new word with synonyms. You can easily add multiple
                  synonyms for the same word. If you add existing word, synonyms
                  will merge with the existing one. Type one synonym in each
                  box. Words are case insensitive.
                </p>
                <div className="row">
                  <label>Word:</label>
                  <TextField
                    type="text"
                    id="word"
                    name="word"
                    onChange={handleChange}
                    onBlur={handleBlur}
                    value={values.word}
                  />
                  <div className="validation-error">
                    {errors.word && touched.word && errors.word}
                  </div>
                </div>
                <label>Synonyms:</label>
                <FieldArray
                  name="synonyms"
                  render={({ push, remove }) => (
                    <div>
                      {values.synonyms.map((input, index) => (
                        <div className="synonyms-wrapper" key={index}>
                          <div className="input-synonyms">
                            <TextField
                              type="text"
                              id={`synonyms[${index}]`}
                              name={`synonyms[${index}]`}
                              onChange={handleChange}
                              onBlur={handleBlur}
                              value={values.synonyms[index]}
                            />
                          </div>
                          <div className="button-synonyms">
                            <Button
                              variant="danger"
                              type="button"
                              onClick={() => remove(index)}
                            >
                              Remove
                            </Button>
                          </div>
                          <br />
                        </div>
                      ))}
                      <div className="validation-error">
                        {errors.synonyms && touched.synonyms && errors.synonyms}
                      </div>
                      <div className="add-synonym-button-wrapper">
                        <Button
                          variant="light"
                          type="button"
                          onClick={() => push("")}
                        >
                          + Add new synonym
                        </Button>
                      </div>
                    </div>
                  )}
                ></FieldArray>
                <hr></hr>
                <div className="add-word-button-wrapper">
                  <Button variant="primary" type="submit">
                    Add Word
                  </Button>
                </div>
              </Form>
            )}
          </Formik>
        </div>
      )}
    </>
  );
}

export default AddWord;
