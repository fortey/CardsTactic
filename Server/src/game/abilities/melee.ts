import { CreatureSchema, GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";
import { Ability } from "./ability";

export const melee = new Ability();
melee.onClicked = function (cell: number, source: CreatureSchema, state: GameRoomState, board: Board, sendTargets: any) {
    const targets = board.neighboringTargets(state.board, cell);
    sendTargets(targets);
};
melee.invoke = function (cellSource: number, source: CreatureSchema, state: GameRoomState, board: Board, cellTarget: number) {
    const targetID = state.board[cellTarget];
    const target = state.creatures.get(targetID);
    if (target == undefined) return false;
    const ability = source.abilities.find(ability => ability.name == "melee");
    if (ability == undefined) return false;

    //target.health -= ability.values[0];
    this.damage(cellTarget, target, state, ability.values[0]);
    return true;
};
