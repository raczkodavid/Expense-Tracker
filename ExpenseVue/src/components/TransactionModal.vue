<template>
  <div class="modal modal-open">
    <div class="modal-box">
      <h3 class="text-lg font-semibold">
        {{ isEditing ? 'Edit' : 'Add' }} Transaction
      </h3>
      <form @submit.prevent="submitForm">
        <div class="mb-2">
          <label class="mb-1 block">Amount</label>
          <input
            v-model.number="localTransaction.amount"
            type="number"
            step="0.01"
            min="0"
            class="input input-bordered w-full"
            required
          />
        </div>
        <div class="mb-2">
          <label class="mb-1 block">Description</label>
          <input
            v-model="localTransaction.description"
            type="text"
            class="input input-bordered w-full"
            required
          />
        </div>
        <div class="mb-2">
          <label class="mb-1 block">Category</label>
          <input
            v-model="localTransaction.category"
            type="text"
            class="input input-bordered w-full"
            required
          />
        </div>
        <div class="mb-2">
          <label class="mb-1 block">Date</label>
          <input
            v-model="localTransaction.date"
            type="date"
            class="input input-bordered w-full"
            required
          />
        </div>
        <div class="mb-2">
          <label class="mb-1 block">Type</label>
          <select
            v-model="localTransaction.type"
            class="select select-bordered w-full"
            required
          >
            <option :value="TransactionType.Income">Income</option>
            <option :value="TransactionType.Expense">Expense</option>
          </select>
        </div>
        <div class="modal-action">
          <button
            type="submit"
            class="btn btn-primary"
            :disabled="!isFormValid"
          >
            {{ isEditing ? 'Save' : 'Add' }}
          </button>
          <button
            type="button"
            class="btn btn-secondary"
            @click="$emit('close')"
          >
            Cancel
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, computed, watch } from 'vue';
import { TransactionType } from '@/types/transaction';
import type { Transaction } from '@/types/transaction';

export default defineComponent({
  name: 'TransactionModal',
  props: {
    transaction: {
      type: Object as () => Transaction,
      required: true,
    },
    isEditing: {
      type: Boolean,
      default: false,
    },
  },
  emits: ['submit', 'close'],
  setup(props, { emit }) {
    // Local copy of transaction to avoid mutating prop directly
    const localTransaction = ref<Transaction>({
      ...props.transaction,
      type: props.transaction.type ?? TransactionType.Expense, // Default to Expense if type is not set
    });

    // Ensure amount is always a number
    watch(
      () => localTransaction.value.amount,
      (newAmount) => {
        if (typeof newAmount !== 'number' || isNaN(newAmount)) {
          localTransaction.value.amount = 0;
        }
      },
      { immediate: true }
    );

    /**
     * Validates the form data
     * @returns {boolean} True if the form is valid
     */
    const isFormValid = computed(() => {
      return (
        localTransaction.value.amount > 0 &&
        localTransaction.value.description?.trim() !== '' &&
        localTransaction.value.category?.trim() !== '' &&
        localTransaction.value.date !== '' &&
        (localTransaction.value.type === TransactionType.Income ||
          localTransaction.value.type === TransactionType.Expense)
      );
    });

    /**
     * Submits the form data
     */
    const submitForm = (): void => {
      if (isFormValid.value) {
        const formattedTransaction: Transaction = {
          ...localTransaction.value,
          amount: parseFloat(localTransaction.value.amount.toString()) || 0,
          type: localTransaction.value.type,
        };
        emit('submit', formattedTransaction);
      }
    };

    // Sync localTransaction when prop changes
    watch(
      () => props.transaction,
      (newTransaction) => {
        localTransaction.value = {
          ...newTransaction,
          type: newTransaction.type ?? TransactionType.Expense, // Default to Expense if type is not set
        };
      },
      { deep: true, immediate: true }
    );

    return {
      localTransaction,
      isFormValid,
      submitForm,
      TransactionType,
    };
  },
});
</script>
