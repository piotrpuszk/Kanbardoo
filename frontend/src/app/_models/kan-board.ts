import { KanBoardStatus } from "./kan-board-status";
import { KanTable } from "./kan-table";
import { KanUser } from "./kan-user";

export interface KanBoard {
    id: number;
    name: string;
    owner: KanUser;
    creationDate: Date;
    startDate: Date;
    finishDate: Date;
    status: KanBoardStatus;
    backgroundImageUrl: string;
    tables: KanTable[];
}