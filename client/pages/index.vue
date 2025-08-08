<template>
  <div class="pa-4">
    <!-- Header section with title and task form -->
    <div class="d-flex justify-space-between align-center mb-4">
      <h1 class="text-h5">Tasks</h1>
      <!-- TaskForm component to add or edit tasks -->
      <TaskForm
        :tags="tags"
        :onSaved="reloadTasks"
        v-model="editingTask"
        :isEdit="isEditing"
      />
    </div>

    <!-- Toggleable card to show/hide incompleted tasks -->
    <v-card
      class="ma-2 mb-1"
      @click="toggleIncompleted"
      style="background-color: #f2f2f2"
    >
      <v-card-text>
        <div style="display: flex; justify-content: space-between">
          <div>INCOMPLETED</div>

          <!-- Icon indicates whether the section is expanded or collapsed -->
          <v-icon size="small">
            {{ showIncompleted ? "mdi-chevron-up" : "mdi-chevron-down" }}
          </v-icon>
        </div>
      </v-card-text>
    </v-card>
    <!-- List of incompleted tasks, visible only when showIncompleted is true -->
    <v-list v-show="showIncompleted">
      <v-list-item
        v-for="task in incompletedTasks"
        :key="task.taskItemId"
        class="pa-0"
      >
        <!-- TaskCard component for each incompleted task -->
        <TaskCard :task="task" @edit="startEditing" :getTasks="reloadTasks" />
      </v-list-item>
    </v-list>

    <!-- Toggleable card to show/hide completed tasks -->
    <v-card
      style="background-color: #f2f2f2"
      class="ma-2"
      @click="toggleCompleted"
    >
      <v-card-text>
        <div style="display: flex; justify-content: space-between">
          <div>COMPLETED</div>

          <!-- Icon indicates whether the section is expanded or collapsed -->
          <v-icon size="small">
            {{ showCompleted ? "mdi-chevron-up" : "mdi-chevron-down" }}
          </v-icon>
        </div>
      </v-card-text>
    </v-card>
    <!-- List of completed tasks, visible only when showCompleted is true -->
    <v-list v-show="showCompleted">
      <v-list-item
        v-for="task in completedTasks"
        :key="task.taskItemId"
        class="pa-0"
      >
        <!-- TaskCard component for each completed task -->
        <TaskCard :task="task" @edit="startEditing" :getTasks="reloadTasks" />
      </v-list-item>
    </v-list>
  </div>
</template>

<script setup lang="ts">
import { inject, ref, watch, onMounted, type Ref } from "vue";
import TaskForm from "~/components/TaskForm.vue";
import TaskCard from "~/components/TaskCard.vue";
import type { TagType, TaskCreatedType, TaskType } from "~/server/api-schema";
import { useTasks } from "~/composables/useTasks";

// Composable that manages loading tasks and separating completed/incompleted tasks
const { completedTasks, incompletedTasks, loadTasks } = useTasks();

// Reactive states to control visibility of completed and incompleted task lists
const showCompleted = ref(true);
const showIncompleted = ref(true);

// Holds the task currently being edited; null if no edit in progress
const editingTask = ref<TaskCreatedType | null>(null);
const isEditing = ref(false);

// Inject the selected tag ID and the list of tags from the parent or layout
const tagId = inject<Ref<number | undefined>>("selectedTag", ref(undefined));
const tags = inject<TagType[]>("tags", []) ?? [];

// Function to reload tasks based on the currently selected tag
const reloadTasks = () => loadTasks(tagId.value || 0);

// Toggle functions for collapsing/expanding task lists
const toggleCompleted = () => (showCompleted.value = !showCompleted.value);
const toggleIncompleted = () =>
  (showIncompleted.value = !showIncompleted.value);

// When editing a task, prepare a copy with tag IDs for the form model
const startEditing = (task: TaskType) => {
  editingTask.value = { ...task, tags: task.tags.map((t) => t.tagId) };
  isEditing.value = true;
};

// Watch the selected tag ID and reload tasks when it changes
watch(tagId, reloadTasks);

// Load tasks when component is mounted
onMounted(reloadTasks);
</script>
