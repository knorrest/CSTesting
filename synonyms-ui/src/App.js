import "./App.css";
import { BrowserRouter, Route, Routes, Navigate } from "react-router-dom";
import WordList from "./components/WordList/WordList";
import AddSynonym from "./components/AddSynonym/AddSynonym";

function App() {
  return (
    <BrowserRouter>
      <div id="main-wrapper" className="main-wrapper">
        <Routes>
          <Route path="/" element={<WordList />}></Route>
          <Route path="/add-word" element={<AddSynonym />}></Route>
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;
