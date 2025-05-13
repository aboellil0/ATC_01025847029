window.backgroundAnimation = {
    initialize: function () {
        const container = document.querySelector('.tickets-candy-container');
        const elements = document.querySelectorAll('.dot, .triangle, .lines');
        let bounds = container.getBoundingClientRect();

        function handleMouseMove(e) {
            const mouseX = e.clientX - bounds.left;
            const mouseY = e.clientY - bounds.top;

            elements.forEach(element => {
                const elementBounds = element.getBoundingClientRect();
                const elementX = elementBounds.left - bounds.left + elementBounds.width / 2;
                const elementY = elementBounds.top - bounds.top + elementBounds.height / 2;

                const distanceX = mouseX - elementX;
                const distanceY = mouseY - elementY;
                const distance = Math.sqrt(distanceX * distanceX + distanceY * distanceY);
                const maxDistance = 300;

                if (distance < maxDistance) {
                    const power = (1 - distance / maxDistance) * 30;
                    const moveX = (distanceX / distance) * power;
                    const moveY = (distanceY / distance) * power;

                    element.style.transform = `translate(${-moveX}px, ${-moveY}px)`;
                } else {
                    element.style.transform = 'translate(0, 0)';
                }
            });
        }

        function updateBounds() {
            bounds = container.getBoundingClientRect();
        }

        container.addEventListener('mousemove', handleMouseMove);
        window.addEventListener('resize', updateBounds);
    }
};