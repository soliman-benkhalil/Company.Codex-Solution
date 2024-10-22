document.addEventListener('DOMContentLoaded', () => {
    const cards = document.querySelectorAll('.card');

    cards.forEach(card => {
        card.addEventListener('click', () => {
            const content = card.querySelector('.card-content');
            if (content.style.maxHeight) {
                content.style.maxHeight = null;  // Collapse the card
            } else {
                content.style.maxHeight = content.scrollHeight + 'px';  // Expand the card
            }
        });
    });
});
