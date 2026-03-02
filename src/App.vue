<template>
  <div class="app">
    <header>
      <h1>Менеджер книг</h1>
      <p>Управляй своей библиотекой</p>
    </header>
    <main>
      <AddBookForm @add-book="addBook" />
      <BookFilters
        v-model:searchQuery="searchQuery"
        v-model:filter="currentFilter"
        :books="books"
      />
      <div v-if="filteredBooks.length === 0" class="empty-state">
        <p>Книги не найдены:(</p>
        <p>Добавьте первую книгу или измените параметры поиска</p>
      </div>
      <div v-else class="books-list">
        <BookCard
          v-for="book in filteredBooks"
          :key="book.id"
          :book="book"
          @toggle="toggleBook(book.id)"
          @delete="deleteBook(book.id)"
          @rate="rateBook(book.id, $event)"
        />
      </div>
    </main>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'
import AddBookForm from './components/AddBookForm.vue'
import BookFilters from './components/BookFilters.vue'
import BookCard from './components/BookCard.vue'

// Состояние книг с загрузкой из localStorage
const books = ref([])

// Загрузка сохраненных книг
const savedBooks = localStorage.getItem('books')
if (savedBooks) {
  books.value = JSON.parse(savedBooks)
}

// Состояния фильтрации
const currentFilter = ref('all')
const searchQuery = ref('')

// Сохранение изменений в localStorage
watch(
  books,
  (newBooks) => {
    localStorage.setItem('books', JSON.stringify(newBooks))
  },
  { deep: true }
)

// Добавление книги
const addBook = (bookData) => {
  const newBook = {
    id: Date.now(),
    ...bookData,
    completed: false,
    rating: 0
  }
  books.value.push(newBook)
}

// Переключение статуса
const toggleBook = (id) => {
  const book = books.value.find((b) => b.id === id)
  if (book) {
    book.completed = !book.completed
    if (!book.completed) {
      book.rating = 0
    }
  }
}

// Оценка книги
const rateBook = (id, rating) => {
  const book = books.value.find((b) => b.id === id)
  if (book && book.completed) {
    book.rating = rating
  }
}

// Удаление книги
const deleteBook = (id) => {
  if (confirm('Удалить книгу?')) {
    books.value = books.value.filter((b) => b.id !== id)
  }
}

// Фильтрация и поиск книг
const filteredBooks = computed(() => {
  return books.value
    .filter((book) => {
      if (currentFilter.value === 'unread') return !book.completed
      if (currentFilter.value === 'read') return book.completed
      return true
    })
    .filter((book) => {
      if (!searchQuery.value) return true
      const query = searchQuery.value.toLowerCase()
      return (
        book.title.toLowerCase().includes(query) ||
        book.author.toLowerCase().includes(query)
      )
    })
})
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}
body {
  font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
  background: #f0f2f5;
  line-height: 1.6;
}
.app {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}
header {
  text-align: center;
  margin-bottom: 30px;
  padding: 20px;
  background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
  color: white;
  border-radius: 10px;
  box-shadow: 0 4px 6px rgba(0,0,0,0.1);
}
header h1 {
  font-size: 2.5em;
  margin-bottom: 5px;
}
main {
  background: white;
  padding: 30px;
  border-radius: 10px;
  box-shadow: 0 2px 10px rgba(0,0,0,0.1);
}
.empty-state {
  text-align: center;
  padding: 40px;
  color: #999;
  font-size: 1.2em;
}
.empty-state p:first-child {
  font-size: 3em;
  margin-bottom: 20px;
}
.books-list {
  margin-top: 20px;
}
</style>