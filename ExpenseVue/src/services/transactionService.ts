import type { Transaction } from "../types/transaction";
import api from "../api";

/**
 * Service for handling transaction operations
 */
export const transactionService = {
  /**
   * Fetches all transactions
   * @returns {Promise<Transaction[]>} List of transactions
   */
  async getTransactions(): Promise<Transaction[]> {
    const response = await api.get("/transactions");
    return response.data;
  },

  /**
   * Creates a new transaction
   * @param {Transaction} transaction - The transaction to create
   * @returns {Promise<Transaction>} The created transaction
   */
  async createTransaction(transaction: Transaction): Promise<Transaction> {
    const response = await api.post("/transactions", transaction);
    return response.data;
  },

  /**
   * Updates an existing transaction
   * @param {number} id - The ID of the transaction to update
   * @param {Transaction} transaction - The updated transaction data
   * @returns {Promise<Transaction>} The updated transaction
   */
  async updateTransaction(
    id: number,
    transaction: Transaction
  ): Promise<Transaction> {
    const response = await api.put(`/transactions/${id}`, transaction);
    return response.data;
  },

  /**
   * Deletes a transaction
   * @param {number} id - The ID of the transaction to delete
   * @returns {Promise<void>}
   */
  async deleteTransaction(id: number): Promise<void> {
    await api.delete(`/transactions/${id}`);
  },
};
