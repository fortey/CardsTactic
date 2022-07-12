import { Ability } from "./ability";
import { melee } from "./melee";
import { shot } from "./shot";


export const abilities: { [key: string]: Ability } = {
    "melee": melee,
    "shot": shot,
}

