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

	[Type(2, "string")]
	public string owner = default(string);

	[Type(3, "boolean")]
	public bool active = default(bool);

	[Type(4, "number")]
	public float health = default(float);

	[Type(5, "array", typeof(ArraySchema<string>), "string")]
	public ArraySchema<string> attributes = new ArraySchema<string>();

	[Type(6, "array", typeof(ArraySchema<AbilitySchema>))]
	public ArraySchema<AbilitySchema> abilities = new ArraySchema<AbilitySchema>();
}

