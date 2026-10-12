window.sayHello = function () {
    alert("Hello from JavaScript");
}

// Custom SweetAlert2 styling for calm POS theme
const posSwalMixin = {
    customClass: {
        popup: 'pos-swal-popup',
        title: 'pos-swal-title',
        htmlContainer: 'pos-swal-html',
        confirmButton: 'btn btn-primary px-4 me-2',
        cancelButton: 'btn btn-secondary px-4'
    },
    buttonsStyling: false
};

// Keeps legacy Razor pages Arabic while Blazor updates their DOM dynamically.
(() => {
    const dictionary = {
        "Home": "الرئيسية", "Items": "الخدمات", "Barber": "الحلاقون", "Orders": "الطلبات",
        "Reports": "التقارير", "Expense": "المصروفات", "Attendance": "الحضور والغياب",
        "Salary": "الرواتب", "Deduction": "الخصومات", "Paid Salary": "الرواتب المدفوعة",
        "Logout": "تسجيل الخروج", "Login": "تسجيل الدخول", "Search": "بحث", "Show All": "عرض الكل",
        "Previous": "السابق", "Next": "التالي", "Edit": "تعديل", "Delete": "حذف", "Cancel": "إلغاء",
        "Save": "حفظ", "Add": "إضافة", "Details": "التفاصيل", "Password": "كلمة المرور",
        "Name": "الاسم", "Price": "السعر", "Amount": "المبلغ", "Reason": "السبب",
        "Created By": "أُنشئ بواسطة", "Date": "التاريخ", "Status": "الحالة", "Actions": "الإجراءات",
        "Present": "حاضر", "Absent": "غائب", "Not Recorded": "غير مسجل", "Not Started": "لم يبدأ العمل",
        "Service Name": "اسم الخدمة", "Daily Price": "الأجر اليومي", "Percentage": "النسبة",
        "Total Price": "الإجمالي", "Total": "الإجمالي", "Barber": "الحلاق", "Attendance": "الحضور",
        "Add Item": "إضافة خدمة", "Edit Item": "تعديل خدمة", "Delete Item": "حذف خدمة",
        "Add Barber": "إضافة حلاق", "Edit Barber": "تعديل حلاق", "Delete Barber": "حذف حلاق",
        "Add Expense": "إضافة مصروف", "Edit Expense": "تعديل مصروف", "Delete Expense": "حذف مصروف",
        "Add Deduction": "إضافة خصم", "Edit Deduction": "تعديل خصم", "Delete Deduction": "حذف خصم",
        "Delete Order": "حذف طلب", "Order Summary": "ملخص الطلب", "Services": "الخدمات",
        "Selected Services": "الخدمات المختارة", "Select barber…": "اختر الحلاق…",
        "No services added": "لم تتم إضافة خدمات", "Complete Order": "إتمام الطلب",
        "Monthly reports": "التقارير الشهرية", "Month": "الشهر", "Year": "السنة",
        "Generate report": "إنشاء التقرير", "Total sales": "إجمالي المبيعات", "Total orders": "إجمالي الطلبات",
        "Net profit": "صافي الربح", "Team salaries": "رواتب الفريق", "Operating expenses": "المصروفات التشغيلية",
        "Item Management": "إدارة الخدمات", "Barber Management": "إدارة الحلاقين",
        "Orders Management": "إدارة الطلبات", "Expense Management": "إدارة المصروفات",
        "Salary Management": "إدارة الرواتب", "Deduction Management": "إدارة الخصومات",
        "Manage your barbershop items and prices.": "أدر خدمات الصالون والأسعار بسهولة.",
        "Manage your barbershop services and prices.": "أدر بيانات الحلاقين وخيارات الرواتب.",
        "Manage your barbershop Orders.": "راجع وأدر طلبات الصالون.",
        "Manage your barbershop expenses.": "راجع وسجل مصروفات الصالون.",
        "View current barber earnings and calculate monthly salaries.": "راجع مستحقات الحلاقين واحسب الرواتب الشهرية.",
        "Manage your barbershop deduction.": "راجع وأدر خصومات الحلاقين.",
        "New Order": "طلب جديد", "Show all": "عرض الكل", "Save Changes": "حفظ التغييرات",
        "Basic Salary": "الراتب الأساسي", "Deductions": "الخصومات", "Final Salary": "صافي الراتب",
        "Orders": "الطلبات", "No Salary": "لا يوجد راتب", "No Deduction": "لا توجد خصومات",
        "Welcome Back": "مرحبًا بعودتك", "Username": "اسم المستخدم", "Sign In": "تسجيل الدخول",
        "Signing in…": "جارٍ تسجيل الدخول…", "Business overview": "نظرة عامة على النشاط",
        "Financial summary": "الملخص المالي", "Team performance": "أداء الفريق",
        "Barbers report": "تقرير الحلاقين", "Employee deductions": "خصومات الموظفين",
        "Gross salary": "الراتب الأساسي", "Net salary": "صافي الراتب", "No services found": "لا توجد خدمات",
        "Processing…": "جارٍ التنفيذ…", "All Deduction": "كل الخصومات", "Record attendance": "تسجيل الحضور",
        "Back to daily attendance": "العودة للحضور اليومي", "Search history": "بحث في السجل",
        "From date": "من تاريخ", "To date": "إلى تاريخ", "Attendance history": "سجل الحضور",
        "All recorded days": "كل الأيام المسجلة", "Attendance information": "معلومات الحضور"
    };

    const translate = (root = document.body) => {
        const walker = document.createTreeWalker(root, NodeFilter.SHOW_TEXT);
        const nodes = [];
        while (walker.nextNode()) nodes.push(walker.currentNode);
        nodes.forEach(node => {
            const value = node.nodeValue;
            const trimmed = value.trim();
            if (dictionary[trimmed]) node.nodeValue = value.replace(trimmed, dictionary[trimmed]);
            else if (/^Page\s+(\d+)$/i.test(trimmed)) node.nodeValue = value.replace(trimmed, `صفحة ${trimmed.match(/\d+/)[0]}`);
            else if (/^(\d+)\s+orders$/i.test(trimmed)) node.nodeValue = value.replace(trimmed, `${trimmed.match(/\d+/)[0]} طلب`);
            else if (/^(\d+)\s+barbers$/i.test(trimmed)) node.nodeValue = value.replace(trimmed, `${trimmed.match(/\d+/)[0]} حلاق`);
        });
        root.querySelectorAll?.('[placeholder], [title]').forEach(element => {
            ["placeholder", "title"].forEach(attribute => {
                const value = element.getAttribute(attribute);
                if (value && dictionary[value]) element.setAttribute(attribute, dictionary[value]);
            });
        });
    };

    const start = () => {
        translate();
        new MutationObserver(mutations => mutations.forEach(mutation => mutation.addedNodes.forEach(node => {
            if (node.nodeType === Node.ELEMENT_NODE) translate(node);
            if (node.nodeType === Node.TEXT_NODE && dictionary[node.nodeValue.trim()]) node.nodeValue = dictionary[node.nodeValue.trim()];
        }))).observe(document.body, { childList: true, subtree: true });
    };
    document.readyState === "loading" ? document.addEventListener("DOMContentLoaded", start) : start();
})();

