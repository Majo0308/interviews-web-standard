<script setup lang="ts">
import { format } from "date-fns";
import { ref, onMounted } from "vue";
import { fetchGetApi, fetchPutApi } from "~/server/api";
import type { SubtaskTaskType } from "~/server/api-schema";

const subtasks = ref<(SubtaskTaskType & { taskName: string })[]>([]);
const subtaskStates = [
  { id: 1, label: "Pending", color: "#F26B50" },
  { id: 2, label: "In Progress", color: "#F2C450" },
  { id: 3, label: "Done", color: "#96CF76" },
];

onMounted(async () => {
  subtasks.value = await fetchGetApi<
    (SubtaskTaskType & { taskName: string })[]
  >("/Subtasks");
});

const getSubtasksByState = (stateId: number) =>
  subtasks.value.filter((s) => s.subtaskStateId === stateId);

const activeButtonStyle = (color: string) => ({
  color: "white",
  backgroundColor: color,
});

const updateSubtaskState = async (
  subtask: SubtaskTaskType,
  newStateId: number
) => {
  try {
    await fetchPutApi(subtask.subtaskId, "/Subtasks", {
      ...subtask,
      subtaskStateId: newStateId,
    });
    const target = subtasks.value.find(
      (s) => s.subtaskId === subtask.subtaskId
    );
    if (target) target.subtaskStateId = newStateId;
  } catch (err) {
    console.error("Error updating subtask state", err);
  }
};
</script>

<template>
  <div class="pa-4">
    <h1 class="text-h5 ">Subtasks</h1>
    <v-divider class="mb-4"></v-divider>
    <v-row>
      <!-- Iterate over states to group subtasks -->
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
              <!-- List all subtasks filtered by state -->
              <v-list-item
                v-for="sub in getSubtasksByState(state.id)"
                :key="sub.subtaskId"
                style="border-radius: 5px; background-color: white"
                class="mb-1"
              >
                <div class="mb-2 mt-2">{{ sub.subtaskName }}</div>
                <div class="text-caption mb-1" style="font-size: 10px">
                  Task: {{ sub.taskName }}
                </div>
                <div class="text-caption mb-1" style="font-size: 10px">
                  {{
                    sub.taskDueDate
                      ? format(new Date(sub.taskDueDate), "dd/MM/yyyy")
                      : ""
                  }}
                </div>
                <div class="d-flex gap-2 mb-1">
                  <!-- Buttons to update subtask state -->
                  <v-btn
                    size="small"
                    icon="mdi-clipboard-text-clock-outline"
                    @click="updateSubtaskState(sub, 1)"
                    :style="
                      sub.subtaskStateId === 1
                        ? activeButtonStyle('#F06565')
                        : {}
                    "
                    class="ma-0 mr-2"
                  />
                  <v-btn
                    size="small"
                    icon="mdi-clipboard-text-play-outline"
                    @click="updateSubtaskState(sub, 2)"
                    :style="
                      sub.subtaskStateId === 2
                        ? activeButtonStyle('#f0b356')
                        : {}
                    "
                    class="ma-0 mr-2"
                  />
                  <v-btn
                    size="small"
                    icon="mdi-clipboard-check-outline"
                    @click="updateSubtaskState(sub, 3)"
                    :style="
                      sub.subtaskStateId === 3
                        ? activeButtonStyle('#5fce73')
                        : {}
                    "
                    class="ma-0"
                  />
                </div>
              </v-list-item>
            </v-list>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </div>
</template>

