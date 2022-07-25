import "./App.css";
import "bootstrap/dist/css/bootstrap.min.css";
import { BrowserRouter, Route, Routes, Navigate } from "react-router-dom";
import WordList from "./components/WordList/WordList";
import AddWord from "./components/AddWord/AddWord";
import WordSearch from "./components/WordSearch";
import { Container, Nav, Navbar } from "react-bootstrap";

function App() {
  return (
    <>
      <Navbar bg="light" expand="lg">
        <Container>
          <Navbar.Brand href="/">Words with synonyms</Navbar.Brand>
          <Navbar.Toggle aria-controls="basic-navbar-nav" />
          <Nav className="me-auto">
            <Nav.Link href="/">Search synonyms</Nav.Link>
            <Nav.Link href="/all">View all words</Nav.Link>
            <Nav.Link href="/add-word">Add new</Nav.Link>
          </Nav>
        </Container>
      </Navbar>
      <BrowserRouter>
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
