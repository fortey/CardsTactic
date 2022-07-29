
class MongoData {
    mongoClient: any;
    url: string;
    db: any;

    public initialize() {
        const MongoClient = require('mongodb').MongoClient;


        if (process.env.MONGODB_URI == undefined)
            this.url = "mongodb+srv://6thlop:blaBla!!@cluster0.scvjk.mongodb.net/?retryWrites=true&w=majority";
        else this.url = process.env.MONGODB_URI;

        this.mongoClient = new MongoClient(this.url, { useUnifiedTopology: true });

        //this.connect();
    }

    public connect() {
        this.mongoClient.connect((err: any, client: {
            db(arg0: string): any; close: () => void;
        }) => {
            if (err) throw err;
            this.db = client.db("mydb");
        });
    }

    public async FindOrCreateUser(name: string): Promise<any> {
        let id = null;
        try {
            await this.mongoClient.connect();
            const users = this.mongoClient.db("userdb").collection("users");
            const user = await this.findUser(users, name);

            if (user == null) {
                const result = await users.insertOne({ name: name });
                id = result.insertedId;
            }
            else
                id = user._id;
        }
        finally {
            this.mongoClient.close();
        }

        return id;
    }

    public async findUser(collection: any, name: string) {
        return await collection.findOne({ name: name });
    }
}


export const Data = new MongoData();

