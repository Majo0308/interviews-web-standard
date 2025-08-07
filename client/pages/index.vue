<template>
  <div class="pa-4">
    <!-- Header -->
    <div class="d-flex justify-space-between align-center mb-4">
      <h1 class="text-h5">Tasks</h1>
    </div>

    <!-- Task form to add a new task -->
    <TaskForm :tags="tags" :onSaved="reloadTasks" />

    <!-- Task list -->
    <v-list>
      <v-list-item
        v-for="task in filteredTasks"
        :key="`task-${task.taskItemId}`"
        class="pa-0"
      >
        <TaskCard :task="task" />
      </v-list-item>
    </v-list>
  </div>
</template>

<script setup lang="ts">
import { inject, ref, watch, onMounted, computed, type Ref } from 'vue'
import TaskForm from '~/components/TaskForm.vue'
import { fetchGetApi, fetchGetByIdApi } from '~/server/api'
import type { TagType, TaskType } from '~/server/api-schema'

// Inject selected tag ID (reactive)
const tagId = inject<Ref<number | undefined>>('selectedTag', ref(undefined))

// Inject available tags
const tags = inject<TagType[]>('tags', []) ?? []

// Reactive list of tasks
const tasks = ref<TaskType[]>([])

//Fetch tasks by tag ID from API and update the tasks list
const getTasksByTag = async (newTagId: number) => {
  try {
    const response=await fetchGetByIdApi<TaskType[]>(newTagId, "/Tasks/by-tags")
    tasks.value = response
  } catch (err: any) {
    console.error('Error loading tasks:', err.message)
  }
}
const getTasks = async () => {
  try {
    const response=await fetchGetApi<TaskType[]>("/Tasks")
    tasks.value = response
  } catch (err: any) {
    console.error('Error loading tasks:', err.message)
  }
}
//Reload tasks based on selected tag (defaults to tag ID 1)
const reloadTasks = async () => {
  const id = tagId.value || 1
  await getTasksByTag(id)
}

// Initial load
onMounted(() => {
  reloadTasks()
})

// Reload tasks when the selected tag changes
watch(tagId, (newTagId) => {
  if (newTagId != null && newTagId != 0) {
    getTasksByTag(newTagId)
  }
  else{
    getTasks();
  }
})

// Computed list of tasks (can apply filters in the future)
const filteredTasks = computed(() => tasks.value)
</script>

<style scoped>
.text-decoration-line-through {
  text-decoration: line-through;
}
</style>
