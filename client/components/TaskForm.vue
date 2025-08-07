<template>
  <v-bottom-sheet v-model="sheetOpen">
    <template v-slot:activator="{ props: activatorProps }">
      <v-btn v-bind="activatorProps" color="primary" text="Add Task" />
    </template>

    <v-card>
      <v-card-text>
        <v-row>
          <v-col cols="12" sm="4">
            <v-text-field
              v-model="task.taskName"
              label="Title"
              outlined
              dense
            />
          </v-col>

          <v-col cols="12" sm="4">
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

        <v-row>
          <v-col>
            <v-text-field
              v-model="task.taskDescription"
              label="Description"
              outlined
              dense
            />
          </v-col>

          <v-col cols="auto">
            <v-btn color="success" class="mt-4" @click="sendTask">
              <v-icon left>mdi-send</v-icon>
              Send Task
            </v-btn>
          </v-col>
          <v-col cols="auto">
            <v-btn color="error" class="mt-4" @click="cancelTask">
              <v-icon left>mdi-close</v-icon>
              Cancel
            </v-btn>
          </v-col>
        </v-row>
      </v-card-text>
    </v-card>
  </v-bottom-sheet>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import { fetchPostApi } from "~/server/api";
import type { TagType, TaskCreatedType, TaskType } from "~/server/api-schema";
const sheetOpen = ref(false);
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
});

const priorityOptions = [
  { priority: 1, priorityLabel: "Normal" },
  { priority: 2, priorityLabel: "Urgent" },
];

const sendTask = async () => {
  try {
    const savedTask = await fetchPostApi<TaskType>("/Tasks", task);

    console.log("Task guardada:", savedTask);
    props.onSaved?.();
  } catch (err) {
    console.error("Error saving task:", err);
  }
};
const cancelTask = () => {
  sheetOpen.value = false;
};
</script>
