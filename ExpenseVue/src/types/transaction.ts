/**
 * Represents a transaction in the system
 */
export interface Transaction {
  id: number;
  amount: number;
  description: string;
  category: string;
  date: string;
  type: TransactionType;
}

export enum TransactionType {
  Income = 0,
  Expense = 1,
}

/**
 * Filter type for transactions
 */
export type FilterType = "all" | "day" | "month" | "year";

/**
 * Formatted transaction with proper types
 */
export interface FormattedTransaction extends Transaction {
  amount: number;
  type: TransactionType;
  date: string;
}
