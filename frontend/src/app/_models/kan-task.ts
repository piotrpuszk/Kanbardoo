import { KanTaskStatus } from "./kan-task-status";
import { KanUser } from "./kan-user";

export interface KanTask {
    id: number;
    name: string;
    description: string;
    dueDate: Date;
    status: KanTaskStatus;
    assignee: KanUser;
}