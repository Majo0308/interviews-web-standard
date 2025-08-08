<template>
  <v-row dense>
    <v-col cols="12" md="6">
      <v-text-field
        v-model="modelValue.taskName"
        label="Title"
        outlined
        dense
        :rules="[(v) => !!v || 'Title is required']"
        required
      />
    </v-col>

    <v-col cols="12" md="2">
      <v-select
        v-model="modelValue.tags"
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

    <v-col cols="12" md="2">
      <v-date-input v-model="modelValue.taskDueDate" label="Due date" />
    </v-col>

    <v-col cols="12" md="2" class="d-flex justify-end">
      <v-btn
        icon="mdi-send"
        size="small"
        color="success"
        class="mr-2"
        @click="$emit('save')"
      />
      <v-btn
        icon="mdi-format-list-group-plus"
        size="small"
        color="primary"
        class="mr-2"
        @click="$emit('toggle-subtasks')"
      />
      <v-btn
        icon="mdi-close"
        size="small"
        color="error"
        @click="$emit('cancel')"
      />
    </v-col>
  </v-row>
</template>

<script setup lang="ts">
import type { TaskCreatedType, TagType } from "~/server/api-schema";

defineProps<{
  modelValue: TaskCreatedType;
  tags: TagType[];
}>();

defineEmits(["save", "toggle-subtasks", "cancel"]);
</script>
