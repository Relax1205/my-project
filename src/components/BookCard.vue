<template>
  <div class="card h-100 shadow-sm" :class="{ 'bg-light': book.completed }">
    <div class="row g-0 h-100">
      <div class="col-4" v-if="book.imageUrl">
        <img :src="book.imageUrl" class="img-fluid rounded-start h-100 object-fit-cover" alt="Обложка" style="object-fit: cover;">
      </div>
      <div :class="book.imageUrl ? 'col-8' : 'col-12'">
        <div class="card-body d-flex flex-column">
          <h5 class="card-title">{{ book.title }}</h5>
          <p class="card-text text-muted small">Автор: {{ book.author }}</p>
          <span class="badge bg-secondary mb-2 align-self-start">{{ book.genre }}</span>
          
          <p class="card-text small flex-grow-1" v-if="book.description">
            {{ book.description }}
          </p>

          <div class="mt-auto">
            <div v-if="book.completed" class="rating mb-2">
              <span
                v-for="star in 5"
                :key="star"
                @click="$emit('rate', star)"
                class="text-warning"
                style="cursor: pointer;"
              >
                {{ star <= book.rating ? '★' : '☆' }}
              </span>
            </div>
            
            <div class="btn-group w-100" role="group">
              <button
                @click="$emit('toggle')"
                class="btn btn-sm"
                :class="book.completed ? 'btn-outline-success' : 'btn-primary'"
              >
                {{ book.completed ? 'Прочитано' : 'Читать' }}
              </button>
              <button @click="$emit('delete')" class="btn btn-sm btn-outline-danger">✕</button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
defineProps(['book'])
defineEmits(['toggle', 'delete', 'rate'])
</script>

<style scoped>
.object-fit-cover {
  object-fit: cover;
}
</style>