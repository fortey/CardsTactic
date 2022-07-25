// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.35
// 

using Colyseus.Schema;

public partial class AbilitySchema : Schema {
	[Type(0, "string")]
	public string name = default(string);

	[Type(1, "number")]
	public float needPoints = default(float);

	[Type(2, "array", typeof(ArraySchema<float>), "number")]
	public ArraySchema<float> values = new ArraySchema<float>();

	[Type(3, "number")]
	public float minRange = default(float);

	[Type(4, "number")]
	public float maxRange = default(float);
}

