import { GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";

export const melee = {
    onClicked(cell: number, state: GameRoomState, board: Board, sendTargets: any) {
        const targets = board.neighboringTargets(state.board, cell);
        sendTargets(targets);
    }
};