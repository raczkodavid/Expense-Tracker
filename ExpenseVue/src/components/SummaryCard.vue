<template>
  <div
    class="card transform bg-base-100 p-6 text-center shadow-lg transition-transform hover:scale-105"
  >
    <h3 class="text-xl font-semibold text-base-content">{{ title }}</h3>
    <p class="text-3xl font-bold" :class="colorClass">${{ value }}</p>
  </div>
</template>

<script>
import { defineComponent } from 'vue';

/**
 * Reusable component for displaying a single summary card with a title and value.
 * @component
 * @prop {string} title - The title of the card (e.g., "Total Expenses").
 * @prop {string} value - The value to display (e.g., "123.45").
 * @prop {string} colorClass - Tailwind CSS class for text color (e.g., "text-error").
 */
export default defineComponent({
  name: 'SummaryCard',
  props: {
    title: {
      type: String,
      required: true,
    },
    value: {
      type: String,
      required: true,
    },
    colorClass: {
      type: String,
      required: true,
    },
  },
});
</script>
