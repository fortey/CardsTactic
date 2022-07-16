import { Schema, Context, type, MapSchema, ArraySchema } from "@colyseus/schema";
import { NetworkedUser } from "./NetworkedUser";

export class ArenaRoomState extends Schema {
    @type({ map: NetworkedUser }) networkedUsers = new MapSchema<NetworkedUser>();
}