window.showSuccess = function (message) {
    Swal.fire({
        ...posSwalMixin,
        title: 'Success',
        text: message,
        icon: 'success',
        confirmButtonText: 'OK'
    });
}

window.showToast = function (message, icon = 'success') {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-toast-popup'
        }
    });
}

window.showToast2 = function (message, icon = 'error') {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-toast-popup'
        }
    });
}

window.showToast3 = function (message, icon = 'warning') {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-toast-popup'
        }
    });
}

window.showToast4 = function (message, icon = 'info') {
    Swal.fire({
        toast: true,
        position: 'top-end',
        icon: icon,
        title: message,
        showConfirmButton: false,
        timer: 3000,
        timerProgressBar: true,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-toast-popup'
        }
    });
}

window.showLoading = function () {
    Swal.fire({
        title: 'Loading...',
        text: 'Please wait',
        allowOutsideClick: false,
        allowEscapeKey: false,
        background: '#ffffff',
        color: '#1e293b',
        customClass: {
            popup: 'pos-swal-popup'
        },
        didOpen: () => {
            Swal.showLoading();
        }
    });
}

window.hideLoading = function () {
    Swal.close();
}

window.exportMonthlyReportPdf = async function (filename, report) {
    if (!window.jspdf) throw new Error("PDF library is unavailable");

    const { jsPDF } = window.jspdf;
    const pdf = new jsPDF({ orientation: "p", unit: "mm", format: "a4" });
    pdf.setFont("helvetica");

    const pageWidth = pdf.internal.pageSize.getWidth();
    const pageHeight = pdf.internal.pageSize.getHeight();
    const margin = 16;
    const contentWidth = pageWidth - margin * 2;
    let y = 20;
    const money = value => `${Number(value).toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })} EGP`;
    const write = (text, size = 10, color = [30, 41, 59]) => {
        pdf.setFontSize(size);
        pdf.setTextColor(...color);
        const lines = pdf.splitTextToSize(String(text), contentWidth);
        if (y + lines.length * (size * 0.55) > pageHeight - margin) { pdf.addPage(); y = margin; }
        pdf.text(lines, margin, y);
        y += lines.length * (size * 0.55) + 4;
    };
    const line = () => { pdf.setDrawColor(226, 232, 240); pdf.line(margin, y, pageWidth - margin, y); y += 6; };

    write("Craft Barbershop", 20);
    write(`Monthly Report - ${report.month} ${report.year}`, 13, [71, 85, 105]);
    line();
    write(`Total Sales: ${money(report.sales)}`, 11);
    write(`Total Orders: ${report.orders}`, 11);
    write(`Net Profit: ${money(report.profit)}`, 11, report.profit >= 0 ? [4, 120, 87] : [185, 28, 28]);
    write(`Team Salaries: ${money(report.salaries)}`, 11);
    write(`Operating Expenses: ${money(report.expenses)}`, 11);
    write(`Employee Deductions: ${money(report.deductions)}`, 11);
    y += 3;
    line();
    write("Barber Performance Details", 14);

    for (const barber of report.barbers || []) {
        if (y > pageHeight - 45) { pdf.addPage(); y = margin; }
        write(barber.name, 12, [37, 99, 235]);
        write(`Orders: ${barber.orders} | Attendance: ${barber.attendance} days | Net Salary: ${money(barber.netSalary)}`, 9);
        write(`Salary: ${money(barber.salary)} | Deductions: ${money(barber.deductions)}`, 9, [71, 85, 105]);
        line();
    }

    pdf.save(filename);
}

