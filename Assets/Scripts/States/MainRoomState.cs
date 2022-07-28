// 
// THIS FILE HAS BEEN GENERATED AUTOMATICALLY
// DO NOT CHANGE IT MANUALLY UNLESS YOU KNOW WHAT YOU'RE DOING
// 
// GENERATED USING @colyseus/schema 1.0.35
// 

using Colyseus.Schema;

public partial class MainRoomState : Schema {
	[Type(0, "map", typeof(MapSchema<NetworkedUser>))]
	public MapSchema<NetworkedUser> networkedUsers = new MapSchema<NetworkedUser>();
}

