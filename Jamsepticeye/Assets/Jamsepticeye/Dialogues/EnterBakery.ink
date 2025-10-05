INCLUDE globals.ink

{ BAKER_DEAD:
It's not safe in there.
-> END
}

{ not KNOWS_ABOUT_BAKER:
Peep: Huh, looks like the lights are off but someone's home...
-> END
}

{ KNOWS_ABOUT_BAKER and not TALKED_TO_BAKER:
T: Hello sir? I’m told a ______ works here?
B: Sorry! We’re closed for today
T: But you’re inside ?
B: Yes, well my assistant seems to have forgotten to order enough ingredients, 
so I can’t open and I can’t leave to grab them myself until said godforsaken assistant arrives… 
but heaven knows the lad sleeps in til noon…
T: What are you missing?
B: Eggs and sugar. Sort of important for a pastry chef are they not?
T: I can go get them for you!
B: Really?
B: Perfect, here’s some money, it should be enough for sugar and eggs at grocerymart.
~ SetTalkedToBaker()
~ TALKED_TO_BAKER = true
-> END
}

{ TALKED_TO_BAKER and not BAKER_DEAD and NEEDS_SUGAR and NEEDS_EGGS:
Peep: C'mon kid, are you even trying? 
We haven't gotten a simple ingredient yet, there's no way 
he'll let you show him a trick at this rate.
-> END
}

{ TALKED_TO_BAKER and NEEDS_SUGAR and HAS_EGGS:
Peep: Sweet as I am, I'm not going into the bowl, kid. Let's go grab some sugar.
-> END
}

{ TALKED_TO_BAKER and HAS_SUGAR and NEEDS_EGGS:
Peep: Still need those eggs Timmy, I'm sure the baker would be stoked for some yolk
-> END
}


{ TALKED_TO_BAKER and HAS_EGGS and HAS_SUGAR:
T: Hello Mr.____ Sir, we got your eggs and sugar! 
B: Oh no way! So quickly too! Come in! Come in!
 ~ ALLOWED_BAKERY = true
 ~ SetAllowBakery()
-> END
}

->END