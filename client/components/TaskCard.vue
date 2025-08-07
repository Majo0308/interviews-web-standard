<template>
  <v-card class="ma-2 pa-2 w-98" outlined>
    <v-card-text>
      <v-row align="center">
        <v-col cols="auto">
          <v-checkbox
            v-model="task.taskCompleted"
            :ripple="false"
            hide-details
            color="primary"
            class="ma-0"
            style="margin-left: -12px"
            density="compact"
          />
        </v-col>
        <v-col>
          <v-list-item-title
            :class="{
              'text-decoration-line-through text-grey': task.taskCompleted,
            }"
          >
            {{ task.taskName }}
          </v-list-item-title>
        </v-col>
        <v-col cols="auto">
          <v-btn
            size="small"
            icon="mdi-pencil"
            @click="editTask"
            class="ma-0 mr-2"
          />
          <v-btn
            size="small"
            icon="mdi-delete"
            @click="deleteTask"
            class="ma-0"
          />
        </v-col>
      </v-row>

      <v-row align="center">
        <v-col>
          <v-chip
            v-for="tag in task.tags"
            :key="tag.tagId"
            class="mr-1"
            :color="tag.tagColor || 'primary'"
            text-color="white"
            small
          >
            {{ tag.tagName }}
          </v-chip>
        </v-col>
        <v-col cols="auto">
          {{ task.taskDueDate }}
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import type { TaskType } from '~/server/api-schema';



defineProps<{
  task: TaskType;
}>();

const editTask = () => {};

const deleteTask = () => {};
</script>
