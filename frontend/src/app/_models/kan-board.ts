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

export function getDefaultBoard() {
    return {
        id: 0,
        name: '',
        owner: { id: 0, userName: '', creationDate: new Date() },
        creationDate: new Date(),
        startDate: new Date(),
        finishDate: new Date(),
        status: { id: 0, name: '' },
        backgroundImageUrl: '',
        tables: []
    };
}