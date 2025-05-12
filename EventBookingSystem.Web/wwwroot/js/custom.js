// Custom JavaScript for Tickets Candy
document.addEventListener('DOMContentLoaded', function () {
    // Example: Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            e.preventDefault();
            document.querySelector(this.getAttribute('href')).scrollIntoView({
                behavior: 'smooth'
            });
        });
    });

    // Optional: Simple performance graph animation
    function animatePerformanceGraph() {
        const graph = document.querySelector('.performance-graph');
        if (graph) {
            // You could add more complex graph animation here
            graph.classList.add('animated');
        }
    }

    // Run animations on page load
    animatePerformanceGraph();
});