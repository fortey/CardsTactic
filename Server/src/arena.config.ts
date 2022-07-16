import Arena from "@colyseus/arena";
import { monitor } from "@colyseus/monitor";
import { ArenaRoom } from "./rooms/arenaRoom";

/**
 * Import your Room files
 */
import { gameRoom } from "./rooms/gameRoom";
import { TicTacToe } from "./rooms/tictactoe";

export default Arena({
    getId: () => "Your Colyseus App",

    initializeGameServer: (gameServer) => {
        /**
         * Define your room handlers:
         */
        gameServer.define('game_room', gameRoom);
        gameServer.define('arena_room', ArenaRoom);

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
    },


    beforeListen: () => {
        /**
         * Before before gameServer.listen() is called.
         */
    }
});