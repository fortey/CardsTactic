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
        this.onMessage("getSquads", (client, message) => this.onGetSquads(client));
    }

    onJoin(client: Client, options: any) {

        Data.FindOrCreateUser(options.name).then(id => this.joinClient(client, options, id));

    }

    onLeave(client: Client, consented?: boolean): void | Promise<any> {
        this.state.networkedUsers.delete(client.sessionId);
    }

    joinClient(client: Client, options: any, mongoId: string) {
        let newNetworkedUser = new NetworkedUser().assign({
            id: client.id,
            sessionId: client.sessionId,
            mongoId: mongoId.toString(),
        });

        this.state.networkedUsers.set(client.sessionId, newNetworkedUser);

        console.log(newNetworkedUser.mongoId);

        client.send("onJoin", newNetworkedUser);
    }

    onGetSquads(client: Client) {

        const mongoId = this.state.networkedUsers.get(client.sessionId).mongoId;

        Data.getSquads(mongoId).then(squads => { client.send('squads', squads); console.log(squads); });
    }
}