import { Room, Client, Delayed } from "colyseus";
import { Data } from "../data";
import { MainRoomState } from "./schema/MainRoomState";
import { NetworkedUser } from "./schema/NetworkedUser";

export class MainRoom extends Room<MainRoomState>{

    maxClients = 100;
    // async onAuth(client: any, options: any) {

    //     console.log(options);
    //     return 1;
    // }

    onCreate(options: any) {
        this.setState(new MainRoomState());
        //Data.connect();
    }

    onJoin(client: Client, options: any) {

        let newNetworkedUser = new NetworkedUser().assign({
            id: client.id,
            sessionId: client.sessionId,
        });

        this.state.networkedUsers.set(client.sessionId, newNetworkedUser);

        Data.CreateUser(options.name);
    }

    onLeave(client: Client, consented?: boolean): void | Promise<any> {
        this.state.networkedUsers.delete(client.sessionId);
    }
}