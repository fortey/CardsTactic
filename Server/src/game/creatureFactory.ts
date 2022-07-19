import { AbilitySchema, CreatureSchema } from "../rooms/schema/GameRoomState"

export const CreatureFactory = {

    "Mousy"(id: string, owner: string): CreatureSchema {
        const creature = new CreatureSchema();
        creature.id = id;
        creature.owner = owner;
        creature.name = "Mousy";
        creature.active = true;
        creature.maxHealth = 10;
        creature.health = 10;
        creature.maxSteps = 1;
        creature.steps = creature.maxSteps;

        let ability = new AbilitySchema();
        ability.name = "melee";
        ability.values.push(1);
        ability.values.push(2);
        ability.values.push(3);
        creature.abilities.push(ability);

        ability = new AbilitySchema();
        ability.name = "shot";
        ability.values.push(2);
        ability.values.push(3);
        ability.values.push(3);
        creature.abilities.push(ability);

        return creature;
    },

    "Hell Mousy"(id: string, owner: string): CreatureSchema {
        const creature = new CreatureSchema();
        creature.id = id;
        creature.owner = owner;
        creature.name = "Hell Mousy";
        creature.active = true;
        creature.maxHealth = 10;
        creature.health = 10;
        creature.maxSteps = 2;
        creature.steps = creature.maxSteps;

        let ability = new AbilitySchema();
        ability.name = "melee";
        ability.values.push(1);
        ability.values.push(2);
        ability.values.push(3);
        creature.abilities.push(ability);

        return creature;
    }
}