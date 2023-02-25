import { OrderByClause } from "./order-by-clause";

export interface BoardFilters {
    boardName : string;
    orderByClauses: OrderByClause[];
    roleID: number;
}