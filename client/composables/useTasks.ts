import { ref, computed } from "vue";
import { fetchGetApi, fetchGetByIdApi } from "~/server/api";
import type { TaskType } from "~/server/api-schema";

export function useTasks() {
  const tasks = ref<TaskType[]>([]);

  const loadTasks = async (tagId?: number) => {
    try {
      if (tagId && tagId !== 0) {
        tasks.value = await fetchGetByIdApi<TaskType[]>(tagId, "/Tasks/by-tags");
      } else {
        tasks.value = await fetchGetApi<TaskType[]>("/Tasks");
      }
    } catch (err: any) {
      console.error("Error loading tasks:", err.message);
    }
  };

  const completedTasks = computed(() => tasks.value.filter(t => t.taskCompleted));
  const incompletedTasks = computed(() => tasks.value.filter(t => !t.taskCompleted));

  return {
    tasks,
    loadTasks,
    completedTasks,
    incompletedTasks
  };
}
