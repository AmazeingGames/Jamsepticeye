INCLUDE globals.ink

{ NEEDS_EGGS:
Wow that nest sure is high up :o
}

{ not PLACED_HAMMOCK and HAS_STICKS:
Build hammock?
* [Yes]
    ~ PLACED_HAMMOCK = true
    ~ HAS_STICKS = false
    ~ SetHammockPlaced()
  -> END

* [No]
  -> END
}


{ PLACED_HAMMOCK and not HAS_STICKS and HAS_ROCKS:
Throw rock at nest?
* [Yes]
    ~ HAS_ROCKS = false
    ~ NEST_ROCKED = true
    ~ SetNestRocked()
  -> END

* [No]
  -> END
}

{ PLACED_HAMMOCK and not HAS_STICKS and not HAS_ROCKS:
Peep: Those eggs look PERFECT for the baker.
TIM: yea but I can't reach them :c
Peep: No big, I'm sure we can find something to knock it down
}

{ not PLACED_HAMMOCK and not HAS_STICKS and HAS_ROCKS:
I know you have to crack an egg to make an omelette, but we're baking right now.
Your cape is large enough to catch them, we just need to attach it to something to soften the blow
}

->DONE