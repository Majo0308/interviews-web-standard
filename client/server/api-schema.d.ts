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
};

export type TaskCreatedType = {
  taskItemId: number;
  taskName: string | null;
  taskDueDate: string;
  taskDescription: string | null;
  taskCompleted: boolean;
  taskPriority: number;
  tags: number[];
};