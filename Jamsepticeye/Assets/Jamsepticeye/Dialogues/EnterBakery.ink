INCLUDE globals.ink

{ 
    - BAKER_DEAD:
        It's not safe in there. #speaker:peep #emotion:neutral #layout:right
        -> END


    - not KNOWS_ABOUT_BAKER:
        Huh, looks like the lights are off but someone's home... #speaker:peep #emotion:neutral #layout:right
        -> END

    - KNOWS_ABOUT_BAKER and not TALKED_TO_BAKER:
        Hello sir? I’m told a Bjorn works here? #speaker:tim #emotion:neutral #layout:left
        Sorry! We’re closed for today #speaker:baker #emotion:neutral #layout:right
        But you’re inside?  #speaker:tim #emotion:neutral #layout:left
        Yes, well my assistant seems to have forgotten to order enough ingredients, #speaker:baker #emotion:neutral #layout:right
        so I can’t open and I can’t leave to grab them myself until said godforsaken assistant arrives… 
        but heaven knows the lad sleeps in til noon…
        What are you missing?  #speaker:tim #emotion:neutral #layout:left
        Eggs and sugar. Sort of important for a pastry chef are they not? #speaker:baker #emotion:neutral #layout:right
        I can go get them for you!  #speaker:tim #emotion:neutral #layout:left
        Really?  #speaker:baker #emotion:neutral #layout:right
        Perfect, here’s some money, it should be enough for sugar and eggs at grocerymart. #speaker:baker #emotion:neutral #layout:right
        ~ SetTalkedToBaker()
        ~ TALKED_TO_BAKER = true
        -> END

    - TALKED_TO_BAKER and not BAKER_DEAD and NEEDS_SUGAR and NEEDS_EGGS:
        C'mon kid, are you even trying? #speaker:peep #emotion:neutral #layout:right
        We haven't gotten a simple ingredient yet, there's no way 
        he'll let you show him a trick at this rate.
        -> END
        
    - TALKED_TO_BAKER and NEEDS_SUGAR and HAS_EGGS:
        Sweet as I am, I'm not going into the bowl, kid. Let's go grab some sugar. #speaker:peep #emotion:neutral #layout:right
        -> END

    - TALKED_TO_BAKER and HAS_SUGAR and NEEDS_EGGS:
        Still need those eggs Timmy, I'm sure the baker would be stoked for some yolk #speaker:peep #emotion:neutral #layout:right
        -> END


    - TALKED_TO_BAKER and HAS_EGGS and HAS_SUGAR:
        Hello Mr.____ Sir, we got your eggs and sugar!  #speaker:tim #emotion:neutral #layout:left
        Oh no way! So quickly too! Come in! Come in! #speaker:baker #emotion:neutral #layout:right
        ~ ALLOWED_BAKERY = true
        ~ SetAllowBakery()
        -> END
}

->END