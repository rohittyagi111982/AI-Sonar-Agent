/*
 * SonarQube Report Dashboard
 * dashboard.js
 */

document.addEventListener("DOMContentLoaded", function () {

    // Bootstrap sidebar navigation
    const navLinks = document.querySelectorAll(
        ".sidebar .nav-link[data-bs-toggle='pill']"
    );

    navLinks.forEach(function (link) {
        link.addEventListener("shown.bs.tab", function (event) {

            navLinks.forEach(function (item) {
                item.classList.remove("active");
            });

            event.target.classList.add("active");
        });
    });

    // Optional DataTables support
    if (window.jQuery &&
        window.jQuery.fn &&
        window.jQuery.fn.DataTable) {

        document.querySelectorAll("table.datatable")
            .forEach(function (table) {

                window.jQuery(table).DataTable({
                    pageLength: 10,
                    lengthMenu: [10, 25, 50, 100],
                    ordering: true,
                    searching: true,
                    responsive: true,
                    autoWidth: false
                });

            });
    }

    // Scroll-to-top button
    createScrollToTopButton();

    // Security for external links
    document.querySelectorAll("a[target='_blank']")
        .forEach(function (link) {

            link.setAttribute(
                "rel",
                "noopener noreferrer"
            );

        });
});


// ============================================================
// Scroll to top
// ============================================================

function createScrollToTopButton() {

    const button = document.createElement("button");

    button.id = "scrollTopBtn";
    button.type = "button";
    button.className = "btn btn-primary";
    button.setAttribute("aria-label", "Scroll to top");
    button.innerHTML = "↑";

    Object.assign(button.style, {
        position: "fixed",
        right: "20px",
        bottom: "20px",
        width: "42px",
        height: "42px",
        borderRadius: "50%",
        display: "none",
        zIndex: "9999"
    });

    document.body.appendChild(button);

    window.addEventListener("scroll", function () {

        if (window.scrollY > 250) {
            button.style.display = "block";
        }
        else {
            button.style.display = "none";
        }

    });

    button.addEventListener("click", function () {

        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });

    });
}


// ============================================================
// Print report
// ============================================================

function printReport() {
    window.print();
}


// ============================================================
// Export HTML
// ============================================================

function exportHtml(filename) {

    filename = filename || "SonarReport.html";

    const html =
        "<!DOCTYPE html>\n" +
        document.documentElement.outerHTML;

    const blob = new Blob(
        [html],
        {
            type: "text/html;charset=utf-8"
        }
    );

    const url = URL.createObjectURL(blob);

    const link = document.createElement("a");

    link.href = url;
    link.download = filename;

    document.body.appendChild(link);

    link.click();

    document.body.removeChild(link);

    setTimeout(function () {
        URL.revokeObjectURL(url);
    }, 1000);
}