let monthlyFinanceComparisonChart;

window.renderMonthlyFinanceChart = function (data) {
    const canvas = document.getElementById("monthly-finance-chart");
    if (!canvas || !window.Chart) return;

    monthlyFinanceComparisonChart?.destroy();

    const sales = Number(data.sales) || 0;
    const netSalaries = Math.max(0, (Number(data.salaries) || 0) - (Number(data.deductions) || 0));
    const expenses = Number(data.expenses) || 0;
    const profit = Number(data.profit) || 0;

    monthlyFinanceComparisonChart = new Chart(canvas, {
        type: "bar",
        data: {
            labels: ["المبيعات", "صافي الرواتب", "المصروفات", profit >= 0 ? "صافي الربح" : "صافي الخسارة"],
            datasets: [{
                data: [sales, netSalaries, expenses, Math.abs(profit)],
                backgroundColor: ["#2563eb", "#f59e0b", "#e11d48", profit >= 0 ? "#059669" : "#dc2626"],
                borderRadius: 10,
                borderSkipped: false,
                maxBarThickness: 34,
                barPercentage: .72
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            indexAxis: "y",
            plugins: {
                legend: { display: false },
                tooltip: {
                    rtl: true,
                    displayColors: false,
                    callbacks: { label: context => `${context.parsed.x.toLocaleString("en-US", { minimumFractionDigits: 2 })} EGP` }
                }
            },
            scales: {
                x: { grid: { color: "#e8eef6", drawBorder: false }, border: { display: false }, ticks: { color: "#94a3b8", callback: value => `${Number(value).toLocaleString("en-US")} EGP` } },
                y: { grid: { display: false }, border: { display: false }, ticks: { color: "#334155", font: { family: "Arial", size: 12, weight: "600" }, padding: 12 } }
            },
            animation: { duration: 450 }
        }
    });
};
