// Задание 5: Простой калькулятор
let num1 = Number(prompt("Введите первое число:"));
let num2 = Number(prompt("Введите второе число:"));
let operator = prompt("Введите оператор (+, -, *, /):");

let result;

if (isNaN(num1) || isNaN(num2)) {
  alert("Ошибка: введены некорректные числа");
  console.log("Ошибка: введены некорректные числа");
} else if (operator === "+") {
  result = num1 + num2;
} else if (operator === "-") {
  result = num1 - num2;
} else if (operator === "*") {
  result = num1 * num2;
} else if (operator === "/") {
  if (num2 === 0) {
    alert("На ноль делить нельзя");
    console.log("На ноль делить нельзя");
  } else {
    result = num1 / num2;
  }
} else {
  alert("Неверный оператор");
  console.log("Неверный оператор");
}

if (result !== undefined) {
  let output = `${num1} ${operator} ${num2} = ${result}`;
  alert(output);
  console.log(output);
}