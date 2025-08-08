export type TagType = {
  tagId: number;
  tagName: string;
  tagColor?: string;
};


export type TaskType = {
  taskItemId: number;
  taskName: string | null;
  taskDueDate: string;
  taskDescription: string | null;
  taskCompleted: boolean;
  taskPriority: number;
  tags: TagType[];
  subtasks: SubtaskType[];
};

export type TaskCreatedType = {
  taskItemId: number;
  taskName: string | null;
  taskDueDate: string;
  taskDescription: string | null;
  taskCompleted: boolean;
  taskPriority: number;
  tags: number[];
  subtasks: SubtaskType[];
};

export type SubtaskStateType={
  subtaskStateId: number;
  subtaskStateName: string;
}
export type SubtaskType = {
  subtaskId: number;
  subtaskName: string;
  subtaskStateId: number;
};

export type SubtaskTaskType = {
  subtaskId: number;
  subtaskName: string;
  subtaskStateId: number;
  taskName:string;
  taskDueDate: string;
  
};