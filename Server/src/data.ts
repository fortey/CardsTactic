
class MongoData {
    mongoClient: any;
    url: string;

    public initialize() {
        this.mongoClient = require('mongodb').MongoClient;

        if (process.env.MONGODB_URI == undefined)
            this.url = "mongodb+srv://6thlop:blaBla!!@cluster0.scvjk.mongodb.net/?retryWrites=true&w=majority";
        else this.url = process.env.MONGODB_URI;
    }

    public connect() {
        this.mongoClient.connect(this.url, function (err: any, db: {
            db(arg0: string): any; close: () => void;
        }) {
            if (err) throw err;
            var dbo = db.db("mydb");
            // dbo.createCollection("customers", function (err: any, res: any) {
            //     if (err) throw err;
            //     console.log("Collection created!");
            //     db.close();
            // });
            db.close();
        });
    }
}


export const Data = new MongoData();

