(() => {
    const reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    let observer;
    let mutationObserver;
    let scanQueued = false;

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

    const scheduleScan = () => {
        if (scanQueued) return;

        scanQueued = true;
        window.requestAnimationFrame(() => {
            scanQueued = false;
            scan();
        });
    };

    const watchDynamicContent = () => {
        if (!document.body || mutationObserver) {
            scheduleScan();
            return;
        }

        mutationObserver = new MutationObserver(mutations => {
            if (mutations.some(mutation => mutation.addedNodes.length > 0)) {
                scheduleScan();
            }
        });

        mutationObserver.observe(document.body, {
            childList: true,
            subtree: true
        });

        scheduleScan();
    };

    window.sectionReveal = { scan: scheduleScan };

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", watchDynamicContent, { once: true });
    } else {
        watchDynamicContent();
    }
})();
