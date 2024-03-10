function getElementPosition(selector) {
    const element = document.querySelector(selector);
    if (!element) return null;

    const rect = element.getBoundingClientRect();
    return {
        top: rect.top + window.scrollY,
        left: rect.left + window.scrollX,
    };
}

function scrollToMidpoint(selector1, selector2) {
    const position1 = getElementPosition(selector1);
    const position2 = getElementPosition(selector2);

    if (!position1 || !position2) return;

    const midpointX = (position1.left + position2.left) / 2;
    const midpointY = (position1.top + position2.top) / 2;

    // Adjust the scroll position to center the midpoint
    window.scrollTo({
        top: midpointY - window.innerHeight / 2,
        left: midpointX - window.innerWidth / 2,
        behavior: 'smooth' // Optional: Add smooth scrolling effect
    });
}

// Example: Call the function with the CSS selectors of your elements
//scrollToMidpoint('#tree > ul > li > ul > li:first-child', '#tree > ul > li > ul > li:last-child');