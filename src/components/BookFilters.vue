<template>
  <div class="d-flex flex-wrap gap-3 align-items-center justify-content-between">
    <div class="search flex-grow-1" style="min-width: 200px;">
      <input
        v-model="searchQuery"
        type="text"
        class="form-control"
        placeholder="Поиск по названию или автору..."
      />
    </div>

    <div class="d-flex gap-2 flex-wrap">
      <select v-model="sortBy" class="form-select" style="width: auto;">
        <option value="date">По дате</option>
        <option value="title">По названию</option>
        <option value="rating">По рейтингу</option>
      </select>

      <div class="btn-group" role="group">
        <button
          v-for="option in filterOptions"
          :key="option.value"
          @click="$emit('update:filter', option.value)"
          class="btn btn-sm"
          :class="filter === option.value ? 'btn-dark' : 'btn-outline-secondary'"
        >
          {{ option.label }}
        </button>
      </div>
    </div>

    <div class="stats small text-muted">
      Всего: {{ total }} | Прочитано: {{ completed }}
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps(['filter', 'books', 'sortBy'])
const emit = defineEmits(['update:filter', 'update:sortBy'])

const searchQuery = defineModel('searchQuery')
const sortBy = defineModel('sortBy')

const filterOptions = [
  { value: 'all', label: 'Все' },
  { value: 'unread', label: 'Непрочитанные' },
  { value: 'read', label: 'Прочитанные' }
]

const total = computed(() => props.books.length)
const completed = computed(() => props.books.filter(b => b.completed).length)
</script>