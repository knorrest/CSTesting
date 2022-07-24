import React, { Component } from "react";
import "./wordList.css";

class WordList extends Component {
  constructor(props) {
    super(props);
    this.state = { wordList: [] };
  }

  getWords() {}

  render() {
    return (
      <>
        <ul>
          <li></li>
        </ul>
      </>
    );
  }
}

export default WordList;
