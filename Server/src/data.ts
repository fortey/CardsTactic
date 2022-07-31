import { ObjectId } from "mongodb";

class MongoData {
    mongoClient: any;
    url: string;
    db: any;

    public initialize() {
        const MongoClient = require('mongodb').MongoClient;


        if (process.env.MONGODB_URI == undefined)
            this.url = "mongodb+srv://6thlop:blaBla!!@cluster0.scvjk.mongodb.net/?retryWrites=true&w=majority";
        else this.url = process.env.MONGODB_URI;

        this.mongoClient = new MongoClient(this.url, { useUnifiedTopology: true }, { connectTimeoutMS: 30000 }, { keepAlive: 1 });
        this.mongoClient.connect();
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
            //await this.mongoClient.connect();
            const users = this.mongoClient.db("userdb").collection("users");
            const user = await this.findUser(users, name);

            if (user == null) {
                // const result = await users.insertOne({ name: name });
                // id = result.insertedId;

                // await this.initialFillingUser(id);
                id = await this.createUser(users, name);
            }
            else
                id = user._id;
        }
        finally {
            //await this.mongoClient.close();
        }

        return id;
    }

    private async findUser(collection: any, name: string) {
        return await collection.findOne({ name: name });
    }

    private async createUser(users: any, name: string) {
        const result = await users.insertOne({ name: name });

        const creatures = [
            { userId: result.insertedId, name: 'Mousy', count: 2 },
            { userId: result.insertedId, name: 'Hell Mousy', count: 2 },
            { userId: result.insertedId, name: 'Swamp Mousy', count: 2 },
            { userId: result.insertedId, name: 'Strong Mouse', count: 1 },
        ];

        await this.mongoClient.db("userdb").collection("user_creatures").insertMany(creatures);

        const squad = {
            userId: result.insertedId,
            name: 'starter',
            board: ['', 'Mousy', 'Hell Mousy', 'Swamp Mousy', 'Strong Mouse', '', '', '']
        }

        await this.mongoClient.db("userdb").collection("squads").insertOne(squad);

        return result.insertedId;
    }

    public async getSquads(id: string): Promise<any> {
        let squads: any = null;
        try {

            const cursor = await this.mongoClient.db("userdb").collection("squads").find({ userId: new ObjectId(id) });
            squads = await cursor.toArray();
        }
        finally {

        }
        return squads;
    }

    public async getUserCreatures(id: string): Promise<any> {
        let creatures: any = null;
        try {

            const cursor = await this.mongoClient.db("userdb").collection("user_creatures").find({ userId: new ObjectId(id) });
            creatures = await cursor.toArray();
        }
        finally {

        }
        return creatures;
    }
}

export const Data = new MongoData();

