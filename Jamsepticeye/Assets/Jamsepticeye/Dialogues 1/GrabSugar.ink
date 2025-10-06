INCLUDE globals.ink



{ NEEDS_SUGAR == true: -> main | -> nothing }

=== main ===
"This bag for 3$, in this economy?! This place is going out of business!" #speaker:peep #emotion:surprise #layout:right

~ SetHasSugar()
~ HAS_SUGAR = true
~ NEEDS_SUGAR = false
-> END

=== nothing ===
-> END