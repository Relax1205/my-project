// Задание 2: Проверка совершеннолетия
let userName = prompt("Введите ваше имя:");
let userAge = Number(prompt("Введите ваш возраст:"));
let message = `Привет, ${userName}! Тебе ${userAge} лет.`;
alert(message);
console.log(message);

if (userAge >= 18) {
  alert("Вы совершеннолетний");
  console.log("Вы совершеннолетний");
} else {
  alert("Вы несовершеннолетний");
  console.log("Вы несовершеннолетний");
}