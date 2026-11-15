window.labProgress = {
    get(key) {
        try {
            const value = window.localStorage.getItem(key);
            return value ? JSON.parse(value) : { viewed: [], completed: [] };
        } catch {
            return { viewed: [], completed: [] };
        }
    },
    set(key, progress) {
        try {
            window.localStorage.setItem(key, JSON.stringify(progress));
        } catch {
            // El laboratorio funciona aunque el navegador bloquee el almacenamiento.
        }
    },
    async copyCode(code) {
        if (navigator.clipboard?.writeText) {
            await navigator.clipboard.writeText(code);
            return;
        }

        const textArea = document.createElement("textarea");
        textArea.value = code;
        textArea.setAttribute("readonly", "");
        textArea.style.position = "fixed";
        textArea.style.opacity = "0";
        document.body.appendChild(textArea);
        textArea.select();
        document.execCommand("copy");
        textArea.remove();
    }
};
