import { ref } from "vue";
import { fetchGetByIdApi } from "~/server/api";
import type { TaskType } from "~/server/api-schema";

const tasks = ref<TaskType[]>([]);

export function useTasks() {
  const getTasksByTag = async (tagId: number) => {
    try {
      const response=await fetchGetByIdApi<TaskType[]>(tagId, "/Tasks/by-tags");
      tasks.value=response;
    } catch (err: any) {
      console.error(err.message);
    }
  };

  const editTask = (task: TaskType) => {
    console.log("Edit task", task);
  };

  const deleteTask = (task: TaskType) => {
    console.log("Delete task", task);
  };

  return {
    tasks,
    getTasksByTag,
    editTask,
    deleteTask,
  };
}
