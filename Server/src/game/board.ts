
const BOARD_WIDTH = 5;
const BOARD_HEIGHT = 4;
const LENGTH = BOARD_WIDTH * BOARD_HEIGHT;

export class Board {

    x_mapping: Map<number, Cell>;

    constructor() {

    }

    i_to_xy(i: number): { x: number, y: number } {
        return { x: i % BOARD_WIDTH, y: Math.floor(i / BOARD_WIDTH) };
    }

    xy_to_i(x: number, y: number): number {
        return y * BOARD_WIDTH + x;
    }

    convertFromSquadBoard(i: number, reverse: boolean): number {
        if (reverse)
            return Math.floor(i / 2) * BOARD_WIDTH + BOARD_WIDTH - 1 - i % 2;
        else
            return Math.floor(i / 2) * BOARD_WIDTH + i % 2;
    }

    availableCellsForMove(board: string[], cellIndex: number): number[] {
        const { x, y } = this.i_to_xy(cellIndex);
        const cells = [];

        if (x - 1 >= 0 && board[this.xy_to_i(x - 1, y)] == "")
            cells.push(this.xy_to_i(x - 1, y));
        if (x + 1 < BOARD_WIDTH && board[this.xy_to_i(x + 1, y)] == "")
            cells.push(this.xy_to_i(x + 1, y));

        if (y - 1 >= 0 && board[this.xy_to_i(x, y - 1)] == "")
            cells.push(this.xy_to_i(x, y - 1));
        if (y + 1 < BOARD_HEIGHT && board[this.xy_to_i(x, y + 1)] == "")
            cells.push(this.xy_to_i(x, y + 1));

        return cells;
    }

    neighboringTargets(board: string[], cellIndex: number): number[] {
        const { x, y } = this.i_to_xy(cellIndex);
        const cells = [];

        if (x - 1 >= 0 && board[this.xy_to_i(x - 1, y)] !== "")
            cells.push(this.xy_to_i(x - 1, y));
        if (x + 1 < BOARD_WIDTH && board[this.xy_to_i(x + 1, y)] !== "")
            cells.push(this.xy_to_i(x + 1, y));

        if (y - 1 >= 0 && board[this.xy_to_i(x, y - 1)] !== "")
            cells.push(this.xy_to_i(x, y - 1));
        if (y + 1 < BOARD_HEIGHT && board[this.xy_to_i(x, y + 1)] !== "")
            cells.push(this.xy_to_i(x, y + 1));

        if (x - 1 >= 0 && y - 1 >= 0 && board[this.xy_to_i(x - 1, y - 1)] !== "")
            cells.push(this.xy_to_i(x - 1, y - 1));
        if (x - 1 >= 0 && y + 1 < BOARD_HEIGHT && board[this.xy_to_i(x - 1, y + 1)] !== "")
            cells.push(this.xy_to_i(x - 1, y + 1));
        if (x + 1 < BOARD_WIDTH && y - 1 >= 0 && board[this.xy_to_i(x + 1, y - 1)] !== "")
            cells.push(this.xy_to_i(x + 1, y - 1));
        if (x + 1 < BOARD_WIDTH && y + 1 < BOARD_HEIGHT && board[this.xy_to_i(x + 1, y + 1)] !== "")
            cells.push(this.xy_to_i(x + 1, y + 1));

        return cells;
    }

    targetsInRange(board: string[], cellIndex: number, minRange: number, maxRange: number): number[] {
        const { x, y } = this.i_to_xy(cellIndex);
        const cells: number[] = [];

        board.forEach((element, index) => {
            if (element !== "") {
                const target = this.i_to_xy(index);
                if (Math.abs(target.x - x) >= minRange && Math.abs(target.x - x) <= maxRange ||
                    Math.abs(target.y - y) >= minRange && Math.abs(target.y - y) <= maxRange) {
                    cells.push(index);
                }
            }
        });

        return cells;
    }

    isNeighbor(a: number, b: number): boolean {
        const A = this.i_to_xy(a);
        const B = this.i_to_xy(b);

        return Math.abs(A.x - B.x) < 2 && Math.abs(A.y - B.y) < 2;
    }
}

class Cell {
    x: number;
    y: number;

    constructor(x: number, y: number) {
        this.x = x;
        this.y = y;
    }
}