import React, { Component } from "react";
import "./addSynonym.css";

class AddSynonym extends Component {
  constructor(props) {
    super(props);
    this.state = { word: "", synonym: "" };

    this.handleSynonymChange = this.handleSynonymChange.bind(this);
    this.handleAgeChange = this.handleAgeChange.bind(this);
    this.handleSubmit = this.handleSubmit.bind(this);
  }

  handleSynonymChange(e) {
    this.setState({ synonym: e.target.value });
  }

  handleSubmit(e) {
    e.preventDefault();
  }

  render() {
    return (
      <>
        <form onSubmit={this.handleSubmit}>
          <label>Word:</label>
          <label>
            Synonym:
            <input
              type="text"
              name="synonym"
              value={this.state.surname}
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
