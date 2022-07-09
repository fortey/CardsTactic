import { Schema, Context, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class AbilitySchema extends Schema {
  @type("string") name: string;
  @type(["number"]) values: number[] = new ArraySchema<number>();
}

export class CreatureSchema extends Schema {
  @type("string") id: string;
  @type("string") name: string;
  @type("string") owner: string;
  @type("boolean") active: boolean;
  @type("number") health: number;
  @type(["string"]) attributes = new ArraySchema<string>();
  @type([AbilitySchema]) abilities = new ArraySchema<AbilitySchema>();
}

export class NetworkedUser extends Schema {
  @type("string") id: string;
  @type("string") sessionId: string;
  @type("boolean") connected: boolean;
  // @type("number") timestamp: number;
  @type({ map: "string" }) attributes = new MapSchema<string>();
}

export class GameRoomState extends Schema {

  @type("string") currentTurn: string;
  @type({ map: "boolean" }) players = new MapSchema<boolean>();
  @type(["string"]) board: string[] = new ArraySchema<string>('', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '', '');
  @type("string") winner: string;
  @type("boolean") draw: boolean;
  @type({ map: CreatureSchema }) creatures = new MapSchema<CreatureSchema>();
  @type({ map: NetworkedUser }) networkedUsers = new MapSchema<NetworkedUser>();
}
