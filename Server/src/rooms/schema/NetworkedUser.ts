import { Schema, Context, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class NetworkedUser extends Schema {
    @type("string") id: string;
    @type("string") sessionId: string;
    @type("string") mongoId: string;
    @type("boolean") connected: boolean;
    // @type("number") timestamp: number;
    @type({ map: "string" }) attributes = new MapSchema<string>();
}