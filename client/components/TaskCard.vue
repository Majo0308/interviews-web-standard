<template>
  <v-card
    class="ma-2 mt-1 pa-0 w-98"
    outlined
    :style="cardStyles"
    style="position: relative"
  >
    <v-card-text>
      <!-- Confetti Canvas -->
      <canvas ref="confettiCanvas" class="confetti-canvas"></canvas>

      <v-row align="center" dense>
        <!-- Checkbox -->
        <v-col cols="auto">
          <v-checkbox
            color="primary"
            :model-value="task.taskCompleted"
            hide-details
            density="compact"
            class="ma-0"
            @change="onCheckboxClick"
          />
        </v-col>

        <!-- Name & Tags -->
        <v-col>
          <NuxtLink :to="`/tasks/${task.taskItemId}`" class="task-link">
            <v-list-item-title :class="{ done: task.taskCompleted }">
              {{ task.taskName }}
            </v-list-item-title>
            <v-row dense>
              <v-col>
                <v-chip
                  v-for="tag in task.tags"
                  :key="tag.tagId"
                  class="mr-1"
                  :color="tag.tagColor || 'primary'"
                  text-color="white"
                  small
                  style="font-size: 11px;"
                >
                  {{ tag.tagName }}
                </v-chip>
              </v-col>
            </v-row>
          </NuxtLink>
        </v-col>

        <!-- Actions -->
        <v-col cols="auto" class="text-right">
          <v-btn
            class="mr-2"
            size="small"
            icon="mdi-pencil"
            @click="$emit('edit', task)"
          />
          <v-btn
            size="small"
            color="red"
            icon="mdi-delete"
            @click="deleteDialog = true"
          />
          <div class="text-caption mt-1">
            {{
              task?.taskDueDate
                ? format(new Date(task.taskDueDate), "dd/MM/yyyy")
                : ""
            }}
          </div>
        </v-col>
      </v-row>
    </v-card-text>

    <!-- Confirm Delete -->
    <v-dialog v-model="deleteDialog" max-width="400">
      <v-card>
        <v-card-title>¿Eliminar tarea?</v-card-title>
        <v-card-text>
          ¿Estás seguro que deseas eliminar la tarea "<b>{{ task.taskName }}</b
          >"?
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="deleteDialog = false">Cancelar</v-btn>
          <v-btn color="red" text @click="confirmDelete">Eliminar</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-card>
</template>

<script setup lang="ts">
import { format } from "date-fns";
import type { TaskType } from "~/server/api-schema";
import confetti from "canvas-confetti";
import { useTaskActions } from "~/composables/useTaskActions";
import { ref, nextTick, watchEffect } from "vue";
import { fetchDeleteApi, fetchPutApi } from "~/server/api";

const { deleteTask, updateTaskCompletion } = useTaskActions();
const props = defineProps<{
  task: TaskType;
  getTasks: () => void;
}>();

//Edit Task
const emit = defineEmits(["edit"]);

//Delete Task
const deleteDialog = ref(false);

const confirmDelete = async () => {
  deleteDialog.value = false;
  await deleteTask(props.task.taskItemId);
  props.getTasks();
};

//Complete Task
const onCheckboxClick = async () => {
  const newState = !props.task.taskCompleted;
  if (!props.task.taskCompleted && localConfetti) {
    fireConfetti();
    setTimeout(() => {
      props.task.taskCompleted = true;
    }, 800);
  } else {
    props.task.taskCompleted = false;
  }
  await updateTaskCompletion(props.task);
};
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
const fireConfetti = () => {
  const fire = (x: number, y: number) => {
    localConfetti!({
      particleCount: 30,
      spread: 60,
      startVelocity: 30,
      origin: { x, y },
      colors: ["#00cfff", "#00e676", "#ffea00"],
    });
  };
  fire(0.2, 0.2);
  fire(0.8, 0.2);
  fire(0.2, 0.8);
  fire(0.8, 0.8);
  fire(0.5, 0.5);
};
const cardStyles = computed(() => ({
  boxShadow:
    props.task.taskPriority === 1 || props.task.taskCompleted
      ? ""
      : "0px 0px 10px 5px #E86D4A",
}));
</script>

<style scoped>
.border-red {
  border: 2px solid red;
}
.done {
  text-decoration: line-through;
  color: grey;
}
.task-link {
  text-decoration: none;
  color: inherit;
}
.confetti-canvas {
  position: absolute;
  inset: 0;
  width: 100%;
  height: 100%;
  pointer-events: none;
}
</style>
