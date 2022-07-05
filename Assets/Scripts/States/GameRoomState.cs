// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.35
// 

using Colyseus.Schema;

public partial class GameRoomState : Schema {
	[Type(0, "string")]
	public string currentTurn = default(string);

	[Type(1, "map", typeof(MapSchema<bool>), "boolean")]
	public MapSchema<bool> players = new MapSchema<bool>();

	[Type(2, "array", typeof(ArraySchema<string>), "string")]
	public ArraySchema<string> board = new ArraySchema<string>();

	[Type(3, "string")]
	public string winner = default(string);

	[Type(4, "boolean")]
	public bool draw = default(bool);

	[Type(5, "map", typeof(MapSchema<CreatureSchema>))]
	public MapSchema<CreatureSchema> creatures = new MapSchema<CreatureSchema>();

	[Type(6, "map", typeof(MapSchema<NetworkedUser>))]
	public MapSchema<NetworkedUser> networkedUsers = new MapSchema<NetworkedUser>();
}

