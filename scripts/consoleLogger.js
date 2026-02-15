// consoleLogger.js
document.addEventListener('DOMContentLoaded', function() {
  // Слушаем кастомное событие formValid
  document.addEventListener('formValid', function(event) {
    // Получаем данные формы из события
    const formData = event.detail;

    // Очищаем консоль для наглядности
    console.clear();

    console.log('========== ОТЗЫВ ОТПРАВЛЕН ==========');
    console.log('ФИО:', formData.fullname);
    console.log('Телефон:', formData.phone);
    console.log('Email:', formData.email);
    console.log('Сообщение:', formData.message);
    
    // Вывод временной метки
    const timestamp = new Date().toLocaleString();
    console.log('Время отправки:', timestamp);
    console.log('======================================');
  });
});