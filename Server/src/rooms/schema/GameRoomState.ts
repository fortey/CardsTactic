import { Schema, Context, type, MapSchema, ArraySchema } from "@colyseus/schema";


export class CreatureSchema extends Schema {
  @type("string") id: string;
  @type("string") name: string;
  @type("boolean") active: boolean;
  @type("number") health: number;
  @type({ map: "string" }) attributes = new MapSchema<string>();
}

export class GameRoomState extends Schema {

  @type("string") currentTurn: string;
  @type({ map: "boolean" }) players = new MapSchema<boolean>();
  @type(["string"]) board: string[] = new ArraySchema<string>('', '', '', '', '', '', '', '', '');
  @type("string") winner: string;
  @type("boolean") draw: boolean;
  @type({ map: CreatureSchema }) creatures = new MapSchema<CreatureSchema>();

}
