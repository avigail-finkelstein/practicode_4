
import axios from 'axios';

axios.defaults.baseURL = "http://localhost:5240";

axios.interceptors.response.use(
  (response) => response,
  (error) => Promise.reject(error)
);

export default {
  getTasks: async () => {
    const result = await axios.get(`/items`);
    console.log(result);
    return result.data;
  },

  addTask: async (name) => {
    const result = await axios.put(`/addItems`, { name: name, isComplete: false });
    return result.data;
  },

  setCompleted: async (id, isComplete) => {
    console.log('setCompleted', { id, isComplete });
    const result = await axios.post(`/updateItems/${id}`, { isComplete });
    return result.data;
  },

  deleteTask: async (id) => {
    console.log('deleteTask');
    const result = await axios.delete(`/DeleteItems/${id}`);
    return result.data;
  },
};
