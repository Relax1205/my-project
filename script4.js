// Задание 4: Проверка пароля
const CORRECT_PASSWORD = "12345";
let userPassword = prompt("Введите пароль:");

if (userPassword === null || userPassword.trim() === "") {
  alert("Пароль не введён");
  console.log("Пароль не введён");
} else if (userPassword === CORRECT_PASSWORD) {
  alert("Доступ разрешен");
  console.log("Доступ разрешен");
} else {
  alert("Доступ запрещен");
  console.log("Доступ запрещен");
}