document.addEventListener('DOMContentLoaded', function() {
  const textElements = document.querySelectorAll('.text');
  console.log('Найдено элементов с классом text:', textElements.length);
  
  const specialElement = document.querySelector('.special');
  if (specialElement) {
    specialElement.classList.add('red-text');
  }
  
  if (textElements.length >= 3) {
    textElements[2].classList.add('bg-green');
  }
  
  const container = document.getElementById('container');
  if (container) {
    container.classList.add('border-container');
  }
});