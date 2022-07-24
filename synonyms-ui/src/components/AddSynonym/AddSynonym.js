import React, { Component } from "react";
import { addSynonym } from "../../services/synonymsApiService";
import "./addSynonym.css";

class AddSynonym extends Component {
  constructor(props) {
    super(props);
    this.state = { word: "", synonym: "" };

    this.handleWordChange = this.handleWordChange.bind(this);
    this.handleSynonymChange = this.handleSynonymChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleWordChange(e) {
    this.setState({ word: e.target.value });
  }

  handleSynonymChange(e) {
    this.setState({ synonym: e.target.value });
  }

  handleSubmit(e) {
    e.preventDefault();
    addSynonym(this.state.word, this.state.synonym);
  }

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
          <label>
            Synonym:
            <input
              type="text"
              name="synonym"
              value={this.state.synonym}
              onChange={this.handleSynonymChange}
            ></input>
          </label>

          <input type="submit" value="Add Synonym"></input>
        </form>
      </>
    );
  }
}

export default AddSynonym;
