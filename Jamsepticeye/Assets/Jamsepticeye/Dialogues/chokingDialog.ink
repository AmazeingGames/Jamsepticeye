INCLUDE globals.ink


{ 
    - not END_SCENE_SETUP:
        OH NO WHAT'S WRONG???  #speaker:tim #emotion:neutral #layout:left
        Pe\-Peanu\-\-\- #speaker:kid_hurt #emotion:neutral #layout:right
        AHHH HE NEEDS THE NURSE HE DOESN'T LOOK GOOD! #speaker:tim #emotion:neutral #layout:left
        I'M GONNA GO GET HER, HOLD ON! PEEP, PLEASE STAY AND WATCH HIM WHILE IM GONE. 
         ~ END_SCENE_SETUP = true
         ~ SetupEndScene()
    -> END
    
    - END_SCENE_SETUP:
        Timmy is performing beyond my expectations. #speaker:peep #emotion:neutral #layout:right
        He judged the baker without even knowing 
        cracking the skull like an egg was poetic justice, I think.
        In any case, there is far more than meets the eye that poor Timbo won't ever understand.
        Even if it gets its head crushed right in front of him. 
        You should be fine soon kid…. I think… 
        I'm going to see how the boss is faring with the nurse… 
        After all, we still have a lot of work to do. 
        ~EndGame()
        -> END
}

->DONE