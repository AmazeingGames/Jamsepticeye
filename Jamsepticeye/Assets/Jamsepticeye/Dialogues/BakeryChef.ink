INCLUDE globals.ink


{ 
    - HAS_EGGS and HAS_SUGAR:
        Thank you lad! I am grateful for your assistance! #speaker:baker #emotion:neutral #layout:right
        Please, wait there while I prepare you something as a token of my appreciation.
        I heard you like magic! #speaker:tim #emotion:neutral #layout:left
        Why of course, I am a baker after all! #speaker:baker #emotion:neutral #layout:right
        Baking is basically akin to alchemy, a sort of magic in its own right.
        Woahhh. I can do magic like a baker! Can I show you? #speaker:tim #emotion:neutral #layout:left
        Why of course, show me what you got after I’m done preparing this!  #speaker:baker #emotion:neutral #layout:right
         ~ HAS_EGGS = false
         ~ HAS_SUGAR = false
         ~ GiveIngredientsToBaker()
        -> END


    - not BAKER_DEAD and not HAS_EGGS and not HAS_SUGAR and not FLOUR_MAGIC_READY:
        Alright kid, let’s see some magic! #speaker:baker #emotion:neutral #layout:right
        I need something heavy to lift with my levitating powers. #speaker:tim #emotion:neutral #layout:left
        All I’s gots is this jar of flour. #speaker:baker #emotion:neutral #layout:right
        Do with it what you will, I’m looking forward to it.  
         ~ FLOUR_MAGIC_READY = true
         ~ PrepareFlourMagicTrick()
        -> END

    - not BAKER_DEAD and FLOUR_MAGIC_READY:
        Wha- he fell asleep! #speaker:tim #emotion:neutral #layout:left
        I know I’m new to magic, but it wasn’t THAT boring of a trick, right?
        Don’t worry about it, kid. 
        A true genius is never appreciated in their time. #speaker:peep #emotion:surprise #layout:right
        ~ BAKER_DEAD = true
        ~ SetBakerDead()
        -> END
}

->DONE