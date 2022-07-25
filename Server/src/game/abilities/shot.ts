import { AbilitySchema, CreatureSchema, GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";
import { Ability } from "./ability";

export const shot = new Ability();
shot.name = "shot";

shot.onClicked = function (cell: number, source: CreatureSchema, state: GameRoomState, board: Board, sendTargets: any) {
    const ability = source.abilities.find(ability => ability.name == this.name);
    if (ability == undefined) return;
    const targets = this.targets(cell, source, state, board, ability);//board.targetsInRange(state.board, cell, ability.values[0], ability.values[1]);

    sendTargets(targets);
};
shot.invoke = function (cellSource: number, source: CreatureSchema, state: GameRoomState, board: Board, cellTarget: number) {
    const targetID = state.board[cellTarget];
    const target = state.creatures.get(targetID);
    if (target == undefined) return false;
    const ability = source.abilities.find(ability => ability.name == this.name);
    if (ability == undefined) return false;

    const targets = this.targets(cellSource, source, state, board, ability);
    if (targets.indexOf(cellTarget) == -1) return false;

    const random = Math.random();

    if (random < 0.6)
        this.damage(cellTarget, target, state, ability.values[0]);
    else if (random < 0.9)
        this.damage(cellTarget, target, state, ability.values[1]);
    else
        this.damage(cellTarget, target, state, ability.values[2]);

    return true;
};

shot.targets = function (cell: number, source: CreatureSchema, state: GameRoomState, board: Board, ability: AbilitySchema): number[] {
    const targets = board.targetsInRange(state.board, cell, ability.minRange, ability.maxRange);

    return targets.filter(targetCell => {
        const targetCreature = state.creatures.get(state.board[targetCell]);
        return targetCreature !== null && targetCreature.attributes.indexOf("shot_protection") == -1;
    });
}
