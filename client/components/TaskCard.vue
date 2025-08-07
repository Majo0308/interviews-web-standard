<template>
  <v-card
    class="ma-2 pa-0 w-98"
    outlined
    :class="{ 'border-red': task.taskPriority === 3 }"
    ref="cardRef"
    style="position: relative"
  >
    <v-card-text>
      <canvas
        ref="confettiCanvas"
        style="
          position: absolute;
          top: 0;
          left: 0;
          width: 100%;
          height: 100%;
          pointer-events: none;
        "
      ></canvas>
      <v-row align="center" class="my-0 py-0" dense>
        <v-col cols="auto">
          <v-checkbox
            v-model="task.taskCompleted"
            :ripple="false"
            hide-details
            color="primary"
            class="ma-0"
            style="margin-left: -12px"
            density="compact"
            @change="handleCheck"
          />
        </v-col>
        <v-col>
          <NuxtLink
            :to="`/tasks/${task.taskItemId}`"
            style="text-decoration: none; color: inherit"
          >
            <v-row
              class="my-0 py-0"
              dense
              style="display: flex; justify-content: space-between"
            >
              <v-list-item-title
                :class="{
                  'text-decoration-line-through text-grey': task.taskCompleted,
                }"
              >
                {{ task.taskName }}
              </v-list-item-title>
            </v-row>

            <v-row align="center" class="my-0 py-0" dense>
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
            </v-row>
          </NuxtLink></v-col
        >
        <v-col cols="auto">
          <div style="display: flex; justify-content: right">
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
          </div>
          <v-col cols="auto">
             {{
            task?.taskDueDate
              ? format(new Date(task?.taskDueDate), "dd/MM/yyyy")
              : ""
          }}
          </v-col>
        </v-col>
      </v-row>
    </v-card-text>
  </v-card>
</template>

<script setup lang="ts">
import { format } from "date-fns";
import type { TaskType } from "~/server/api-schema";
import confetti from "canvas-confetti";
import { ref, nextTick } from "vue";
const confettiCanvas = ref<HTMLCanvasElement | null>(null);
let localConfetti: ReturnType<typeof confetti.create> | null = null;

watchEffect(() => {
  if (confettiCanvas.value && !localConfetti) {
    localConfetti = confetti.create(confettiCanvas.value, {
      resize: true,
      useWorker: true,
    });
  }
});
const props = defineProps<{
  task: TaskType;
}>();

const cardRef = ref<HTMLElement | null>(null);

const editTask = () => {};
const deleteTask = () => {};

const handleCheck = async () => {
  await nextTick();

  if (props.task.taskCompleted && cardRef.value && localConfetti) {
    const fire = (x: number, y: number) => {
      localConfetti!({
        particleCount: 30,
        spread: 60,
        startVelocity: 30,
        origin: { x, y },
        colors: ['#00cfff'],
      });
    };

    fire(0.2, 0.2); // top-left
    fire(0.8, 0.2); // top-right
    fire(0.2, 0.8); // bottom-left
    fire(0.8, 0.8); // bottom-right
    fire(0.5, 0.2); // top-center
    fire(0.5, 0.8); // bottom-center
    fire(0.2, 0.5); // middle-left
    fire(0.8, 0.5); // middle-right
  }
};
</script>

<style scoped>
.border-red {
  border: 2px solid red;
}
</style>
