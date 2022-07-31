import { Schema, Context, type, MapSchema, ArraySchema } from "@colyseus/schema";

export class Squad extends Schema {

    @type("string") _id: string;
    @type("string") userId: string;
    @type("string") name: string;
    @type(["string"]) board: string[];

}
