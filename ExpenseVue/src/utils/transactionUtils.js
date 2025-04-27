/**
 * Utility functions for transaction handling and chart data computation.
 */

/**
 * Filters transactions based on the specified filter type.
 * @param {Array} transactions - List of transaction objects.
 * @param {string} filterBy - Filter type ('all', 'day', 'month', 'year').
 * @returns {Array} Filtered transaction list.
 */
export function filterTransactions(transactions, filterBy) {
  const now = new Date();
  const startOfDay = new Date(now.getFullYear(), now.getMonth(), now.getDate());
  const startOfMonth = new Date(now.getFullYear(), now.getMonth(), 1);
  const startOfYear = new Date(now.getFullYear(), 0, 1);

  return transactions.filter((t) => {
    const tDate = new Date(t.date);
    switch (filterBy) {
      case 'day':
        return (
          tDate >= startOfDay &&
          tDate < new Date(startOfDay.getTime() + 24 * 60 * 60 * 1000)
        );
      case 'month':
        return (
          tDate >= startOfMonth &&
          tDate < new Date(now.getFullYear(), now.getMonth() + 1, 1)
        );
      case 'year':
        return (
          tDate >= startOfYear && tDate < new Date(now.getFullYear() + 1, 0, 1)
        );
      default:
        return true;
    }
  });
}

/**
 * Computes data for the line chart (income/expense over time).
 * @param {Array} transactions - Filtered list of transaction objects.
 * @param {string} filterBy - Filter type ('all', 'day', 'month', 'year').
 * @returns {Object} Chart.js data object.
 */
export function computeLineChartData(transactions, filterBy) {
  // Determine time grouping based on filterBy
  const groupBy =
    filterBy === 'day'
      ? 'hour'
      : filterBy === 'month'
        ? 'day'
        : filterBy === 'year'
          ? 'month'
          : 'month';

  // Group transactions by time period
  const groupedData = transactions.reduce((acc, t) => {
    const tDate = new Date(t.date);
    let key;
    if (groupBy === 'hour') {
      key = `${tDate.toLocaleDateString()} ${tDate.getHours()}:00`;
    } else if (groupBy === 'day') {
      key = tDate.toLocaleDateString();
    } else {
      key = `${tDate.getFullYear()}-${tDate.getMonth() + 1}`;
    }

    if (!acc[key]) {
      acc[key] = { income: 0, expense: 0 };
    }
    if (t.type === 0) {
      acc[key].income += t.amount;
    } else {
      acc[key].expense += t.amount;
    }
    return acc;
  }, {});

  // Sort keys chronologically
  const labels = Object.keys(groupedData).sort(
    (a, b) => new Date(a) - new Date(b)
  );
  const incomeData = labels.map((key) => groupedData[key].income);
  const expenseData = labels.map((key) => groupedData[key].expense);

  return {
    labels,
    datasets: [
      {
        label: 'Income',
        data: incomeData,
        borderColor: 'rgb(75, 192, 192)',
        backgroundColor: 'rgba(75, 192, 192, 0.2)',
        fill: false,
      },
      {
        label: 'Expenses',
        data: expenseData,
        borderColor: 'rgb(255, 99, 132)',
        backgroundColor: 'rgba(255, 99, 132, 0.2)',
        fill: false,
      },
    ],
  };
}

/**
 * Computes data for the pie chart (expenses by category).
 * @param {Array} transactions - Filtered list of transaction objects.
 * @returns {Object} Chart.js data object.
 */
export function computePieChartData(transactions) {
  // Group expenses by category
  const categoryData = transactions
    .filter((t) => t.type === 1) // Only expenses
    .reduce((acc, t) => {
      acc[t.category] = (acc[t.category] || 0) + t.amount;
      return acc;
    }, {});

  const labels = Object.keys(categoryData);
  const data = Object.values(categoryData);
  const backgroundColors = labels.map((_, i) => {
    const hue = (i * 137.5) % 360; // Golden angle for distinct colors
    return `hsl(${hue}, 70%, 50%)`;
  });

  return {
    labels,
    datasets: [
      {
        data,
        backgroundColor: backgroundColors,
      },
    ],
  };
}
