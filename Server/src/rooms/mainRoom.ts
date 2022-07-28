import { Room, Client, Delayed } from "colyseus";
import { MainRoomState } from "./schema/MainRoomState";
import { NetworkedUser } from "./schema/NetworkedUser";

export class MainRoom extends Room<MainRoomState>{

    async onAuth(client: any, options: any) {

        console.log(options);
    }

    onCreate(options: any) {
        this.setState(new MainRoomState());
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