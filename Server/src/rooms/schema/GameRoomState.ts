import { Schema, Context, type, MapSchema, ArraySchema } from "@colyseus/schema";
import { NetworkedUser } from "./NetworkedUser";

export class AbilitySchema extends Schema {
  @type("string") name: string;
  @type("number") needPoints: number;
  @type(["number"]) values: number[] = new ArraySchema<number>();
  @type("number") minRange: number;
  @type("number") maxRange: number;
}

export class CreatureSchema extends Schema {
  @type("string") id: string;
  @type("string") name: string;
  @type("string") owner: string;
  @type("boolean") active: boolean;
  @type("number") maxHealth: number;
  @type("number") health: number;
  @type("number") steps: number;
  @type("number") maxSteps: number;
  @type("boolean") defense: boolean;
  @type("number") points: number;
  @type(["string"]) attributes = new ArraySchema<string>();
  @type([AbilitySchema]) abilities = new ArraySchema<AbilitySchema>();
  @type([AbilitySchema]) passiveAbilities = new ArraySchema<AbilitySchema>();
}



export class GameRoomState extends Schema {

  @type("string") currentTurn: string;
  @type({ map: "boolean" }) players = new MapSchema<boolean>();
  @type(["string"]) board: string[] = new ArraySchema<string>('', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '');
  @type("string") winner: string;
  @type("boolean") draw: boolean;
  @type({ map: CreatureSchema }) creatures = new MapSchema<CreatureSchema>();
  @type({ map: NetworkedUser }) networkedUsers = new MapSchema<NetworkedUser>();
  @type(["string"]) graveyard: string[] = new ArraySchema<string>();
}
