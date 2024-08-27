import axios from "axios";

export interface BookRequest {
  title: string;
  description: string;
  price: number;
}
export const getAllBooks = async () => {
  const response = await axios.get("https://localhost:7056/api/Books");
  try {
    return response.data;
  } catch (e) {
    console.error(e);
  }
};

export const createBook = async (bookRequest: BookRequest) => {
  try {
    await axios.post("https://localhost:7056/api/Books", bookRequest);
  } catch (e) {
    console.error(e);
  }
};

export const updateBook = async (id: string, bookRequest: BookRequest) => {
  try {
    await axios.put(`https://localhost:7056/api/Books/${id}`, bookRequest);
  } catch (e) {
    console.error(e);
  }
};

export const deleteBook = async (id: string) => {
  try {
    await axios.delete(`https://localhost:7056/api/Books/${id}`);
  } catch (e) {
    console.error(e);
  }
};
