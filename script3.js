// Задание 3: Угадай число
let secretNumber = Math.floor(Math.random() * 10) + 1;
let userGuess;

console.log("Загадано число от 1 до 10 (для проверки: " + secretNumber + ")");

while (true) {
  userGuess = prompt("Угадайте число от 1 до 10:");
  
  if (userGuess === null || userGuess.trim() === "") {
    alert("Игра отменена");
    break;
  }
  
  userGuess = Number(userGuess);
  
  if (isNaN(userGuess)) {
    alert("Введите корректное число!");
    continue;
  }
  
  if (userGuess === secretNumber) {
    alert("Поздравляем! Вы угадали число!");
    console.log("Поздравляем! Вы угадали число!");
    break;
  } else if (userGuess < secretNumber) {
    alert("Загаданное число больше");
  } else {
    alert("Загаданное число меньше");
  }
}