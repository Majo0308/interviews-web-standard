<script setup lang="ts">
import type { TaskCreatedType, TagType } from "~/server/api-schema";

defineProps<{
  modelValue: TaskCreatedType;
  tags: TagType[];
}>();

defineEmits(["save", "toggle-subtasks", "cancel"]);
</script>
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
      <v-tooltip text="Save Task" location="top">
        <template v-slot:activator="{ props }">
          <v-btn    
            icon="mdi-send"
            size="small"
            color="success"
            class="mr-2"
            v-bind="props"
            @click="$emit('save')"
          />
        </template>
      </v-tooltip>
      <v-tooltip text="More Details" location="top">
        <template v-slot:activator="{ props }">
          <v-btn
            v-bind="props"
            icon="mdi-format-list-group-plus"
            size="small"
            color="primary"
            class="mr-2"
            @click="$emit('toggle-subtasks')"
          />
        </template>
      </v-tooltip>
      <v-tooltip text="Cancel" location="top">
        <template v-slot:activator="{ props }">
          <v-btn
            v-bind="props"
            icon="mdi-close"
            size="small"
            color="error"
            @click="$emit('cancel')"
          />
        </template>
      </v-tooltip>
    </v-col>
  </v-row>
</template>


