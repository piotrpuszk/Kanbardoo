import { KanBoard } from "./kan-board";
import { KanUser } from "./kan-user";

export interface Invitation {
  id: number;
  user: KanUser;
  board: KanBoard;
}