import { AbilitySchema, CreatureSchema, GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";
import { Ability } from "./ability";

export const melee = new Ability();
melee.name = "melee";

melee.onClicked = function (cell: number, source: CreatureSchema, state: GameRoomState, board: Board, sendTargets: any) {
    const targets = this.targets(cell, source, state, board, null);
    sendTargets(targets);
};
melee.invoke = function (cellSource: number, source: CreatureSchema, state: GameRoomState, board: Board, cellTarget: number) {
    const targetID = state.board[cellTarget];
    const target = state.creatures.get(targetID);
    if (target == undefined) return false;
    const ability = source.abilities.find(ability => ability.name == this.name);
    if (ability == undefined) return false;

    //target.health -= ability.values[0];
    this.damage(cellTarget, target, state, ability.values[0]);
    return true;
};

melee.targets = function (cell: number, source: CreatureSchema, state: GameRoomState, board: Board, ability: AbilitySchema): number[] {
    const targets = board.neighboringTargets(state.board, cell);
    const defenders: number[] = [];
    const notDefenders: number[] = [];

    targets.forEach(targetCell => {
        const targetCreature = state.creatures.get(state.board[targetCell]);
        if (targetCreature.owner == source.owner) { return; }
        if (targetCreature.defense) { defenders.push(targetCell); return; }
        notDefenders.push(targetCell);
    });

    const unavailable = notDefenders.filter(target => {
        let hasDefender = false;
        defenders.forEach(defender => { if (board.isNeighbor(target, defender)) hasDefender = true; });
        return hasDefender;
    });

    return targets.filter(targetCell => {
        return unavailable.indexOf(targetCell) == -1;
    });
};
