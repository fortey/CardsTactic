import { Room, Client, Delayed } from "colyseus";
import { CreatureSchema, GameRoomState } from "./schema/GameRoomState";
import { Board } from "../game/board";
import { abilities } from "../game/abilities/abilities";
import { CreatureFactory } from "../game/creatureFactory";
import { NetworkedUser } from "./schema/NetworkedUser";
import { GameEvents } from "../game/gameEvents";

const TURN_TIMEOUT = 10
const BOARD_WIDTH = 5;

export class gameRoom extends Room<GameRoomState> {

  maxClients = 2;
  randomMoveTimeout: Delayed;
  board = new Board();
  bot = false;
  maxPass = 2;
  passes = new Map<string, number>();
  started = false;
  movedCell: number = null;

  onCreate(options: any) {
    this.setState(new GameRoomState());
    this.onMessage("action", (client, message) => this.playerAction(client, message));
    this.onMessage("select_cell", (client, message) => this.onSelectCell(client, message));
    this.onMessage("move", (client, message) => this.onMove(client, message));
    this.onMessage("ability_clicked", (client, message) => this.onAbilityClicked(client, message));
    this.onMessage("action", (client, message) => this.onAction(client, message));
    this.onMessage("pass", (client, message) => this.pass(client));

    this.bot = options["bot"];
    //this.roomName = options["name"];
  }

  onJoin(client: Client, options: any) {
    this.state.players.set(client.sessionId, true);

    let newNetworkedUser = new NetworkedUser().assign({
      id: client.id,
      sessionId: client.sessionId,
    });

    this.state.networkedUsers.set(client.sessionId, newNetworkedUser);
    client.send("onJoin", newNetworkedUser);

    this.passes.set(client.sessionId, this.maxPass);

    if (this.bot) {
      this.state.players.set("bot", true);
      this.passes.set("bot", this.maxPass);
    }

    if (this.state.players.size === 2) {
      this.state.currentTurn = client.sessionId;
      this.setAutoMoveTimeout();

      this.lock();

      const playerIds = Array.from(this.state.players.keys());

      let creature = CreatureFactory["Hell Mousy"]("3", playerIds[0]);
      this.state.creatures.set(creature.id, creature);
      this.state.board[0] = creature.id;

      creature = CreatureFactory["Mousy"]("4", playerIds[0]);
      this.state.creatures.set(creature.id, creature);
      this.state.board[1] = creature.id;

      creature = CreatureFactory["Mousy"]("1", playerIds[1]);
      this.state.creatures.set(creature.id, creature);
      this.state.board[18] = creature.id;

      creature = CreatureFactory["Hell Mousy"]("2", playerIds[1]);
      this.state.creatures.set(creature.id, creature);
      this.state.board[19] = creature.id;

      this.broadcast("start");
      this.started = true;
    }
    console.log(client.sessionId, "joined!");
  }

  onLeave(client: Client, consented: boolean) {
    this.state.players.delete(client.sessionId);
    this.state.networkedUsers.delete(client.sessionId);

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
  }


  pass(client: Client) {
    if (this.state.currentTurn !== client.sessionId) return;
    let selected: CreatureSchema = null;

    if (this.movedCell != null) {
      selected = this.state.creatures.get(this.state.board[this.movedCell]);
    }

    if (selected !== null)
      this.state.creatures.forEach(creature => {
        if (selected === null && creature.active && creature.owner === client.sessionId && creature.health > 0) {
          selected = creature;
        }
      });
    if (selected !== null) selected.active = false;

    this.TurnDone(client);
  }

  setAutoMoveTimeout() {
    if (this.randomMoveTimeout) {
      this.randomMoveTimeout.clear();
    }

    let hasActive = false;
    this.state.creatures.forEach(creature => { if (creature.active && creature.owner === this.state.currentTurn) hasActive = true; });

    if (!hasActive && this.started) {
      this.TurnDone({ sessionId: this.state.currentTurn } as Client);
      return;
    }
    const timeout = (this.state.currentTurn === "bot" ? 1 : TURN_TIMEOUT) * 1000;
    this.randomMoveTimeout = this.clock.setTimeout(() => this.pass({ sessionId: this.state.currentTurn } as Client), timeout);
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

  }

  onSelectCell(client: Client, cell: number) {

    const creatureID = this.state.board[cell];
    const creature = this.state.creatures.get(creatureID);
    if (creature != null && creature.owner === client.sessionId && creature.active && creature.steps > 0 && (this.movedCell == null || this.movedCell == cell)) {
      client.send("available_cells", this.board.availableCellsForMove(this.state.board, cell));
    }
  }

  onMove(client: Client, data: number[]) {

    const creatureID = this.state.board[data[0]];
    const creature = this.state.creatures.get(creatureID);
    if (creature != null && creature.owner === client.sessionId && creature.active && creature.steps > 0 && (this.movedCell == null || this.movedCell == data[0])) {
      const available_cells = this.board.availableCellsForMove(this.state.board, data[0]);
      if (available_cells.find(i => i == data[1]) !== undefined) {
        this.state.board[data[0]] = "";
        this.state.board[data[1]] = creatureID;
        creature.steps--;

        this.movedCell = data[1];
      }
    }
  }

  onAbilityClicked(client: Client, data: any[]) {
    const creatureID = this.state.board[data[0]];
    const creature = this.state.creatures.get(creatureID);
    if (creature != null && creature.active && creature.owner === client.sessionId && (this.movedCell == null || this.movedCell == data[0])) {
      if (data[1] === "pass") {
        creature.active = false;
        this.TurnDone(client);
        return;
      }
      if (data[1] === "defense") {
        creature.defense = true;
        creature.active = false;
        this.TurnDone(client);
        return;
      }
      if (data[1] === "take_points") {
        creature.active = false;
        creature.points++;
        this.TurnDone(client);
        return;
      }
      if (abilities[data[1]] !== undefined) {
        abilities[data[1]].onClicked(data[0], creature, this.state, this.board, (targets: number[]) => client.send("available_targets", targets));
      }
    }
  }

  onAction(client: Client, data: any[]) {
    if (this.state.currentTurn !== client.sessionId)
      return;
    const cellSource = data[0];
    const action = data[1];
    const cellTarget = data[2];

    const creatureID = this.state.board[cellSource];
    const creature = this.state.creatures.get(creatureID);

    if (creature != null && creature.active && creature.owner === client.sessionId && (this.movedCell == null || this.movedCell == data[0])) {

      if (abilities[action].invoke(cellSource, creature, this.state, this.board, cellTarget)) {
        this.broadcast("action", [cellSource, cellTarget]);
        creature.active = false;
        this.TurnDone(client);
      }
    }
  }

  TurnDone(client: Client) {
    if (this.CheckEndRound())
      this.EndRound();

    if (this.state.currentTurn !== client.sessionId) return;

    this.movedCell = null;

    const playerIds = Array.from(this.state.players.keys());

    this.state.currentTurn = (client.sessionId === playerIds[0]) ? playerIds[1] : playerIds[0];

    this.setAutoMoveTimeout();
  }

  CheckEndRound(): boolean {
    let allInactive = true;
    this.state.creatures.forEach(creature => {
      if (creature.active && creature.health > 0 && (!this.bot || creature.owner != "bot")) allInactive = false;
    });
    return allInactive;
  }

  EndRound() {
    //this.passes.forEach((value, key) => this.passes.set(key, this.maxPass));

    this.state.creatures.forEach(creature => {
      if (!creature.active) creature.active = true;
      creature.steps = creature.maxSteps;
      creature.defense = false;
    });

    GameEvents.onStartRound(this);
  }
}
