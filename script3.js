document.addEventListener('DOMContentLoaded', function() {
  const items = document.querySelectorAll('.item');
  
  const activeItem = document.querySelector('.item.active');
  if (activeItem) {
    activeItem.classList.add('highlight');
  }
  
  let totalPrice = 0;
  let maxPrice = -Infinity;
  let mostExpensiveItem = null;
  
  items.forEach(item => {
    const price = Number(item.dataset.price);
    totalPrice += price;
    
    if (price > maxPrice) {
      maxPrice = price;
      mostExpensiveItem = item;
    }
  });
  
  const totalElement = document.getElementById('total');
  totalElement.textContent = `Общая стоимость: ${totalPrice} руб.`;
  
  if (mostExpensiveItem) {
    console.log(`Самый дорогой товар: ${mostExpensiveItem.textContent} (${maxPrice} руб.)`);
  }
});