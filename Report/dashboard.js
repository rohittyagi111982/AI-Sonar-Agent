document.addEventListener("DOMContentLoaded", function () {

    const menuItems = document.querySelectorAll(
        ".sidebar .nav-link"
    );

    const tabs = document.querySelectorAll(
        ".tab-pane"
    );

    menuItems.forEach(function (menu) {

        menu.addEventListener("click", function (event) {

            event.preventDefault();

            const targetId =
                menu.getAttribute("data-bs-target") ||
                menu.getAttribute("href");

            if (!targetId) {
                return;
            }

            // Remove active from menu
            menuItems.forEach(function (item) {
                item.classList.remove("active");
            });

            // Activate clicked menu
            menu.classList.add("active");

            // Hide all sections
            tabs.forEach(function (tab) {
                tab.classList.remove("show");
                tab.classList.remove("active");
            });

            // Show selected section
            const target =
                document.querySelector(targetId);

            if (target) {
                target.classList.add("show");
                target.classList.add("active");
            }

        });

    });

    // Scroll to top button
    createScrollToTopButton();
});


function createScrollToTopButton() {

    const button = document.createElement("button");

    button.id = "scrollTopBtn";
    button.innerHTML = "↑";
    button.type = "button";

    button.className = "btn btn-primary";

    button.style.position = "fixed";
    button.style.right = "20px";
    button.style.bottom = "20px";
    button.style.width = "42px";
    button.style.height = "42px";
    button.style.borderRadius = "50%";
    button.style.display = "none";
    button.style.zIndex = "9999";

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


function printReport() {
    window.print();
}


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