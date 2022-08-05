import { AbilitySchema, CreatureSchema } from "../rooms/schema/GameRoomState"

export const CreatureFactory = new Map([
    ["Mousy", function (id: string, owner: string): CreatureSchema {
        const creature = new CreatureSchema();
        creature.id = id;
        creature.owner = owner;
        creature.name = "Mousy";
        creature.active = true;
        creature.maxHealth = 10;
        creature.health = creature.maxHealth;
        creature.maxSteps = 2;
        creature.steps = creature.maxSteps;

        creature.attributes.push("shot_protection");

        let ability = new AbilitySchema();
        ability.name = "melee";
        ability.values.push(2);
        ability.values.push(3);
        ability.values.push(3);
        creature.abilities.push(ability);

        return creature;
    }],

    ["Hell Mousy", function (id: string, owner: string): CreatureSchema {
        const creature = new CreatureSchema();
        creature.id = id;
        creature.owner = owner;
        creature.name = "Hell Mousy";
        creature.active = true;
        creature.maxHealth = 8;
        creature.health = creature.maxHealth;
        creature.maxSteps = 2;
        creature.steps = creature.maxSteps;

        let ability = new AbilitySchema();
        ability.name = "melee";
        ability.values.push(1);
        ability.values.push(2);
        ability.values.push(3);
        creature.abilities.push(ability);

        ability = new AbilitySchema();
        ability.name = "shot";
        ability.minRange = 2;
        ability.maxRange = 3;
        ability.values.push(2);
        ability.values.push(3);
        ability.values.push(3);
        creature.abilities.push(ability);

        return creature;
    }],

    ["Swamp Mousy", function (id: string, owner: string): CreatureSchema {
        const creature = new CreatureSchema();
        creature.id = id;
        creature.owner = owner;
        creature.name = "Swamp Mousy";
        creature.active = true;
        creature.maxHealth = 6;
        creature.health = creature.maxHealth;
        creature.maxSteps = 1;
        creature.steps = creature.maxSteps;
        creature.points = 0;

        let ability = new AbilitySchema();
        ability.name = "melee";
        ability.values.push(1);
        ability.values.push(2);
        ability.values.push(2);
        creature.abilities.push(ability);

        ability = new AbilitySchema();
        ability.name = "poison_shot";
        ability.needPoints = 1;
        ability.minRange = 2;
        ability.maxRange = 3;
        ability.values.push(1);
        ability.values.push(2);
        ability.values.push(2);
        creature.abilities.push(ability);

        ability = new AbilitySchema();
        ability.name = "regeneration";
        ability.values.push(1);
        creature.passiveAbilities.push(ability);

        return creature;
    }],

    ["Strong Mouse", function (id: string, owner: string): CreatureSchema {
        const creature = new CreatureSchema();
        creature.id = id;
        creature.owner = owner;
        creature.name = "Strong Mouse";
        creature.active = true;
        creature.maxHealth = 12;
        creature.health = creature.maxHealth;
        creature.maxSteps = 1;
        creature.steps = creature.maxSteps;

        let ability = new AbilitySchema();
        ability.name = "melee";
        ability.values.push(2);
        ability.values.push(3);
        ability.values.push(4);
        creature.abilities.push(ability);

        ability = new AbilitySchema();
        ability.name = "regeneration";
        ability.values.push(1);
        creature.passiveAbilities.push(ability);

        return creature;
    }],
]);
