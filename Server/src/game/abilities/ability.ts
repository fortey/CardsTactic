import { CreatureSchema, GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";

export class Ability {
    name: string;
    onClicked(cell: number, source: CreatureSchema, state: GameRoomState, board: Board, sendTargets: any) { }
    invoke(cellSource: number, source: CreatureSchema, state: GameRoomState, board: Board, cellTarget: number) { }

    damage(cell: number, creature: CreatureSchema, state: GameRoomState, damage: number) {
        creature.health -= damage;
        if (creature.health <= 0) {
            creature.health = 0;
            state.board[cell] = "";
            state.graveyard.push(creature.id);
        }
    }
}