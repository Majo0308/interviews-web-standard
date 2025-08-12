<script setup lang="ts">
import { format } from "date-fns";
import { ref, onMounted } from "vue";
import { useRoute } from "vue-router";
import { fetchGetByIdApi, fetchPutApi } from "~/server/api";
import type { TaskType, SubtaskType } from "~/server/api-schema";

const route = useRoute();
const task = ref<TaskType | null>(null);
const subtaskStates = [
  { id: 1, label: "Pendant", labelShort: "ToDo", color: "#F26B50" },
  { id: 2, label: "In Progress", labelShort: "Doing", color: "#F2C450" },
  { id: 3, label: "Done", labelShort: "Done", color: "#96CF76" },
];

onMounted(async () => {
  const id = route.params.id;
  const response = await fetchGetByIdApi<TaskType>(id, "/Tasks");
  task.value = response;
});

const getSubtasksByState = (stateId: number) => {
  return (
    task.value?.subtasks?.filter(
      (s: SubtaskType) => s.subtaskStateId === stateId
    ) || []
  );
};

const updateSubtaskState = async (
  subtaskSent: SubtaskType,
  newStateId: number
) => {
  try {
    await fetchPutApi(subtaskSent?.subtaskId, `/Subtasks`, {
      ...subtaskSent,
      subtaskStateId: newStateId,
    });

    const subtask = task.value?.subtasks.find(
      (s: SubtaskType) => s.subtaskId === subtaskSent.subtaskId
    );
    if (subtask) {
      subtask.subtaskStateId = newStateId;
    }
  } catch (err) {
    console.error("Error updating subtask state", err);
  }
};
</script>

<template>
  <v-container>
    <v-card class="pa-4">
      <div class="d-flex justify-space-between align-center mb-2">
        <h1 class="text-h5">{{ task?.taskName }}</h1>
        <v-btn
          size="small"
          icon="mdi-arrow-left"
          :to="'/'"
          class="ma-0"
        ></v-btn>
      </div>
      <div style="display: flex; justify-content: space-between">
        <div>
          <v-chip
            v-for="tag in task?.tags"
            style="font-size: 12px"
            :key="tag.tagId"
            class="mr-2 mb-2"
            :color="tag.tagColor || 'primary'"
            text-color="white"
            small
          >
            {{ tag.tagName }}
          </v-chip>
        </div>
        <div class="text-caption grey--text">
          {{
            task?.taskDueDate
              ? format(new Date(task?.taskDueDate), "dd/MM/yyyy")
              : ""
          }}
        </div>
      </div>
      <v-divider class="mb-3" />

      <v-row>
        <v-col cols="12" md="12">
          <h2 class="text-subtitle-1 mb-1">Description</h2>
          <p class="text-body-2">{{ task?.taskDescription }}</p>
        </v-col>
      </v-row>
    </v-card>

    <h2 class="text-h6 mt-6 mb-4">Subtasks</h2>
    <v-row>
      <v-col cols="12" md="4" v-for="state in subtaskStates" :key="state.id">
        <v-card
          class="pa-2"
          :style="{
            borderLeft: `4px solid ${state.color}`,
            backgroundColor: '#FAFAFA',
          }"
        >
          <v-card-title class="text-subtitle-1 font-weight-bold">
            {{ state.label }}
          </v-card-title>
          <v-divider />
          <v-card-text>
            <v-list density="compact" style="background-color: #fafafa">
              <v-list-item
                v-for="sub in getSubtasksByState(state.id)"
                :key="sub.subtaskId"
                style="border-radius: 5px; background-color: white"
                class="mb-1"
              >
                <div class="mb-2 mt-2">{{ sub.subtaskName }}</div>
                <div class="d-flex gap-2">
                  <v-col cols="auto">
                    <v-btn
                      size="small"
                      icon="mdi-clipboard-text-clock-outline"
                      @click="updateSubtaskState(sub, 1)"
                      :style="
                        sub.subtaskStateId === 1
                          ? {
                              color: 'white',
                              backgroundColor: '#F06565',
                            }
                          : {}
                      "
                      class="ma-0, mr-2"
                    ></v-btn>
                    <v-btn
                      size="small"
                      icon="mdi-clipboard-text-play-outline"
                      @click="updateSubtaskState(sub, 2)"
                      class="ma-0 mr-2"
                      :style="
                        sub.subtaskStateId === 2
                          ? {
                              color: 'white',
                              backgroundColor: '#f0b356',
                            }
                          : {}
                      "
                    ></v-btn>
                    <v-btn
                      size="small"
                      icon="mdi-clipboard-check-outline"
                      @click="updateSubtaskState(sub, 3)"
                      class="ma-0"
                      :style="
                        sub.subtaskStateId === 3
                          ? {
                              color: 'white',
                              backgroundColor: '#5fce73',
                            }
                          : {}
                      "
                    ></v-btn>
                  </v-col>
                </div>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

