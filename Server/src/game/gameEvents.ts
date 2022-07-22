import { gameRoom } from "../rooms/gameRoom";
import { abilities } from "./abilities/abilities";

export const GameEvents = {

    onStartRound(gameRoom: gameRoom) {
        gameRoom.state.creatures.forEach((creature, creatureID) => {
            if (creature.health > 0) {
                creature.passiveAbilities.forEach(abilitySchema => {
                    const ability = abilities[abilitySchema.name];
                    if (ability.onStartTurn) {
                        ability.invoke(null, creature, gameRoom.state, null, null);
                    }
                });
            }
        });
    }
};