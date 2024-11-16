export interface Task {
  taskId: string;
  title: string;
  description?: string;
  dueDate?: string;
  isCompleted: boolean;
}