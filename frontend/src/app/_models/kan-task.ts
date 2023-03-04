import { KanTaskStatus } from "./kan-task-status";
import { KanUser } from "./kan-user";

export interface KanTask {
    id: number;
    name: string;
    description: string;
    dueDate: Date;
    status: KanTaskStatus;
    assignee: KanUser;
    tableID: number;
    priority: number;
}

export function getDefaultTask() {
    return {
        id: 0,
        name: '',
        description: '',
        dueDate: new Date(),
        status: { id: 0, name: '' },
        assignee: { id: 0, userName: '', creationDate: new Date() },
        tableID: 0,
        priority: 0
    };
}