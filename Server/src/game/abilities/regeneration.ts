import { CreatureSchema, GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";
import { Ability } from "./ability";

export const regeneration = new Ability();
regeneration.name = "regeneration";
regeneration.onStartTurn = true;

regeneration.invoke = function (cellSource: number, source: CreatureSchema, state: GameRoomState, board: Board, cellTarget: number) {

    const ability = source.passiveAbilities.find(ability => ability.name == this.name);
    if (ability == undefined) return false;

    source.health = Math.min(source.maxHealth, source.health + ability.values[0]);

    return true;
};
