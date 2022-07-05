import { Room, Client, Delayed } from "colyseus";
import { GameRoomState, CreatureSchema, NetworkedUser } from "./schema/GameRoomState";
import { Board } from "../game/Board";

const TURN_TIMEOUT = 10
const BOARD_WIDTH = 3;

export class gameRoom extends Room<GameRoomState> {

  maxClients = 2;
  randomMoveTimeout: Delayed;
  board = new Board();

  onCreate(options: any) {
    this.setState(new GameRoomState());
    this.onMessage("action", (client, message) => this.playerAction(client, message));
    this.onMessage("select_cell", (client, message) => this.onSelectCell(client, message));
    this.onMessage("move", (client, message) => this.onMove(client, message));

    const creature = new CreatureSchema();
    creature.id = "1";
    creature.name = "Orc";
    creature.active = true;
    creature.health = 10;
    this.state.board[1] = creature.id;

    // this.state.players.forEach((value, key) => {
    //   creature.owner = key; console.log(key);
    // });

    this.state.creatures.set(creature.id, creature);

  }

  onJoin(client: Client, options: any) {
    this.state.players.set(client.sessionId, true);

    let newNetworkedUser = new NetworkedUser().assign({
      id: client.id,
      sessionId: client.sessionId,
    });

    this.state.networkedUsers.set(client.sessionId, newNetworkedUser);
    client.send("onJoin", newNetworkedUser);

    this.state.creatures.get("1").owner = client.sessionId;

    if (this.state.players.size === 1) {
      this.state.currentTurn = client.sessionId;
      this.setAutoMoveTimeout();

      // lock this room for new users
      this.lock();

      client.send("start");
    }
    console.log(client.sessionId, "joined!");
  }

  onLeave(client: Client, consented: boolean) {
    this.state.players.delete(client.sessionId);

    if (this.randomMoveTimeout) {
      this.randomMoveTimeout.clear()
    }

    let remainingPlayerIds = Array.from(this.state.players.keys());
    if (!this.state.winner && !this.state.draw && remainingPlayerIds.length > 0) {
      this.state.winner = remainingPlayerIds[0]
    }
    console.log(client.sessionId, "left!");
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }

  playerAction(client: Client, data: any) {
    if (this.state.winner || this.state.draw) {
      return false;
    }

    if (client.sessionId === this.state.currentTurn) {

      const orc = this.state.creatures.get("1");
      orc.health--;

      const playerIds = Array.from(this.state.players.keys());

      const index = data.x + BOARD_WIDTH * data.y;

      if (this.state.board[index] === '') {
        const move = (client.sessionId === playerIds[0]) ? '1' : '2';
        this.state.board[index] = move;

        if (this.checkWin(data.x, data.y, move)) {
          this.state.winner = client.sessionId;

        } else if (this.checkBoardComplete()) {
          this.state.draw = true;

        } else {
          // switch turn
          //const otherPlayerSessionId = (client.sessionId === playerIds[0]) ? playerIds[1] : playerIds[0];

          //this.state.currentTurn = otherPlayerSessionId;

          this.setAutoMoveTimeout();
        }

      }
    }
  }

  setAutoMoveTimeout() {
    if (this.randomMoveTimeout) {
      this.randomMoveTimeout.clear();
    }

    //this.randomMoveTimeout = this.clock.setTimeout(() => this.doRandomMove(), TURN_TIMEOUT * 1000);
  }

  checkBoardComplete() {
    return this.state.board
      .filter(item => item === '0')
      .length === 0;
  }

  doRandomMove() {
    const sessionId = this.state.currentTurn;
    for (let x = 0; x < BOARD_WIDTH; x++) {
      for (let y = 0; y < BOARD_WIDTH; y++) {
        const index = x + BOARD_WIDTH * y;
        if (this.state.board[index] === '0') {
          this.playerAction({ sessionId } as Client, { x, y });
          return;
        }
      }
    }
  }

  checkWin(x: number, y: number, move: string) {
    let won = false;
    let board = this.state.board;

    // horizontal
    for (let y = 0; y < BOARD_WIDTH; y++) {
      const i = x + BOARD_WIDTH * y;
      if (board[i] !== move) { break; }
      if (y == BOARD_WIDTH - 1) {
        won = true;
      }
    }

    // vertical
    for (let x = 0; x < BOARD_WIDTH; x++) {
      const i = x + BOARD_WIDTH * y;
      if (board[i] !== move) { break; }
      if (x == BOARD_WIDTH - 1) {
        won = true;
      }
    }

    // cross forward
    if (x === y) {
      for (let xy = 0; xy < BOARD_WIDTH; xy++) {
        const i = xy + BOARD_WIDTH * xy;
        if (board[i] !== move) { break; }
        if (xy == BOARD_WIDTH - 1) {
          won = true;
        }
      }
    }

    // cross backward
    for (let x = 0; x < BOARD_WIDTH; x++) {
      const y = (BOARD_WIDTH - 1) - x;
      const i = x + BOARD_WIDTH * y;
      if (board[i] !== move) { break; }
      if (x == BOARD_WIDTH - 1) {
        won = true;
      }
    }

    return won;
  }

  onSelectCell(client: Client, cell: number) {

    const creatureID = this.state.board[cell];
    const creature = this.state.creatures.get(creatureID);
    if (creature != null && creature.owner === client.sessionId) {
      client.send("available_cells", this.board.availableCellsForMove(this.state.board, cell));
    }
  }

  onMove(client: Client, data: number[]) {

    const creatureID = this.state.board[data[0]];
    const creature = this.state.creatures.get(creatureID);
    if (creature != null && creature.owner === client.sessionId) {
      const available_cells = this.board.availableCellsForMove(this.state.board, data[0]);
      if (available_cells.find(i => i == data[1]) !== undefined) {
        this.state.board[data[0]] = "";
        this.state.board[data[1]] = creatureID;
      }
    }
  }
}
