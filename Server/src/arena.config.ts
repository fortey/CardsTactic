import Arena from "@colyseus/arena";
import { monitor } from "@colyseus/monitor";
import { LobbyRoom } from "colyseus";

/**
 * Import your Room files
 */
import { gameRoom } from "./rooms/gameRoom";

export default Arena({
    getId: () => "Your Colyseus App",

    initializeGameServer: (gameServer) => {
        /**
         * Define your room handlers:
         */
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

        var MongoClient = require('mongodb').MongoClient;
        let url = "";
        if (process.env.MONGODB_URI == undefined)
            url = "mongodb+srv://6thlop:blaBla!!@cluster0.scvjk.mongodb.net/?retryWrites=true&w=majority";
        else url = process.env.MONGODB_URI;

        MongoClient.connect(url, function (err: any, db: {
            db(arg0: string): any; close: () => void;
        }) {
            if (err) throw err;
            var dbo = db.db("mydb");
            dbo.createCollection("customers", function (err: any, res: any) {
                if (err) throw err;
                console.log("Collection created!");
                db.close();
            });
        });
    },


    beforeListen: () => {
        /**
         * Before before gameServer.listen() is called.
         */
    }
});