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

        Data.FindOrCreateUser(options.name).then(id => this.JoinClient(client, options, id));


    }

    onLeave(client: Client, consented?: boolean): void | Promise<any> {
        this.state.networkedUsers.delete(client.sessionId);
    }

    JoinClient(client: Client, options: any, mongoId: string) {
        let newNetworkedUser = new NetworkedUser().assign({
            id: client.id,
            sessionId: client.sessionId,
            mongoId: mongoId.toString(),
        });

        this.state.networkedUsers.set(client.sessionId, newNetworkedUser);

        console.log('---');
        console.log(newNetworkedUser.mongoId);
    }
}