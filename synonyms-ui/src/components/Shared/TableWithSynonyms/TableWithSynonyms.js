import "./tableWithSynonyms.css";
import { Table } from "react-bootstrap";
import SynonymBubble from "../SynonymBubble";

function TableWithSynonyms(props) {
  const getSynonyms = (synonyms) => {
    return (
      <div className="table-synonyms">
        {synonyms.map((synonym) => (
          <SynonymBubble synonym={synonym} />
        ))}
      </div>
    );
  };

  return (
    <Table striped bordered hover>
      <thead>
        <tr>
          <th>Word</th>
          <th>Synonyms</th>
        </tr>
      </thead>
      <tbody>
        {props.wordList.map((word) => (
          <tr key={word.id}>
            <td>
              <b>{word.wordString}</b>
            </td>
            <td>{getSynonyms(word.synonyms)}</td>
          </tr>
        ))}
      </tbody>
    </Table>
  );
}

export default TableWithSynonyms;
