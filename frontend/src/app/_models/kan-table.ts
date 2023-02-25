import { KanTask } from "./kan-task";

export interface KanTable {
    id: number;
    boardID: number;
    name: number;
    creationDate: Date;
    tasks: KanTask[];
}