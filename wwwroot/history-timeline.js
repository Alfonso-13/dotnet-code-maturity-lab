(() => {
    let listening = false;
    let frameRequested = false;

    const update = () => {
        frameRequested = false;
        const chronology = document.querySelector(".history-chronology");
        if (!chronology) return;

        const rows = [...chronology.querySelectorAll(".history-year-row")];
        if (!rows.length) return;

        const activationY = window.innerHeight * 0.62;
        let lastReached = -1;

        rows.forEach((row, index) => {
            const reached = row.getBoundingClientRect().top + 27 <= activationY;
            row.classList.toggle("is-reached", reached);
            row.classList.remove("is-current-step");
            if (reached) lastReached = index;
        });

        if (lastReached < 0) lastReached = 0;
        rows[lastReached].classList.add("is-current-step");

        const start = rows[0].offsetTop + 27;
        const finish = rows[lastReached].offsetTop + 27;
        chronology.style.setProperty("--history-progress-top", `${start}px`);
        chronology.style.setProperty("--history-progress-height", `${Math.max(0, finish - start)}px`);
    };

    const requestUpdate = () => {
        if (frameRequested) return;
        frameRequested = true;
        window.requestAnimationFrame(update);
    };

    const scan = () => {
        requestUpdate();
        if (listening) return;
        listening = true;
        window.addEventListener("scroll", requestUpdate, { passive: true });
        window.addEventListener("resize", requestUpdate, { passive: true });
    };

    window.dotnetHistoryTimeline = { scan };

    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", scan, { once: true });
    } else {
        scan();
    }
})();
