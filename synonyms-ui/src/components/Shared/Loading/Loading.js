import Spinner from "react-bootstrap/Spinner";
import "./loading.css";

function Loading() {
  return (
    <div className="custom-spinner">
      <Spinner animation="border" />
    </div>
  );
}

export default Loading;
