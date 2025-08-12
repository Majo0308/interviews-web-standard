<script setup lang="ts">
import type { TaskCreatedType, SubtaskType } from "~/server/api-schema";

const props = defineProps<{
  modelValue: TaskCreatedType;
  priorityOptions: { priority: number; priorityLabel: string }[];
  subtaskStates: { subtaskStateId: number; stateName: string }[];
}>();

const addSubtask = () => {
  props.modelValue.subtasks.push({
    subtaskId: 0,
    subtaskName: "",
    subtaskStateId: 1,
  } as SubtaskType);
};

const removeSubtask = (index: number) => {
  props.modelValue.subtasks.splice(index, 1);
};
</script>

<template>
  <v-row dense>
    <v-col md="10">
      <v-text-field
        v-model="modelValue.taskDescription"
        label="Description"
        outlined
        dense
      />
    </v-col>
    <v-col md="2">
      <v-select
        v-model="modelValue.taskPriority"
        :items="priorityOptions"
        item-value="priority"
        item-title="priorityLabel"
        label="Priority"
        outlined
        dense
      />
    </v-col>
  </v-row>

  <div class="d-flex align-center mb-2">
    <h1 class="text-h6 mt-1 mr-2">Subtask Form</h1>
    <v-tooltip text="Add Subtask" location="top">
      <template v-slot:activator="{ props }">
        <v-btn
          v-bind="props"
          icon="mdi-plus"
          size="small"
          color="primary"
          @click="addSubtask"
        />
      </template>
    </v-tooltip>
  </div>

  <div v-for="(subtask, index) in modelValue.subtasks" :key="index">
    <v-row dense>
      <v-col>
        <v-text-field
          v-model="subtask.subtaskName"
          label="Subtask Description"
          outlined
          dense
        />
      </v-col>
      <v-col>
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
        <v-tooltip text="Delete Subtask" location="top">
          <template v-slot:activator="{ props }">
            <v-btn
              v-bind="props"
              size="small"
              icon="mdi-delete"
              color="red"
              @click="removeSubtask(index)"
            />
          </template>
        </v-tooltip>
      </v-col>
    </v-row>
  </div>
</template>
