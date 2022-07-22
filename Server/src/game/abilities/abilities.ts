import { Ability } from "./ability";
import { melee } from "./melee";
import { poisoning } from "./poisoning";
import { regeneration } from "./regeneration";
import { shot } from "./shot";


export const abilities: { [key: string]: Ability } = {
    "melee": melee,
    "shot": shot,
    "regeneration": regeneration,
    "poisoning": poisoning,
}

