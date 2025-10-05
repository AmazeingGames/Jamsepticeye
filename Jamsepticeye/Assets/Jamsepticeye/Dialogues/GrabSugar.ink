INCLUDE globals.ink



{ NEEDS_SUGAR == true: -> main | -> nothing }

=== main ===
Peep: Hell yea, gimme some sug'!!! for only 3$ ??

~ SetHasSugar()
~ HAS_SUGAR = true
~ NEEDS_SUGAR = false
-> END

=== nothing ===
-> END