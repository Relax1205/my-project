document.addEventListener('DOMContentLoaded', function() {
  const app = document.getElementById('app');
  
  const title = document.createElement('h1');
  title.textContent = 'Список пользователей';
  app.appendChild(title);
  
  const userList = document.createElement('ul');
  const users = ['Анна', 'Борис', 'Виктор'];
  
  users.forEach(userName => {
    const li = document.createElement('li');
    li.textContent = userName;
    userList.appendChild(li);
  });
  
  app.appendChild(userList);
  
  const addButton = document.createElement('button');
  addButton.textContent = 'Добавить пользователя';
  app.appendChild(addButton);
  
  addButton.addEventListener('click', function() {
    const newLi = document.createElement('li');
    newLi.textContent = 'Новый пользователь';
    userList.appendChild(newLi);
  });
});