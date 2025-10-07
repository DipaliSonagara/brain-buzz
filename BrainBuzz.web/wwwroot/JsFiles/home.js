namespace BrainBuzz.web.wwwroot.JsFiles
{
    // home.js - JavaScript functions for Home page
    window.initializeHomePage = () => {
        // Smooth scrolling for navigation links
        document.querySelectorAll('a[href^="#"]').forEach(anchor => {
            anchor.addEventListener('click', function (e) {
                e.preventDefault();
                const target = document.querySelector(this.getAttribute('href'));
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            });
        });

        // Animate elements on scroll
        const observerOptions = {
            threshold: 0.1,
            rootMargin: '0px 0px -50px 0px'
        };

        const observer = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    entry.target.classList.add('animated');
                }
            });
        }, observerOptions);

        // Observe all elements with animate-on-scroll class
        document.querySelectorAll('.animate-on-scroll').forEach(el => {
            observer.observe(el);
        });

        // Counter animation for stats
        function animateCounter(element, target, duration = 2000) {
            let start = 0;
            let increment = target / (duration / 16);

            function updateCounter() {
                start += increment;
                if (start < target) {
                    if (target >= 1000000) {
                        element.textContent = (Math.floor(start / 1000000 * 10) / 10) + 'M+';
                    } else if (target >= 1000) {
                        element.textContent = Math.floor(start / 1000) + 'K+';
                    } else {
                        element.textContent = Math.floor(start) + '+';
                    }
                    requestAnimationFrame(updateCounter);
                } else {
                    if (target >= 1000000) {
                        element.textContent = (target / 1000000) + 'M+';
                    } else if (target >= 1000) {
                        element.textContent = (target / 1000) + 'K+';
                    } else {
                        element.textContent = target + '+';
                    }
                }
            }
            updateCounter();
        }

        // Animate stats when they come into view
        const statsObserver = new IntersectionObserver((entries) => {
            entries.forEach(entry => {
                if (entry.isIntersecting) {
                    const counterElement = entry.target.querySelector('.counter');
                    if (counterElement) {
                        const targetValue = parseInt(counterElement.getAttribute('data-target'));
                        counterElement.textContent = '0+';
                        animateCounter(counterElement, targetValue);
                        statsObserver.unobserve(entry.target);
                    }
                }
            });
        }, { threshold: 0.5 });

        document.querySelectorAll('.stat-item').forEach(item => {
            statsObserver.observe(item);
        });

        // Add click event listeners for category cards
        document.querySelectorAll('.category-card').forEach(card => {
            card.addEventListener('mouseenter', function () {
                this.style.transform = 'translateY(-5px)';
            });

            card.addEventListener('mouseleave', function () {
                this.style.transform = 'translateY(-3px)';
            });
        });
    };
}
