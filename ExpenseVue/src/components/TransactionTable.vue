<!-- TransactionTable.vue -->
<template>
  <section class="card bg-base-100 p-6 shadow-lg">
    <!-- Header with title and filter -->
    <div class="mb-4 flex items-center justify-between">
      <h3 class="text-xl font-semibold text-base-content">
        Recent Transactions
      </h3>
      <div class="dropdown dropdown-end">
        <label tabindex="0" class="btn btn-outline btn-sm">
          Filter: <span class="capitalize">{{ filterBy }}</span>
        </label>
        <ul
          tabindex="0"
          class="dropdown-content menu w-52 rounded-box bg-base-100 p-2 shadow"
        >
          <li><a @click="updateFilter('all')">All</a></li>
          <li><a @click="updateFilter('day')">Day</a></li>
          <li><a @click="updateFilter('month')">Month</a></li>
          <li><a @click="updateFilter('year')">Year</a></li>
        </ul>
      </div>
    </div>

    <!-- Transaction Table -->
    <div class="overflow-x-auto">
      <table class="table w-full">
        <thead class="sticky top-0 z-10 bg-base-100">
          <tr class="text-base-content">
            <th class="min-w-[100px] cursor-pointer" @click="sortBy('date')">
              Date {{ sortIcon('date') }}
            </th>
            <th class="min-w-[80px] cursor-pointer" @click="sortBy('amount')">
              Amount {{ sortIcon('amount') }}
            </th>
            <th class="hidden min-w-[100px] md:table-cell">Category</th>
            <th class="hidden min-w-[120px] lg:table-cell">Description</th>
            <th class="min-w-[80px]">Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-if="paginatedTransactions.length === 0"
            class="hover:bg-base-200"
          >
            <td colspan="5" class="py-4 text-center">No transactions found.</td>
          </tr>
          <tr
            v-else
            v-for="transaction in paginatedTransactions"
            :key="transaction.id"
            class="hover:bg-base-200"
          >
            <td class="whitespace-nowrap">
              {{ formatDate(transaction.date) }}
            </td>
            <td
              :class="
                transaction.type === TransactionType.Expense
                  ? 'text-error'
                  : 'text-success'
              "
              class="whitespace-nowrap"
            >
              {{ formatAmount(transaction) }}
            </td>
            <td class="hidden md:table-cell">{{ transaction.category }}</td>
            <td class="hidden max-w-xs truncate lg:table-cell">
              {{ transaction.description }}
            </td>
            <td>
              <div class="flex items-center gap-2">
                <button
                  class="btn btn-primary btn-outline btn-xs"
                  @click="$emit('edit', transaction)"
                >
                  Edit
                </button>
                <button
                  class="btn btn-error btn-outline btn-xs"
                  @click="$emit('delete', transaction.id)"
                >
                  Delete
                </button>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- Pagination Controls -->
    <div class="mt-4 flex items-center justify-between">
      <button
        class="btn btn-outline btn-sm"
        :disabled="currentPage === 1 || totalPages === 0"
        @click="currentPage--"
      >
        Previous
      </button>
      <span> Page {{ currentPage }} of {{ totalPages || 1 }} </span>
      <button
        class="btn btn-outline btn-sm"
        :disabled="currentPage === totalPages || totalPages === 0"
        @click="currentPage++"
      >
        Next
      </button>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, computed, ref, watch } from 'vue';
import type { Transaction } from '@/types/transaction';
import { TransactionType } from '@/types/transaction';

/**
 * Component for displaying a table of transactions with filtering, sorting, and pagination.
 * @component
 * @prop {Array} transactions - List of transaction objects
 * @prop {string} filterBy - Current filter type ('all', 'day', 'month', 'year')
 * @emits {string} update:filter-by - Emits new filter value
 * @emits {Object} edit - Emits transaction object to edit
 * @emits {number} delete - Emits transaction ID to delete
 */
export default defineComponent({
  name: 'TransactionTable',
  props: {
    transactions: {
      type: Array as () => Transaction[],
      required: true,
    },
    filterBy: {
      type: String,
      default: 'all',
    },
  },
  emits: ['update:filter-by', 'edit', 'delete'],
  setup(props, { emit }) {
    // State
    const currentPage = ref(1);
    const itemsPerPage = 10;
    const sortField = ref('date');
    const sortDirection = ref('desc');

    // Computed properties
    const totalPages = computed(() =>
      Math.ceil(props.transactions.length / itemsPerPage)
    );

    const sortedTransactions = computed(() => {
      return [...props.transactions].sort((a, b) => {
        let comparison = 0;
        if (sortField.value === 'date') {
          const dateA = new Date(a.date).getTime();
          const dateB = new Date(b.date).getTime();
          comparison = dateA - dateB;
        } else if (sortField.value === 'amount') {
          // For expenses (type 1), we want to show them as negative values
          const amountA =
            a.type === TransactionType.Expense
              ? -Math.abs(a.amount)
              : Math.abs(a.amount);
          const amountB =
            b.type === TransactionType.Expense
              ? -Math.abs(b.amount)
              : Math.abs(b.amount);
          comparison = amountA - amountB;
        }
        return sortDirection.value === 'asc' ? comparison : -comparison;
      });
    });

    const paginatedTransactions = computed(() => {
      const start = (currentPage.value - 1) * itemsPerPage;
      const end = start + itemsPerPage;
      return sortedTransactions.value.slice(start, end);
    });

    // Methods
    const formatDate = (date: string): string => {
      return new Date(date).toLocaleDateString();
    };

    const formatAmount = (transaction: Transaction): string => {
      const prefix = transaction.type === TransactionType.Expense ? '-' : '';
      return `${prefix}$${transaction.amount.toFixed(2)}`;
    };

    const sortIcon = (field: string): string => {
      if (sortField.value !== field) return '↕';
      return sortDirection.value === 'asc' ? '↑' : '↓';
    };

    const sortBy = (field: string): void => {
      if (sortField.value === field) {
        sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
      } else {
        sortField.value = field;
        sortDirection.value = 'desc';
      }
    };

    const updateFilter = (value: string): void => {
      emit('update:filter-by', value);
    };

    // Reset to page 1 when transactions update
    watch(
      () => props.transactions,
      () => {
        currentPage.value = 1;
      }
    );

    return {
      currentPage,
      totalPages,
      paginatedTransactions,
      sortIcon,
      sortBy,
      updateFilter,
      formatDate,
      formatAmount,
      TransactionType,
    };
  },
});
</script>
