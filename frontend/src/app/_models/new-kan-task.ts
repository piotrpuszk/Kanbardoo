import { KanTaskStatus } from "./kan-task-status";
import { KanUser } from "./kan-user";

export interface NewKanTask {
  name: string;
  description: string;
  assignee: KanUser;
  status: KanTaskStatus;
  dueDate: Date;
  tableID: number;
}