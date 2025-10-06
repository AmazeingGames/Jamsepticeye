INCLUDE globals.ink

{ 
    - TALKED_TO_BAKER and FOUND_NEST and NEEDS_ROCKS:
        Hey ! One of those could surely knock down the nest! go grab one! #speaker:peep #emotion:neutral #layout:left 
        ~ NEEDS_ROCKS = false
        ~ HAS_ROCKS = true
        ~ SetHasRocks()
        -> END

    - TALKED_TO_BAKER and not FOUND_NEST and NEEDS_ROCKS:
        Ooo these rocks are cool. I could use them for a disappearing act #speaker:tim #emotion:neutral #layout:left 
        that would really knock someone's socks off
        ~ NEEDS_ROCKS = false
        ~ HAS_ROCKS = true
        ~ SetHasRocks()
        -> END

    - not TALKED_TO_BAKER and FOUND_NEST and NEEDS_ROCKS:
        Ooo these rocks are cool. I could use them for a disappearing act #speaker:tim #emotion:neutral #layout:left 
        that would really knock someone's socks off
        ~ NEEDS_ROCKS = false
        ~ HAS_ROCKS = true
        ~ SetHasRocks()
        -> END

    - not TALKED_TO_BAKER and not FOUND_NEST and NEEDS_ROCKS:
        Ooo these rocks are cool. I could use them for a disappearing act #speaker:tim #emotion:neutral #layout:left 
        that would really knock someone's socks off
        ~ NEEDS_ROCKS = false
        ~ HAS_ROCKS = true
        ~ SetHasRocks()
        -> END
}

->DONE