import { CreatureSchema, GameRoomState } from "../../rooms/schema/GameRoomState";
import { Board } from "../board";
import { Ability } from "./ability";

export const poisoning = new Ability();
poisoning.name = "poisoning";
poisoning.onStartTurn = true;

poisoning.invoke = function (cellSource: number, source: CreatureSchema, state: GameRoomState, board: Board, cellTarget: number) {
    const ability = source.passiveAbilities.find(ability => ability.name == this.name);
    if (ability == undefined) return false;

    console.log("tut");
    if (cellSource == null)
        cellSource = state.board.indexOf(source.id);
    console.log("tut2");
    this.damage(cellSource, source, state, ability.values[0]);

    return true;
};
