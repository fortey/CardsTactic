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
	public float maxHealth = default(float);

	[Type(5, "number")]
	public float health = default(float);

	[Type(6, "number")]
	public float steps = default(float);

	[Type(7, "number")]
	public float maxSteps = default(float);

	[Type(8, "boolean")]
	public bool defense = default(bool);

	[Type(9, "number")]
	public float points = default(float);

	[Type(10, "array", typeof(ArraySchema<string>), "string")]
	public ArraySchema<string> attributes = new ArraySchema<string>();

	[Type(11, "array", typeof(ArraySchema<AbilitySchema>))]
	public ArraySchema<AbilitySchema> abilities = new ArraySchema<AbilitySchema>();

	[Type(12, "array", typeof(ArraySchema<AbilitySchema>))]
	public ArraySchema<AbilitySchema> passiveAbilities = new ArraySchema<AbilitySchema>();
}

