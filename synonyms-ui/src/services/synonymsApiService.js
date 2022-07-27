import axios from "axios";
const API_URL = process.env.REACT_APP_API_URL;

export const getAllWords = async () => {
  return axios.get(`${API_URL}`).then((res) => {
    const words = res.data.data;
    return words;
  });
};

export const getWordSynonyms = async (word) => {
  return axios.get(`${API_URL}/search?word=${word}`).then((res) => {
    const words = res.data.data;
    return words;
  });
};

export const addSynonym = async (word, synonymList) => {
  return axios
    .post(`${API_URL}`, {
      WordString: word,
      Synonyms: synonymList,
    })
    .then((res) => {
      return res;
    });
};
