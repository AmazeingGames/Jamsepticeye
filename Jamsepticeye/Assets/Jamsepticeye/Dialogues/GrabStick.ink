INCLUDE globals.ink

{ TALKED_TO_BAKER and FOUND_NEST and NEEDS_STICKS:
Oh hey, if we attach your cape with some sticks, we can make a safety net to catch the nest. #speaker:peep #emotion:neutral #layout:right
~ NEEDS_STICKS = false
~ HAS_STICKS = true
~ SetHasSticks()
-> END
}


{ TALKED_TO_BAKER and not FOUND_NEST and NEEDS_STICKS:
I can maybe make some extra wands with these sticks! Maybe you would want one, Peep? #speaker:tim #emotion:neutral #layout:left
I'm happy youre trying to give me a weapon kid, but i'm unarmed in more of a literal sense #speaker:peep #emotion:neutral #layout:right
-> END
}

{ not TALKED_TO_BAKER and FOUND_NEST and NEEDS_STICKS:
I can maybe make some extra wands with these sticks! Maybe you would want one, Peep? #speaker:tim #emotion:neutral #layout:left
I'm happy youre trying to give me a weapon kid, but i'm unarmed in more of a literal sense #speaker:peep #emotion:neutral #layout:right
~ NEEDS_STICKS = false
~ HAS_STICKS = true
~ SetHasSticks()
-> END
}

{ not TALKED_TO_BAKER and not FOUND_NEST and NEEDS_STICKS:
I can maybe make some extra wands with these sticks! Maybe you would want one, Peep? #speaker:tim #emotion:neutral #layout:left
I'm happy youre trying to give me a weapon kid, but i'm unarmed in more of a literal sense #speaker:peep #emotion:neutral #layout:right
~ NEEDS_STICKS = false
~ HAS_STICKS = true
~ SetHasSticks()
-> END
}

->DONE