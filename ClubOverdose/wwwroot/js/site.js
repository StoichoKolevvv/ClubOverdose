// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.addEventListener("DOMContentLoaded", function () {
    const toasts = document.querySelectorAll(".club-toast");

    toasts.forEach(function (toast) {
        const closeButton = toast.querySelector(".club-toast-close");

        const hideToast = function () {
            toast.classList.add("is-hiding");
            window.setTimeout(function () {
                toast.remove();
            }, 260);
        };

        const timer = window.setTimeout(hideToast, 4200);

        if (closeButton) {
            closeButton.addEventListener("click", function () {
                window.clearTimeout(timer);
                hideToast();
            });
        }
    });
});
