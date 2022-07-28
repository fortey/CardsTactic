
class MongoData {
    mongoClient: any;
    url: string;

    public initialize() {
        const MongoClient = require('mongodb').MongoClient;


        if (process.env.MONGODB_URI == undefined)
            this.url = "mongodb+srv://6thlop:blaBla!!@cluster0.scvjk.mongodb.net/?retryWrites=true&w=majority";
        else this.url = process.env.MONGODB_URI;

        this.mongoClient = new MongoClient(this.url, { useUnifiedTopology: true });
    }

    public connect() {
        this.mongoClient.connect(function (err: any, client: {
            db(arg0: string): any; close: () => void;
        }) {
            if (err) throw err;
            var dbo = client.db("mydb");
            dbo.createCollection("users", function (err: any, res: any) {
                if (err) throw err;
                console.log("Collection created!");
                client.close();
            });
            //db.close();
        });
    }

    public CreateUser(name: string) {
        this.mongoClient.connect(function (err: any, db: {
            db(arg0: string): any; users: any; close: () => void;
        }) {
            if (err) throw err;
            var dbo = db.db("userdb");
            // dbo.createCollection("users", function (err: any, res: any) {
            //     if (err) throw err;
            //     console.log("Collection created!");
            //     db.close();
            // });
            //db.close();

            const users = dbo.collection("users");

            const user = users.find();
            console.log(user.length);
            db.close();
        });
    }
}


export const Data = new MongoData();

