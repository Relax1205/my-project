<template>
  <form @submit.prevent="handleSubmit" class="add-form">
    <h2>Добавить новую книгу</h2>
    <div class="form-group">
      <input
        v-model="formData.title"
        type="text"
        placeholder="Название книги"
        required
      />
    </div>
    <div class="form-group">
      <input
        v-model="formData.author"
        type="text"
        placeholder="Автор"
        required
      />
    </div>
    <div class="form-group">
      <select v-model="formData.genre" required>
        <option value="">Выберите жанр</option>
        <option value="Роман">Роман</option>
        <option value="Фантастика">Фантастика</option>
        <option value="Детектив">Детектив</option>
        <option value="Научная">Научная</option>
        <option value="Поэзия">Поэзия</option>
      </select>
    </div>
    <button type="submit" class="btn-submit">Добавить книгу</button>
  </form>
</template>

<script setup>
import { reactive } from 'vue'

const emit = defineEmits(['add-book'])

const formData = reactive({
  title: '',
  author: '',
  genre: ''
})

const handleSubmit = () => {
  emit('add-book', { ...formData })
  // Очистка формы
  formData.title = ''
  formData.author = ''
  formData.genre = ''
}
</script>

<style scoped>
.add-form {
  background: white;
  padding: 20px;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0,0,0,0.1);
  margin-bottom: 20px;
}
.add-form h2 {
  margin-bottom: 15px;
  color: #333;
}
.form-group {
  margin-bottom: 15px;
}
.form-group input,
.form-group select {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1em;
}
.form-group input:focus,
.form-group select:focus {
  outline: none;
  border-color: #4CAF50;
}
.btn-submit {
  width: 100%;
  padding: 12px;
  background: #4CAF50;
  color: white;
  border: none;
  border-radius: 4px;
  font-size: 1em;
  cursor: pointer;
  transition: background 0.3s;
}
.btn-submit:hover {
  background: #45a049;
}
</style>