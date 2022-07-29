// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.35
// 

using Colyseus.Schema;

public partial class NetworkedUser : Schema {
	[Type(0, "string")]
	public string id = default(string);

	[Type(1, "string")]
	public string sessionId = default(string);

	[Type(2, "string")]
	public string mongoId = default(string);

	[Type(3, "boolean")]
	public bool connected = default(bool);

	[Type(4, "map", typeof(MapSchema<string>), "string")]
	public MapSchema<string> attributes = new MapSchema<string>();
}

