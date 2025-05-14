window.onload = function () {
    // Get all background elements
    const dots = document.querySelectorAll('.dot');
    const triangle = document.querySelector('.triangle');
    const lines = document.querySelector('.lines');
    let animationsActive = true;

    // Initial animation
    floatingAnimation();

    // Mouse movement interaction
    document.addEventListener('mousemove', function (e) {
        if (!animationsActive) return;

        const mouseX = e.clientX;
        const mouseY = e.clientY;

        dots.forEach((dot, index) => {
            // Different movement factor for each dot
            const factor = 0.02 + (index * 0.01);
            const dotRect = dot.getBoundingClientRect();
            const dotCenterX = dotRect.left + dotRect.width / 2;
            const dotCenterY = dotRect.top + dotRect.height / 2;

            // Calculate distance and direction
            const deltaX = mouseX - dotCenterX;
            const deltaY = mouseY - dotCenterY;

            // Apply subtle movement based on mouse position
            const translateX = deltaX * factor;
            const translateY = deltaY * factor;

            // Apply the transformation
            dot.style.transform = `translate(${translateX}px, ${translateY}px)`;
        });

        // Make triangle rotate slightly based on mouse position
        if (triangle) {
            const triangleRect = triangle.getBoundingClientRect();
            const triangleCenterX = triangleRect.left + triangleRect.width / 2;
            const triangleCenterY = triangleRect.top + triangleRect.height / 2;

            const angle = Math.atan2(mouseY - triangleCenterY, mouseX - triangleCenterX);
            const rotation = angle * (180 / Math.PI);

            triangle.style.transform = `rotate(${rotation}deg)`;
        }

        // Make lines element pulse/rotate
        if (lines) {
            const linesRect = lines.getBoundingClientRect();
            const linesCenterX = linesRect.left + linesRect.width / 2;
            const linesCenterY = linesRect.top + linesRect.height / 2;

            const angle = Math.atan2(mouseY - linesCenterY, mouseX - linesCenterX);
            const distance = Math.sqrt(Math.pow(mouseX - linesCenterX, 2) + Math.pow(mouseY - linesCenterY, 2));

            const rotation = angle * (180 / Math.PI);
            const scale = 1 + Math.min(0.2, distance / 5000);

            lines.style.transform = `rotate(${rotation}deg) scale(${scale})`;
        }
    });

    // Click interaction for all elements
    document.querySelectorAll('.dot, .triangle, .lines').forEach(element => {
        element.addEventListener('click', function () {
            // Create "pop" effect
            this.style.transition = 'transform 0.3s cubic-bezier(0.175, 0.885, 0.32, 1.275)';
            this.style.transform = 'scale(1.5)';

            setTimeout(() => {
                this.style.transform = '';
            }, 300);
        });
    });

    // Floating animation function
    function floatingAnimation() {
        dots.forEach((dot, index) => {
            const delay = index * 0.5;
            const duration = 3 + index * 0.7;

            dot.style.animation = `floating ${duration}s ease-in-out ${delay}s infinite alternate`;
        });

        if (triangle) {
            triangle.style.animation = 'rotateFloat 7s ease-in-out infinite alternate';
        }

        if (lines) {
            lines.style.animation = 'pulseRotate 8s ease-in-out infinite alternate';
        }
    }

    //// Button controls
    //document.getElementById('toggleAnimation').addEventListener('click', function () {
    //    animationsActive = !animationsActive;

    //    if (animationsActive) {
    //        floatingAnimation();
    //        this.textContent = 'Toggle Animations';
    //    } else {
    //        dots.forEach(dot => { dot.style.animation = 'none'; });
    //        if (triangle) triangle.style.animation = 'none';
    //        if (lines) lines.style.animation = 'none';
    //        this.textContent = 'Enable Animations';
    //    }
    //});

    //document.getElementById('resetPositions').addEventListener('click', function () {
    //    dots.forEach(dot => {
    //        dot.style.transform = 'none';
    //    });
    //    if (triangle) triangle.style.transform = 'rotate(-30deg)';
    //    if (lines) lines.style.transform = 'none';
    //});
};