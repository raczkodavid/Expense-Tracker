<template>
  <div class="flex min-h-screen flex-col bg-base-200">
    <main class="flex-grow p-6">
      <div
        class="mx-auto flex w-full max-w-7xl flex-col gap-8 lg:flex-row lg:items-stretch"
      >
        <!-- Left Column -->
        <div class="flex h-full flex-1 flex-col space-y-8 lg:w-2/3">
          <section class="grid grid-cols-1 gap-6 sm:grid-cols-3">
            <SummaryCard
              title="Total Expenses"
              :value="totalExpenses"
              color-class="text-error"
            />
            <SummaryCard
              title="Total Income"
              :value="totalIncome"
              color-class="text-success"
            />
            <SummaryCard
              title="Net Balance"
              :value="netBalance"
              :color-class="
                parseFloat(netBalance) >= 0 ? 'text-success' : 'text-error'
              "
            />
          </section>

          <section class="flex-grow">
            <TransactionTable
              :transactions="filteredTransactions"
              :filter-by="filterBy"
              @update:filter-by="filterBy = $event"
              @edit="openEditTransaction"
              @delete="deleteTransaction"
            />
          </section>
        </div>

        <!-- Right Column -->
        <div class="flex h-full w-full flex-col lg:w-1/3 lg:flex-shrink-0">
          <ChartContainer :transactions="filteredTransactions" />

          <section class="mt-8 text-center">
            <button
              class="btn btn-primary btn-lg w-full shadow-md"
              @click="openAddTransaction"
            >
              Add Transaction
            </button>
          </section>
        </div>
      </div>
    </main>

    <!-- Transaction Modal -->
    <TransactionModal
      v-if="editingTransaction"
      :transaction="editingTransaction"
      :is-editing="isEditing"
      @submit="submitTransaction"
      @close="closeEditModal"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, computed, onMounted } from 'vue';
import SummaryCard from './SummaryCard.vue';
import TransactionTable from './TransactionTable.vue';
import TransactionModal from './TransactionModal.vue';
import ChartContainer from './ChartContainer.vue';
import { transactionService } from '@/services/transactionService';
import { TransactionType } from '@/types/transaction';
import type {
  Transaction,
  FormattedTransaction,
  FilterType,
} from '@/types/transaction';

export default defineComponent({
  name: 'Dashboard',
  components: {
    SummaryCard,
    TransactionTable,
    TransactionModal,
    ChartContainer,
  },
  setup() {
    const transactions = ref<FormattedTransaction[]>([]);
    const editingTransaction = ref<FormattedTransaction | null>(null);
    const isEditing = ref(false);
    const filterBy = ref<FilterType>('all');

    const fetchTransactions = async () => {
      try {
        const data = await transactionService.getTransactions();
        transactions.value = data.map(formatTransaction);
      } catch (error) {
        console.error('Error fetching transactions:', error);
      }
    };

    const formatTransaction = (
      transaction: Transaction
    ): FormattedTransaction => ({
      ...transaction,
      date: transaction.date
        ? new Date(transaction.date).toISOString().split('T')[0]
        : new Date().toISOString().split('T')[0],
      amount: parseFloat(transaction.amount.toString()) || 0,
      type:
        parseInt(transaction.type.toString(), 10) === 0
          ? TransactionType.Income
          : TransactionType.Expense,
    });

    const filteredTransactions = computed(() => {
      const now = new Date();
      return transactions.value.filter((t) => {
        const tDate = new Date(t.date);
        switch (filterBy.value) {
          case 'day':
            return tDate.toDateString() === now.toDateString();
          case 'month':
            return (
              tDate.getMonth() === now.getMonth() &&
              tDate.getFullYear() === now.getFullYear()
            );
          case 'year':
            return tDate.getFullYear() === now.getFullYear();
          default:
            return true;
        }
      });
    });

    const totalExpenses = computed(() =>
      filteredTransactions.value
        .filter((t) => t.type === TransactionType.Expense)
        .reduce((sum, t) => sum + t.amount, 0)
        .toFixed(2)
    );

    const totalIncome = computed(() =>
      filteredTransactions.value
        .filter((t) => t.type === TransactionType.Income)
        .reduce((sum, t) => sum + t.amount, 0)
        .toFixed(2)
    );

    const netBalance = computed(() =>
      (parseFloat(totalIncome.value) - parseFloat(totalExpenses.value)).toFixed(
        2
      )
    );

    const openAddTransaction = () => {
      editingTransaction.value = {
        id: 0,
        amount: 0,
        description: '',
        category: '',
        date: new Date().toISOString().split('T')[0],
        type: TransactionType.Income, // Default to Income for Add
      };
      isEditing.value = false;
    };

    const openEditTransaction = (transaction: Transaction) => {
      editingTransaction.value = formatTransaction(transaction);
      isEditing.value = true;
    };

    const closeEditModal = () => {
      editingTransaction.value = null;
      isEditing.value = false;
    };

    const submitTransaction = async (transactionData: Transaction) => {
      if (!transactionData) return;
      try {
        const formattedTransaction = formatTransaction(transactionData);

        if (isEditing.value) {
          await transactionService.updateTransaction(
            formattedTransaction.id,
            formattedTransaction
          );
        } else {
          await transactionService.createTransaction(formattedTransaction);
        }

        await fetchTransactions();
        closeEditModal();
      } catch (error) {
        console.error('Failed to submit transaction:', error);
      }
    };

    const deleteTransaction = async (id: number) => {
      try {
        await transactionService.deleteTransaction(id);
        transactions.value = transactions.value.filter((t) => t.id !== id);
      } catch (error) {
        console.error('Failed to delete transaction:', error);
      }
    };

    onMounted(fetchTransactions);

    return {
      transactions,
      filteredTransactions,
      editingTransaction,
      isEditing,
      filterBy,
      totalExpenses,
      totalIncome,
      netBalance,
      openAddTransaction,
      openEditTransaction,
      closeEditModal,
      submitTransaction,
      deleteTransaction,
    };
  },
});
</script>
