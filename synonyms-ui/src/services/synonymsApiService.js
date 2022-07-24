import axios from "axios";

export const getCities = async () => {
  return axios.get(`${API_URL}/cities`).then((res) => {
    const cities = res.data.data;
    return mapToKeyValuePair(cities);
  });
};
