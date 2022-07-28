import Arena from "@colyseus/arena";
import { monitor } from "@colyseus/monitor";
import { LobbyRoom } from "colyseus";
import { Data } from "./data";

/**
 * Import your Room files
 */
import { gameRoom } from "./rooms/gameRoom";
import { MainRoom } from "./rooms/mainRoom";

export default Arena({
    getId: () => "Your Colyseus App",

    initializeGameServer: (gameServer) => {
        gameServer.define('main_room', MainRoom);
        gameServer.define('game_room', gameRoom).enableRealtimeListing();
        gameServer.define('arena_lobby', LobbyRoom);

    },

    initializeExpress: (app) => {
        /**
         * Bind your custom express routes here:
         */
        app.get("/", (req, res) => {
            res.send("It's time to kick ass and chew bubblegum!");
        });

        /**
         * Bind @colyseus/monitor
         * It is recommended to protect this route with a password.
         * Read more: https://docs.colyseus.io/tools/monitor/
         */
        app.use("/colyseus", monitor());

        Data.initialize();
        Data.CreateUser("tt");
    },


    beforeListen: () => {
        /**
         * Before before gameServer.listen() is called.
         */
    }
});