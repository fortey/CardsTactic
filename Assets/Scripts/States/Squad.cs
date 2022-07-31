// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.35
// 

using Colyseus.Schema;

public partial class Squad : Schema {
	[Type(0, "string")]
	public string _id = default(string);

	[Type(1, "string")]
	public string userId = default(string);

	[Type(2, "string")]
	public string name = default(string);

	[Type(3, "array", typeof(ArraySchema<string>), "string")]
	public ArraySchema<string> board = new ArraySchema<string>();
}

