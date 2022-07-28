import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";

import { BrowserRouter, Route, Routes, Navigate, Link } from "react-router-dom";
import { Container, Nav, Navbar } from "react-bootstrap";

import WordList from "./components/WordList";
import AddWord from "./components/AddWord";
import WordSearch from "./components/WordSearch";

function App() {
  return (
    <>
      <BrowserRouter>
        <Navbar bg="light">
          <Container>
            <Navbar.Brand href="/">Words with synonyms</Navbar.Brand>
            <Navbar.Toggle aria-controls="basic-navbar-nav" />
            <Nav className="me-auto">
              <Link className="nav-link" to="/">
                Search synonyms
              </Link>
              <Link className="nav-link" to="/all">
                View all words
              </Link>
              <Link className="nav-link" to="/add-word">
                Add new
              </Link>
            </Nav>
          </Container>
        </Navbar>
        <div id="main-wrapper" className="main-wrapper">
          <Container>
            <Routes>
              <Route path="/" element={<WordSearch />}></Route>
              <Route path="/all" element={<WordList />}></Route>
              <Route path="/add-word" element={<AddWord />}></Route>
              <Route path="*" element={<Navigate to="/" replace />} />
            </Routes>
          </Container>
        </div>
      </BrowserRouter>
    </>
  );
}

export default App;
