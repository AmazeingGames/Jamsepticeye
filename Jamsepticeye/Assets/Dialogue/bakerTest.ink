INCLUDE globals.ink

Hello there! #speaker:baker #emotion:neutral #layout:right 
-> main

=== main ===
How are you feeling today?
+ [Neutral]
    ~ playEmote("exclamation")
    That makes me feel <color=\#F8FF30>neutral</color> as well! #emotion:neutral
+ [Mad]
    Oh, well that makes me <color=\#5B81FF>mad</color> too. #emotion:mad
    
- Don't trust him, he's <b><color=\#FF1E35>not</color></b> a real doctor! #speaker:peeper #emotion:mad #layout:right 
~ playEmote("question")
Well, do you have any more questions? #speaker:baker #emotion:neutral #layout:left 
+ [Yes]
    -> main
+ [No]
    Goodbye then!
    ~ playEmote("exclamation")
    -> END