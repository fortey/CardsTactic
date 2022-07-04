// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.35
// 

using Colyseus.Schema;

public partial class CreatureSchema : Schema {
	[Type(0, "string")]
	public string id = default(string);

	[Type(1, "string")]
	public string name = default(string);

	[Type(2, "boolean")]
	public bool active = default(bool);

	[Type(3, "number")]
	public float health = default(float);

	[Type(4, "map", typeof(MapSchema<string>), "string")]
	public MapSchema<string> attributes = new MapSchema<string>();
}

