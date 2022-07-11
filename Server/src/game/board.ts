
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

    nneighboringTargets(board: string[], cellIndex: number, range: number): number[] {
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
}

class Cell {
    x: number;
    y: number;

    constructor(x: number, y: number) {
        this.x = x;
        this.y = y;
    }
}