<template>
  <v-bottom-sheet v-model="sheetOpen">
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn v-bind="activatorProps" color="primary" text="Add Task" />
    </template>

    <v-card>
      <v-card-text class="pb-0">
        <h1 class="text-h6">Task Form</h1>
        <v-row class="my-0 py-0" dense>
          <v-col>
            <v-text-field
              v-model="task.taskName"
              label="Title"
              outlined
              dense
            />
          </v-col>

          <v-col cols="12" sm="2">
            <v-select
              v-model="task.tags"
              :items="tags"
              item-title="tagName"
              item-value="tagId"
              label="Tags"
              multiple
              chips
              outlined
              dense
            />
          </v-col>

          <v-col cols="12" sm="2">
            <v-date-input v-model="task.taskDueDate" label="Due date" />
          </v-col>
          <v-col cols="auto">
            <v-btn
              icon="mdi-send"
              size="small"
              color="success"
              class="mt-2"
              @click="sendTask"
            />
          </v-col>
          <v-col cols="auto">
            <v-btn
              icon="mdi-format-list-group-plus"
              size="small"
              color="primary"
              class="mt-2"
              @click="toggleSubtasksForm"
            />
          </v-col>
          <v-col cols="auto">
            <v-btn
              icon="mdi-close"
              size="small"
              color="error"
              class="mt-2"
              @click="cancelTask"
            />
          </v-col>
        </v-row>

        <!-- Subtasks form -->
        <v-expand-transition>
          <div v-if="showSubtasksForm" class="">
            <v-row class="my-0 py-0" dense>
              <v-col>
                <v-text-field
                  v-model="task.taskDescription"
                  label="Description"
                  outlined
                  dense
                />
              </v-col>
              <v-col cols="12" sm="2">
                <v-select
                  v-model="task.taskPriority"
                  :items="priorityOptions"
                  item-value="priority"
                  item-title="priorityLabel"
                  label="Priority"
                  outlined
                  dense
                />
              </v-col>
            </v-row>
            <div style="display: flex">
              <h1 class="text-h6 mt-1 mr-2 mb-3">Subtask Form</h1>
              <v-btn
                icon="mdi-plus"
                size="small"
                color="primary"
                @click="addSubtask"
              ></v-btn>
            </div>

            <div
              v-for="(subtask, index) in task.subtasks"
              :key="index"
              class="mb-0"
            >
              <v-row class="my-0 py-0" dense>
                <v-col>
                  <v-text-field
                    v-model="subtask.subtaskName"
                    label="Subtask Description"
                    outlined
                    dense
                    class="mb-1"
                  />
                </v-col>
                <v-col cols="auto">
                  <v-select
                    v-model="subtask.subtaskStateId"
                    :items="subtaskStates"
                    item-value="subtaskStateId"
                    item-title="stateName"
                    label="State"
                    outlined
                    dense
                  />
                </v-col>
                <v-col cols="auto">
                  <v-btn
                    class="mt-2"
                    size="small"
                    icon="mdi-delete"
                    color="red"
                    @click="removeSubtask(index)"
                  />
                </v-col>
              </v-row>
            </div>
          </div>
        </v-expand-transition>
      </v-card-text>
    </v-card>
  </v-bottom-sheet>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import { fetchPostApi } from "~/server/api";
import type {
  SubtaskType,
  TagType,
  TaskCreatedType,
  TaskType,
} from "~/server/api-schema";

const sheetOpen = ref(false);
const showSubtasksForm = ref(false);

const props = defineProps<{
  tags: TagType[];
  onSaved?: () => void;
}>();

const task = reactive<TaskCreatedType>({
  taskItemId: 0,
  taskName: "",
  taskDescription: "",
  taskCompleted: false,
  taskDueDate: new Date().toISOString().slice(0, 10),
  taskPriority: 1,
  tags: [],
  subtasks: [],
});


const priorityOptions = [
  { priority: 1, priorityLabel: "Normal" },
  { priority: 2, priorityLabel: "Urgent" },
];

const subtaskStates = [
  { subtaskStateId: 1, stateName: "Pending" },
  { subtaskStateId: 2, stateName: "Pending" },
  { subtaskStateId: 2, stateName: "Completed" },
];

const sendTask = async () => {
  try {
    const savedTask = await fetchPostApi<TaskType>("/Tasks", task);
    console.log("Task saved:", savedTask);
    props.onSaved?.();
    sheetOpen.value = false;
    showSubtasksForm.value = false;
    task.subtasks.splice(0);
  } catch (err) {
    console.error("Error saving task:", err);
  }
};

const cancelTask = () => {
  sheetOpen.value = false;
  showSubtasksForm.value = false;
  task.subtasks.splice(0);
};

const toggleSubtasksForm = () => {
  showSubtasksForm.value = !showSubtasksForm.value;
};

const addSubtask = () => {
  task.subtasks.push({ subtaskId:0, subtaskName: "", subtaskStateId: 1 } as SubtaskType);
};

const removeSubtask = (index: number) => {
  task.subtasks.splice(index, 1);
};
</script>
