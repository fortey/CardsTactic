import { CreatureSchema, GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";

export class Ability {
    onClicked(cell: number, state: GameRoomState, board: Board, sendTargets: any) { }
    invoke(cellSource: number, source: CreatureSchema, state: GameRoomState, board: Board, cellTarget: number) { }
}