let monthlyFinanceChart;

window.renderMonthlyFinanceChart = function (data) {
    const canvas = document.getElementById("monthly-finance-chart");
    if (!canvas || !window.Chart) return;

    monthlyFinanceChart?.destroy();
    const sales = Number(data.sales) || 0;
    const salaries = Number(data.salaries) || 0;
    const expenses = Number(data.expenses) || 0;
    const deductions = Number(data.deductions) || 0;
    const afterSalaries = sales - salaries;
    const afterExpenses = afterSalaries - expenses;
    const profit = Number(data.profit) || 0;
    const values = [sales, -salaries, -expenses, deductions, profit];
    const waterfall = [[0, sales], [afterSalaries, sales], [afterExpenses, afterSalaries], [afterExpenses, afterExpenses + deductions], [0, profit]];
    const valueLabels = {
        id: "waterfallValueLabels",
        afterDatasetsDraw(chart) {
            const context = chart.ctx;
            const meta = chart.getDatasetMeta(0);
            context.save();
            context.fillStyle = "#334155";
            context.font = "600 11px Cairo, sans-serif";
            context.textAlign = "center";
            meta.data.forEach((bar, index) => {
                const point = bar.getProps(["x", "y", "base"], true);
                const position = values[index] >= 0 ? Math.min(point.y, point.base) - 7 : Math.max(point.y, point.base) + 16;
                context.fillText(`${Math.abs(values[index]).toLocaleString("ar-EG", { maximumFractionDigits: 0 })} ج.م`, point.x, position);
            });
            context.restore();
        }
    };
    monthlyFinanceChart = new Chart(canvas, {
        type: "bar",
        data: {
            labels: ["المبيعات", "إجمالي الرواتب", "المصروفات", "الخصومات", "صافي النتيجة"],
            datasets: [{
                data: waterfall,
                backgroundColor: ["#2563eb", "#f59e0b", "#e11d48", "#10b981", profit >= 0 ? "#059669" : "#dc2626"],
                borderRadius: 8,
                borderSkipped: false,
                maxBarThickness: 52
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: false },
                tooltip: {
                    rtl: true,
                    callbacks: { label: context => `${Math.abs(values[context.dataIndex]).toLocaleString("ar-EG", { minimumFractionDigits: 2 })} ج.م` }
                }
            },
            scales: {
                x: { grid: { display: false }, ticks: { color: "#475569", font: { family: "Cairo", size: 12 } } },
                y: {
                    grid: { color: "#e2e8f0", drawBorder: false },
                    ticks: { color: "#64748b", callback: value => `${Number(value).toLocaleString("ar-EG")} ج.م` }
                }
            },
            animation: { duration: 650 }
        },
        plugins: [valueLabels]
    });
}

const orderPrintWindowName = "barbershop-order-receipt";

