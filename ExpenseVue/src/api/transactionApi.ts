import type { Transaction } from '../types/transaction';

const API_URL = '/api';

export const transactionApi = {
  async getTransactions(): Promise<Transaction[]> {
    const response = await fetch(`${API_URL}/transactions`);
    if (!response.ok) {
      throw new Error('Failed to fetch transactions');
    }
    return response.json();
  },

  async addTransaction(
    transaction: Omit<Transaction, 'id'>
  ): Promise<Transaction> {
    const response = await fetch(`${API_URL}/transactions`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(transaction),
    });
    if (!response.ok) {
      throw new Error('Failed to add transaction');
    }
    return response.json();
  },

  async deleteTransaction(id: string): Promise<void> {
    const response = await fetch(`${API_URL}/transactions/${id}`, {
      method: 'DELETE',
    });
    if (!response.ok) {
      throw new Error('Failed to delete transaction');
    }
  },
};
