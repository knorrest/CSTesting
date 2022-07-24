import React, { Component } from "react";
import { addSynonym } from "../../services/synonymsApiService";
import "./addWord.css";

class AddWord extends Component {
  constructor(props) {
    super(props);
    this.state = { word: "", inputList: [""] };

    this.handleWordChange = this.handleWordChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleWordChange = (e) => {
    this.setState({ word: e.target.value });
  };

  addNewSynonym = () => {
    let newInputList = [...this.state.inputList, ""];
    this.setState({ inputList: newInputList });
  };

  handleSubmit = (e) => {
    e.preventDefault();
    addSynonym(this.state.word, this.state.inputList);
  };

  handleChange = (index, event) => {
    let data = [...this.state.inputList];
    data[index] = event.target.value;
    this.setState({ inputList: data });
  };

  render() {
    return (
      <>
        <form onSubmit={this.handleSubmit}>
          <label>
            Word:
            <input
              type="text"
              name="word"
              value={this.state.word}
              onChange={this.handleWordChange}
            ></input>
          </label>
          <br />
          <br />
          <label>
            Synonyms:
            {this.state.inputList.map((input, index) => (
              <div key={index}>
                <input
                  type="text"
                  name="synonym"
                  value={input}
                  onChange={(e) => this.handleChange(index, e)}
                ></input>
                <br />
              </div>
            ))}
            <button type="button" onClick={this.addNewSynonym}>
              Add new synonym
            </button>
          </label>
          <button type="submit" onClick={this.addWord}>
            Add Word
          </button>
        </form>
      </>
    );
  }
}

export default AddWord;
