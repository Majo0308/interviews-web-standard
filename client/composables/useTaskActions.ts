import { fetchDeleteApi, fetchGetApi, fetchGetByIdApi, fetchPutApi } from "~/server/api";
import type { TaskType } from "~/server/api-schema";

export function useTaskActions() {
  const deleteTask = async (taskId: number) => {
    await fetchDeleteApi(taskId, "/Tasks");
  };

  const updateTaskCompletion = async (task: TaskType) => {
    await fetchGetByIdApi(task.taskItemId, "/Tasks/completed");
  };

  return { deleteTask, updateTaskCompletion };
}
