import { AbilitySchema, CreatureSchema, GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";
import { Ability } from "./ability";

export const poison_shot = new Ability();
poison_shot.name = "poison_shot";

poison_shot.onClicked = function (cell: number, source: CreatureSchema, state: GameRoomState, board: Board, sendTargets: any) {
    if (source.points <= 0) return;

    const ability = source.abilities.find(ability => ability.name == this.name);
    if (ability == undefined) return;
    const targets = this.targets(cell, source, state, board, ability);

    sendTargets(targets);
};
poison_shot.invoke = function (cellSource: number, source: CreatureSchema, state: GameRoomState, board: Board, cellTarget: number) {
    if (source.points <= 0) return;

    const targetID = state.board[cellTarget];
    const target = state.creatures.get(targetID);
    if (target == undefined) return false;
    const ability = source.abilities.find(ability => ability.name == this.name);
    if (ability == undefined) return false;

    const targets = this.targets(cellSource, source, state, board, ability);
    if (targets.indexOf(cellTarget) == -1) return false;

    source.points--;
    if (target.passiveAbilities.findIndex(passive => passive.name == "poisoning") == - 1) {
        const poisoningAbility = new AbilitySchema();
        poisoningAbility.name = "poisoning";
        poisoningAbility.values.push(1);
        target.passiveAbilities.push(poisoningAbility);
    }

    this.damage(cellTarget, target, state, ability.values[2]);
    return true;
};

poison_shot.targets = function (cell: number, source: CreatureSchema, state: GameRoomState, board: Board, ability: AbilitySchema): number[] {
    const targets = board.targetsInRange(state.board, cell, ability.values[0], ability.values[1]);

    return targets.filter(targetCell => {
        const targetCreature = state.creatures.get(state.board[targetCell]);
        return targetCreature !== null && targetCreature.attributes.indexOf("shot_protection") == -1;
    });
}
