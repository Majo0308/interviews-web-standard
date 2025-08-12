<script setup lang="ts">
import { inject, ref, watch, onMounted, type Ref } from "vue";
import TaskForm from "~/components/TaskForm.vue";
import TaskCard from "~/components/TaskCard.vue";
import type { TagType, TaskCreatedType, TaskType } from "~/server/api-schema";
import { useTasks } from "~/composables/useTasks";

const { completedTasks, incompletedTasks, loadTasks } = useTasks();

const showCompleted = ref(true);
const showIncompleted = ref(true);

const editingTask = ref<TaskCreatedType | null>(null);
const isEditing = ref(false);

const tagId = inject<Ref<number | undefined>>("selectedTag", ref(undefined));
const tags = inject<TagType[]>("tags", []) ?? [];

const toggleCompleted = () => (showCompleted.value = !showCompleted.value);
const toggleIncompleted = () =>
  (showIncompleted.value = !showIncompleted.value);

const startEditing = (task: TaskType) => {
  editingTask.value = { ...task, tags: task.tags.map((t) => t.tagId) };
  isEditing.value = true;
};
const reloadTasks = () => loadTasks(tagId.value || 0);

watch(tagId, reloadTasks);
onMounted(reloadTasks);
</script>
<template>
  <div class="pa-4">
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

    <!-- Incompleted tasks -->
    <v-card
      class="ma-2 mb-1"
      @click="toggleIncompleted"
      style="background-color: #f2f2f2"
    >
      <v-card-text>
        <div style="display: flex; justify-content: space-between">
          <div>INCOMPLETED</div>

          <v-icon size="small">
            {{ showIncompleted ? "mdi-chevron-up" : "mdi-chevron-down" }}
          </v-icon>
        </div>
      </v-card-text>
    </v-card>
    <v-list v-show="showIncompleted">
      <v-list-item
        v-for="task in incompletedTasks"
        :key="task.taskItemId"
        class="pa-0"
      >
        <TaskCard :task="task" @edit="startEditing" :getTasks="reloadTasks" />
      </v-list-item>
    </v-list>

    <!-- Completed tasks -->
    <v-card
      style="background-color: #f2f2f2"
      class="ma-2"
      @click="toggleCompleted"
    >
      <v-card-text>
        <div style="display: flex; justify-content: space-between">
          <div>COMPLETED</div>
          <v-icon size="small">
            {{ showCompleted ? "mdi-chevron-up" : "mdi-chevron-down" }}
          </v-icon>
        </div>
      </v-card-text>
    </v-card>
    <v-list v-show="showCompleted">
      <v-list-item
        v-for="task in completedTasks"
        :key="task.taskItemId"
        class="pa-0"
      >
        <TaskCard :task="task" @edit="startEditing" :getTasks="reloadTasks" />
      </v-list-item>
    </v-list>
  </div>
</template>
