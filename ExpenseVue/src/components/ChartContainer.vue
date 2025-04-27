<template>
  <div class="space-y-8">
    <!-- Bar Chart (Category Breakdown) -->
    <div class="bg-base-100 rounded-lg p-6 shadow">
      <h3 class="mb-4 text-lg font-semibold">Category Breakdown</h3>
      <div class="h-64">
        <canvas ref="barChart"></canvas>
      </div>
    </div>

    <!-- Doughnut Chart -->
    <div class="bg-base-100 rounded-lg p-6 shadow">
      <h3 class="mb-4 text-lg font-semibold">Income vs Expenses</h3>
      <div class="h-64">
        <canvas ref="doughnutChart"></canvas>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted, watch } from 'vue';
import Chart from 'chart.js/auto';
import type { Transaction } from '@/types/transaction';

interface CategoryData {
  income: number;
  expenses: number;
}

export default defineComponent({
  name: 'ChartContainer',
  props: {
    transactions: {
      type: Array as () => Transaction[],
      required: true,
    },
  },
  setup(props) {
    const barChart = ref<HTMLCanvasElement | null>(null);
    const doughnutChart = ref<HTMLCanvasElement | null>(null);
    let barChartInstance: Chart | null = null;
    let doughnutChartInstance: Chart | null = null;

    const processChartData = () => {
      const categoryData: Record<string, CategoryData> = {};
      let totalIncome = 0;
      let totalExpenses = 0;

      props.transactions.forEach((transaction) => {
        const category = transaction.category || 'Uncategorized';
        if (!categoryData[category]) {
          categoryData[category] = { income: 0, expenses: 0 };
        }
        if (transaction.type === 0) {
          categoryData[category].income += transaction.amount;
          totalIncome += transaction.amount;
        } else {
          categoryData[category].expenses += transaction.amount;
          totalExpenses += transaction.amount;
        }
      });

      return { categoryData, totalIncome, totalExpenses };
    };

    const updateCharts = () => {
      const { categoryData, totalIncome, totalExpenses } = processChartData();

      // Update bar chart
      const categories = Object.keys(categoryData);
      const expenseData = categories.map(
        (cat) => Math.round(categoryData[cat].expenses * 100) / 100
      );
      const incomeData = categories.map(
        (cat) => Math.round(categoryData[cat].income * 100) / 100
      );

      if (barChartInstance) {
        barChartInstance.data.labels = categories;
        barChartInstance.data.datasets[0].data = incomeData;
        barChartInstance.data.datasets[1].data = expenseData;
        barChartInstance.update();
      }

      // Update doughnut chart
      if (doughnutChartInstance) {
        doughnutChartInstance.data.datasets[0].data = [
          totalIncome,
          totalExpenses,
        ];
        doughnutChartInstance.update();
      }
    };

    onMounted(() => {
      if (barChart.value && doughnutChart.value) {
        barChartInstance = new Chart(barChart.value, {
          type: 'bar',
          data: {
            labels: [],
            datasets: [
              {
                label: 'Income',
                data: [],
                backgroundColor: '#10b981',
              },
              {
                label: 'Expenses',
                data: [],
                backgroundColor: '#ef4444',
              },
            ],
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
            scales: {
              x: {
                stacked: false,
              },
              y: {
                beginAtZero: true,
              },
            },
          },
        });

        doughnutChartInstance = new Chart(doughnutChart.value, {
          type: 'doughnut',
          data: {
            labels: ['Income', 'Expenses'],
            datasets: [
              {
                data: [0, 0],
                backgroundColor: ['#10b981', '#ef4444'],
                hoverOffset: 4,
              },
            ],
          },
          options: {
            responsive: true,
            maintainAspectRatio: false,
          },
        });

        updateCharts();
      }
    });

    watch(() => props.transactions, updateCharts, { deep: true });

    return {
      barChart,
      doughnutChart,
    };
  },
});
</script>
