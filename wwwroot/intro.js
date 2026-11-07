(() => {
    const intro = document.getElementById("site-intro");

    if (!intro) {
        return;
    }

    const storageKey = "dotnet-code-maturity-intro-seen";
    const replayRequested = new URLSearchParams(window.location.search).has("intro");
    const reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    let alreadySeen = false;

    try {
        alreadySeen = window.sessionStorage.getItem(storageKey) === "true";
    } catch {
        alreadySeen = false;
    }

    if (reducedMotion || (alreadySeen && !replayRequested)) {
        intro.remove();
        return;
    }

    document.documentElement.classList.add("intro-playing");

    try {
        window.sessionStorage.setItem(storageKey, "true");
    } catch {
        // The animation can still run when session storage is unavailable.
    }

    const finishIntro = () => {
        document.documentElement.classList.remove("intro-playing");
        intro.remove();
    };

    intro.addEventListener("animationend", event => {
        if (event.target === intro && event.animationName === "intro-curtain-exit") {
            finishIntro();
        }
    }, { once: true });

    window.setTimeout(finishIntro, 2200);
})();
