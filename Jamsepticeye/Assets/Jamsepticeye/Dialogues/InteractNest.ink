INCLUDE globals.ink

{
    - not PLACED_HAMMOCK and HAS_STICKS:
        Build hammock? #speaker:tim #emotion:neutral #layout:left 
        * [Yes]
            ~ PLACED_HAMMOCK = true
            ~ HAS_STICKS = false
            ~ SetHammockPlaced()
            -> END
            
        * [No]
            -> END
            
        -> END
    - PLACED_HAMMOCK and not HAS_STICKS and HAS_ROCKS:
        Throw rock at nest? #speaker:tim #emotion:neutral #layout:left 
        * [Yes]
            ~ HAS_ROCKS = false
            ~ NEST_ROCKING_STARTS = true
            ~ SetNestRocked()
            -> END
            
        * [No]
            -> END
            
        -> END
    - PLACED_HAMMOCK and not HAS_STICKS and not HAS_ROCKS:
        Those eggs look PERFECT for the baker. #speaker:peep #emotion:neutral #layout:right
        Yea but I can't reach them :c #speaker:tim #emotion:sad #layout:left
        No big, I'm sure we can find something to knock it down #speaker:peep #emotion:neutral #layout:right
        -> END
        ~ FOUND_NEST = true
        ~ SetFoundNest()
    - not PLACED_HAMMOCK and not HAS_STICKS and HAS_ROCKS:
        I know you have to crack an egg to make an omelette, but we're baking right now. #speaker:peep #emotion:neutral #layout:right
        Your cape is large enough to catch them, we just need to attach it to something to soften the blow
        ~ FOUND_NEST = true
        ~ SetFoundNest()
        -> END
    - not NEEDS_EGGS and not PLACED_HAMMOCK and not HAS_STICKS and not HAS_ROCKS:
        Wow that nest sure is high up :o #speaker:peep #emotion:neutral #layout:right
        ~ FOUND_NEST = true
        ~ SetFoundNest()
        -> END
    - not PLACED_HAMMOCK and not HAS_STICKS and not HAS_ROCKS and NEEDS_EGGS:
        Those eggs look PERFECT for the baker. #speaker:peep #emotion:neutral #layout:right
        ~ FOUND_NEST = true
        ~ SetFoundNest()
        -> END
}

-> DONE