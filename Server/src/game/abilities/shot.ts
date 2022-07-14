import { CreatureSchema, GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";
import { Ability } from "./ability";

export const shot = new Ability();
shot.name = "shot";
shot.onClicked = function (cell: number, source: CreatureSchema, state: GameRoomState, board: Board, sendTargets: any) {
    const ability = source.abilities.find(ability => ability.name == this.name);
    if (ability == undefined) return;
    const targets = board.targetsInRange(state.board, cell, ability.values[0], ability.values[1]);
    sendTargets(targets);
};
shot.invoke = function (cellSource: number, source: CreatureSchema, state: GameRoomState, board: Board, cellTarget: number) {
    const targetID = state.board[cellTarget];
    const target = state.creatures.get(targetID);
    if (target == undefined) return;
    const ability = source.abilities.find(ability => ability.name == this.name);
    if (ability == undefined) return;

    const targets = board.targetsInRange(state.board, cellSource, ability.values[0], ability.values[1]);
    if (targets.indexOf(cellTarget) == -1) return;

    //target.health -= ability.values[2];
    this.damage(cellTarget, target, state, ability.values[2]);
};
