window.drawBracketLines = () => {
    const svg = document.querySelector(".connector-lines");
    if (!svg) return;

    svg.innerHTML = "";

    const wrappers = document.querySelectorAll(".match-wrapper");

    wrappers.forEach(wrapper => {
        const fromCard = wrapper.querySelector(".bracket-card");
        const nextId = wrapper.dataset.next;
        if (!nextId) return;

        const toCard = document.getElementById(nextId);
        if (!toCard) return;

        const fromRect = fromCard.getBoundingClientRect();
        const toRect = toCard.getBoundingClientRect();
        const svgRect = svg.getBoundingClientRect();

        const fromCenterY = fromRect.top + fromRect.height / 2 - svgRect.top;
        const toCenterY = toRect.top + toRect.height / 2 - svgRect.top;

        const fromX = fromRect.right - svgRect.left;
        const toX = toRect.left - svgRect.left;

        // junction X between columns (midpoint)
        const junctionX = (fromX + toX) / 2;

        const h1 = document.createElementNS("http://www.w3.org/2000/svg", "line");
        const v = document.createElementNS("http://www.w3.org/2000/svg", "line");
        const h2 = document.createElementNS("http://www.w3.org/2000/svg", "line");

        h1.setAttribute("class", "h-line");
        v.setAttribute("class", "v-line");
        h2.setAttribute("class", "h-line");

        // first horizontal: out of semifinal card
        h1.setAttribute("x1", fromX);
        h1.setAttribute("y1", fromCenterY);
        h1.setAttribute("x2", junctionX);
        h1.setAttribute("y2", fromCenterY);

        // vertical: up/down to final center
        v.setAttribute("x1", junctionX);
        v.setAttribute("y1", fromCenterY);
        v.setAttribute("x2", junctionX);
        v.setAttribute("y2", toCenterY);

        // second horizontal: into final card
        h2.setAttribute("x1", junctionX);
        h2.setAttribute("y1", toCenterY);
        h2.setAttribute("x2", toX);
        h2.setAttribute("y2", toCenterY);

        svg.appendChild(h1);
        svg.appendChild(v);
        svg.appendChild(h2);
    });
};
