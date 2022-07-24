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

	[Type(1, "boolean")]
	public bool needPoints = default(bool);

	[Type(2, "array", typeof(ArraySchema<float>), "number")]
	public ArraySchema<float> values = new ArraySchema<float>();
}

