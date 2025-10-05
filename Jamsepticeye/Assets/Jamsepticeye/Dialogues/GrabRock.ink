INCLUDE globals.ink

{ TALKED_TO_BAKER and FOUND_NEST and NEEDS_ROCKS:
Peep: Hey ! One of those could surely knock down the nest! go grab one!
~ NEEDS_ROCKS = false
~ HAS_ROCKS = true
~ SetHasRocks()
}


{ TALKED_TO_BAKER and not FOUND_NEST and NEEDS_ROCKS:
Tim:  Ooo these rocks are cool. I could use them for a disappearing act
that would really knock someone's socks off
}

{ not TALKED_TO_BAKER and FOUND_NEST and NEEDS_ROCKS:
Tim:  Ooo these rocks are cool. I could use them for a disappearing act
that would really knock someone's socks off
~ NEEDS_ROCKS = false
~ HAS_ROCKS = true
~ SetHasRocks()
}

{ not TALKED_TO_BAKER and not FOUND_NEST and NEEDS_ROCKS:
Tim:  Ooo these rocks are cool. I could use them for a disappearing act
that would really knock someone's socks off
~ NEEDS_ROCKS = false
~ HAS_ROCKS = true
~ SetHasRocks()
}

->DONE