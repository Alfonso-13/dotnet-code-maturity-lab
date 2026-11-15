(() => {
    const reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    let observer;

    const show = element => {
        element.classList.add("reveal-visible");
        observer?.unobserve(element);
    };

    const scan = () => {
        const sections = document.querySelectorAll(".reveal-section:not(.reveal-ready)");

        if (reducedMotion || !("IntersectionObserver" in window)) {
            sections.forEach(element => {
                element.classList.add("reveal-ready");
                show(element);
            });
            return;
        }

        observer ??= new IntersectionObserver(entries => {
            entries.forEach(entry => {
                if (entry.isIntersecting) show(entry.target);
            });
        }, { threshold: 0.14, rootMargin: "0px 0px -8% 0px" });

        sections.forEach((element, index) => {
            element.classList.add("reveal-ready");
            element.style.setProperty("--reveal-order", index);
            observer.observe(element);
        });
    };

    window.sectionReveal = { scan };

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", scan, { once: true });
    } else {
        scan();
    }
})();
