<template>
  <div class="container py-5">
    <header class="text-center mb-5">
      <h1 class="display-4 fw-bold text-primary">Менеджер книг</h1>
      <p class="lead text-muted">Управляй своей библиотекой с помощью Vue 3 + Bootstrap</p>
    </header>

    <main class="row g-4">
      <div class="col-lg-4">
        <div class="card shadow-sm">
          <div class="card-body">
            <AddBookForm @add-book="addBook" />
          </div>
        </div>
      </div>

      <div class="col-lg-8">
        <div class="card shadow-sm mb-4">
          <div class="card-body">
            <BookFilters
              v-model:searchQuery="searchQuery"
              v-model:filter="currentFilter"
              v-model:sortBy="sortBy"
              :books="books"
            />
          </div>
        </div>

        <div v-if="filteredBooks.length === 0" class="alert alert-info text-center">
          <p class="mb-0">Книги не найдены :(</p>
          <small>Добавьте первую книгу или измените параметры поиска</small>
        </div>

        <div v-else class="row g-3">
          <div class="col-md-6" v-for="book in filteredBooks" :key="book.id">
            <BookCard
              :book="book"
              @toggle="toggleBook(book.id)"
              @delete="deleteBook(book.id)"
              @rate="rateBook(book.id, $event)"
            />
          </div>
        </div>
      </div>
    </main>
  </div>
</template>

<script setup>
import { ref, computed, watch } from 'vue'
import AddBookForm from './components/AddBookForm.vue'
import BookFilters from './components/BookFilters.vue'
import BookCard from './components/BookCard.vue'

const books = ref([])

const savedBooks = localStorage.getItem('books')
if (savedBooks) {
  books.value = JSON.parse(savedBooks)
}

const currentFilter = ref('all')
const searchQuery = ref('')
const sortBy = ref('date')

watch(
  books,
  (newBooks) => {
    localStorage.setItem('books', JSON.stringify(newBooks))
  },
  { deep: true }
)

const addBook = (bookData) => {
  const newBook = {
    id: Date.now(),
    ...bookData,
    completed: false,
    rating: 0,
    dateAdded: new Date().toISOString()
  }
  books.value.push(newBook)
}

const toggleBook = (id) => {
  const book = books.value.find((b) => b.id === id)
  if (book) {
    book.completed = !book.completed
    if (!book.completed) {
      book.rating = 0
    }
  }
}

const rateBook = (id, rating) => {
  const book = books.value.find((b) => b.id === id)
  if (book && book.completed) {
    book.rating = rating
  }
}

const deleteBook = (id) => {
  if (confirm('Удалить книгу?')) {
    books.value = books.value.filter((b) => b.id !== id)
  }
}

const filteredBooks = computed(() => {
  let result = books.value.filter((book) => {
    if (currentFilter.value === 'unread') return !book.completed
    if (currentFilter.value === 'read') return book.completed
    return true
  })

  if (searchQuery.value) {
    const query = searchQuery.value.toLowerCase()
    result = result.filter((book) =>
      book.title.toLowerCase().includes(query) ||
      book.author.toLowerCase().includes(query)
    )
  }

  return result.sort((a, b) => {
    if (sortBy.value === 'title') {
      return a.title.localeCompare(b.title)
    } else if (sortBy.value === 'rating') {
      return b.rating - a.rating
    } else {
      return new Date(b.dateAdded) - new Date(a.dateAdded)
    }
  })
})
</script>

<style>
body {
  background-color: #f8f9fa;
}
</style>