window.prepareOrderPrint = function () {
    const popup = window.open("", orderPrintWindowName, "width=420,height=640");
    if (!popup) return false;
    popup.document.open();
    popup.document.write("<p style='font-family:Arial;text-align:center;padding:2rem'>جارٍ تجهيز الفاتورة...</p>");
    popup.document.close();
    return true;
}

window.closeOrderPrintWindow = function () {
    const popup = window.open("", orderPrintWindowName);
    if (popup) popup.close();
}

window.printOrderReceipt = function (receipt) {
    const popup = window.open("", orderPrintWindowName, "width=420,height=640");
    if (!popup) {
        window.showToast3("اسمح للنوافذ المنبثقة لطباعة الفاتورة.");
        return;
    }

    const escapeHtml = value => String(value ?? "").replace(/[&<>'"]/g, character => ({
        "&": "&amp;", "<": "&lt;", ">": "&gt;", "'": "&#39;", '"': "&quot;"
    })[character]);
    const money = value => `${Number(value).toLocaleString("ar-EG", { minimumFractionDigits: 2, maximumFractionDigits: 2 })} ج.م`;
    const rows = (receipt.items || []).map(item => `
        <tr><td>${escapeHtml(item.name)}</td><td>${money(item.price)}</td></tr>`).join("");

    popup.document.open();
    popup.document.write(`<!doctype html><html lang="ar" dir="rtl"><head><meta charset="utf-8"><title>فاتورة طلب</title>
        <style>
            *{box-sizing:border-box} body{font-family:Tahoma,Arial,sans-serif;margin:0;color:#172033;background:#fff}
            .receipt{width:80mm;margin:0 auto;padding:8mm 6mm}.brand{text-align:center;border-bottom:2px dashed #cbd5e1;padding-bottom:4mm}
            h1{font-size:18px;margin:0 0 5px}.muted{color:#64748b;font-size:12px}.meta{margin:5mm 0;font-size:13px;line-height:1.8}
            table{width:100%;border-collapse:collapse;font-size:13px}th,td{padding:3mm 0;border-bottom:1px solid #e2e8f0;text-align:right}th:last-child,td:last-child{text-align:left}
            .total{display:flex;justify-content:space-between;font-weight:700;font-size:16px;padding:4mm 0}.thanks{text-align:center;color:#64748b;font-size:12px;border-top:2px dashed #cbd5e1;padding-top:4mm}
            @media print{body{margin:0}.receipt{width:80mm;margin:0;padding:5mm}.no-print{display:none}}
        </style></head><body><main class="receipt"><header class="brand"><h1>كرافت باربرشوب</h1><span class="muted">فاتورة طلب</span></header>
        <div class="meta"><div><strong>الحلاق:</strong> ${escapeHtml(receipt.barberName)}</div>${receipt.clientName ? `<div><strong>العميل:</strong> ${escapeHtml(receipt.clientName)}</div>` : ''}<div><strong>التاريخ:</strong> ${escapeHtml(receipt.date)}</div></div>
        <table><thead><tr><th>الخدمة</th><th>السعر</th></tr></thead><tbody>${rows}</tbody></table>
        <div class="total"><span>الإجمالي</span><span>${money(receipt.total)}</span></div><p class="thanks">شكرًا لزيارتكم</p></main>
        <script>window.onload=()=>setTimeout(()=>window.print(),150);<\/script></body></html>`);
    popup.document.close();
}

window.storage = {
    setAccessToken: function (token) {
        document.cookie = "AccessToken=" + token + "; path=/; max-age=2592000; samesite=lax";
        try { localStorage.setItem("AccessToken", token); } catch(e){}
    },

    getAccessToken: function () {
        const name = "AccessToken=";
        const decodedCookie = decodeURIComponent(document.cookie);
        const ca = decodedCookie.split(';');
        for(let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        try { return localStorage.getItem("AccessToken") || ""; } catch(e){}
        return "";
    },

    setRefreshToken: function (token) {
        document.cookie = "RefreshToken=" + token + "; path=/; max-age=2592000; samesite=lax";
        try { localStorage.setItem("RefreshToken", token); } catch(e){}
    },

    getRefreshToken: function () {
        const name = "RefreshToken=";
        const decodedCookie = decodeURIComponent(document.cookie);
        const ca = decodedCookie.split(';');
        for(let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        try { return localStorage.getItem("RefreshToken") || ""; } catch(e){}
        return "";
    },

    clear: function () {
        document.cookie = "AccessToken=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; samesite=lax";
        document.cookie = "RefreshToken=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC; samesite=lax";
        try { localStorage.removeItem("AccessToken"); localStorage.removeItem("RefreshToken"); } catch(e){}
    }
};
