<script setup lang="ts">
import { ref, reactive, watch } from "vue";
import { useDisplay } from "vuetify";
import { fetchPostApi, fetchPutApi } from "~/server/api";
import type { TagType, TaskCreatedType, TaskType } from "~/server/api-schema";
import SubtasksForm from "./SubtaskForm.vue";

const { mdAndUp } = useDisplay();
const isMdAndUp = mdAndUp;

const sheetOpen = ref(false);
const showSubtasksForm = ref(false);

const props = defineProps<{
  tags: TagType[];
  onSaved?: () => void;
  modelValue?: TaskCreatedType | null;
  isEdit?: boolean;
}>();

watch(
  () => props.modelValue,
  (newTask) => {
    if (newTask) {
      loadTask(newTask);
      sheetOpen.value = true;
      showSubtasksForm.value = newTask.subtasks?.length > 0;
    }
  },
  { immediate: true }
);

const priorityOptions = [
  { priority: 1, priorityLabel: "Normal" },
  { priority: 2, priorityLabel: "Urgent" },
];

const subtaskStates = [
  { subtaskStateId: 1, stateName: "Pending" },
  { subtaskStateId: 2, stateName: "In Progress" },
  { subtaskStateId: 3, stateName: "Completed" },
];

const onSubmit = async () => {
  try {
    if (props.isEdit && task.taskItemId) {
      await fetchPutApi(task.taskItemId, "/Tasks", task);
    } else {
      await fetchPostApi<TaskType>("/Tasks", task);
    }
    props.onSaved?.();
    closeForm();
  } catch (err) {
    console.error("Error saving task:", err);
  }
};

const closeForm = () => {
  sheetOpen.value = false;
  resetForm();
};

const cancelTask = () => {
  closeForm();
};

const resetForm = () => {
  Object.assign(task, resetTask());
  showSubtasksForm.value = false;
};

const resetTask = (): TaskCreatedType => {
  return {
    taskItemId: 0,
    taskName: "",
    taskDescription: "",
    taskCompleted: false,
    taskDueDate: new Date().toISOString().slice(0, 10),
    taskPriority: 1,
    tags: [],
    subtasks: [],
  };
};

const loadTask = (newTask: TaskCreatedType) => {
  Object.assign(task, {
    ...newTask,
    taskDueDate: newTask.taskDueDate?.slice(0, 10),
  });
};

const toggleSubtasksForm = () => {
  showSubtasksForm.value = !showSubtasksForm.value;
};

const task = reactive<TaskCreatedType>(resetTask());
</script>

<template>
  <div>
    <component
      :is="!isMdAndUp || task.taskItemId != 0 ? 'v-dialog' : 'v-bottom-sheet'"
      v-model="sheetOpen"
      :max-width="!isMdAndUp || task.taskItemId != 0 ? 1200 : undefined"
      persistent
    >
      <template #activator="{ props: activatorProps }">
        <v-btn v-bind="activatorProps" color="primary" text="Add Task" />
      </template>

      <v-card>
        <v-card-text class="pb-0">
          <h1 class="text-h6">Task Form</h1>

          <!-- Main Fields -->
          <task-basic-fields
            v-model="task"
            :tags="tags"
            @save="onSubmit"
            @toggle-subtasks="toggleSubtasksForm"
            @cancel="cancelTask"
          />

          <!-- Secondary Fields -->
          <v-expand-transition>
            <subtasks-form
              v-if="showSubtasksForm"
              v-model="task"
              :priority-options="priorityOptions"
              :subtask-states="subtaskStates"
            />
          </v-expand-transition>
        </v-card-text>
      </v-card>
    </component>
  </div>
</template>
