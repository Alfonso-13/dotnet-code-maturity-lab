(() => {
    const intro = document.getElementById("site-intro");

    if (!intro) {
        return;
    }

    const storageKey = "dotnet-code-maturity-intro-seen";
    const reducedMotion = window.matchMedia("(prefers-reduced-motion: reduce)").matches;
    let alreadySeen = false;

    try {
        alreadySeen = window.sessionStorage.getItem(storageKey) === "true";
    } catch {
        alreadySeen = false;
    }

    if (reducedMotion || alreadySeen) {
        document.documentElement.classList.add("intro-seen");
        intro.hidden = true;
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
        document.documentElement.classList.add("intro-seen");
        intro.hidden = true;
    };

    const hideIfSeen = () => {
        try {
            if (window.sessionStorage.getItem(storageKey) === "true") {
                finishIntro();
            }
        } catch {
            // The animation timeout remains as the fallback.
        }
    };

    window.dotnetIntro = { hideIfSeen };

    document.addEventListener("click", event => {
        const link = event.target.closest?.("a[href]");
        if (link && link.origin === window.location.origin) {
            hideIfSeen();
        }
    }, true);

    window.addEventListener("popstate", hideIfSeen);

    intro.addEventListener("animationend", event => {
        if (event.target === intro && event.animationName === "intro-curtain-exit") {
            finishIntro();
        }
    });

    window.setTimeout(finishIntro, 2700);
})();
