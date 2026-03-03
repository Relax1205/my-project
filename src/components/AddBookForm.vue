<template>
  <form @submit.prevent="handleSubmit">
    <h2 class="h4 mb-3">Добавить книгу</h2>
    
    <div class="mb-3">
      <input
        v-model="formData.title"
        type="text"
        class="form-control"
        placeholder="Название книги"
        required
      />
    </div>
    
    <div class="mb-3">
      <input
        v-model="formData.author"
        type="text"
        class="form-control"
        placeholder="Автор"
        required
      />
    </div>

    <div class="mb-3">
      <select v-model="formData.genre" class="form-select" required>
        <option value="" disabled>Выберите жанр</option>
        <option value="Роман">Роман</option>
        <option value="Фантастика">Фантастика</option>
        <option value="Детектив">Детектив</option>
        <option value="Научная">Научная</option>
        <option value="Поэзия">Поэзия</option>
      </select>
    </div>

    <div class="mb-3">
      <textarea
        v-model="formData.description"
        class="form-control"
        rows="3"
        placeholder="Краткое описание"
      ></textarea>
    </div>

    <div class="mb-3">
      <input
        v-model="formData.imageUrl"
        type="url"
        class="form-control"
        placeholder="URL обложки (https://...)"
      />
    </div>

    <button type="submit" class="btn btn-primary w-100">Добавить книгу</button>
  </form>
</template>

<script setup>
import { reactive } from 'vue'

const emit = defineEmits(['add-book'])

const formData = reactive({
  title: '',
  author: '',
  genre: '',
  description: '',
  imageUrl: ''
})

const handleSubmit = () => {
  emit('add-book', { ...formData })
  formData.title = ''
  formData.author = ''
  formData.genre = ''
  formData.description = ''
  formData.imageUrl = ''
}
</script>