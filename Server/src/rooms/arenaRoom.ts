import { Room, Client, Delayed } from "colyseus";
import { ArenaRoomState } from "./schema/ArenaRoomState";
import { NetworkedUser } from "./schema/NetworkedUser";

export class ArenaRoom extends Room<ArenaRoomState>{

    onCreate(options: any) {
        this.setState(new ArenaRoomState());
    }

    onJoin(client: Client, options: any) {

        let newNetworkedUser = new NetworkedUser().assign({
            id: client.id,
            sessionId: client.sessionId,
        });

        this.state.networkedUsers.set(client.sessionId, newNetworkedUser);
    }

    onLeave(client: Client, consented?: boolean): void | Promise<any> {
        this.state.networkedUsers.delete(client.sessionId);
    }
}