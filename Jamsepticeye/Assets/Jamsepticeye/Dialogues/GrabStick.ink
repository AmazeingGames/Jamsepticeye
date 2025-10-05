INCLUDE globals.ink

{ TALKED_TO_BAKER and FOUND_NEST and NEEDS_STICKS:
Peep: Oh hey, if we attach your cape with some sticks, we can make a safety net to catch the nest
~ NEEDS_STICKS = false
~ HAS_STICKS = true
~ SetHasSticks()
}


{ TALKED_TO_BAKER and not FOUND_NEST and NEEDS_STICKS:
Tim : I can maybe make some extra wands with these sticks! Maybe you would want one, Peep?
PEEP:  I'm happy youre trying to give me a weapon kid, but i'm unarmed in more of a literal sense"
}

{ not TALKED_TO_BAKER and FOUND_NEST and NEEDS_STICKS:
Tim : I can maybe make some extra wands with these sticks! Maybe you would want one, Peep?
PEEP:  I'm happy youre trying to give me a weapon kid, but i'm unarmed in more of a literal sense"
~ NEEDS_STICKS = false
~ HAS_STICKS = true
~ SetHasSticks()
}

{ not TALKED_TO_BAKER and not FOUND_NEST and NEEDS_STICKS:
Tim : I can maybe make some extra wands with these sticks! Maybe you would want one, Peep?
PEEP:  I'm happy youre trying to give me a weapon kid, but i'm unarmed in more of a literal sense"
~ NEEDS_STICKS = false
~ HAS_STICKS = true
~ SetHasSticks()
}

->DONE