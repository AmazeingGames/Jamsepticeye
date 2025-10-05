INCLUDE globals.ink

{ HAS_COFFEE == false: -> main | -> nothing }

=== main ===
There's a sign that says free samples:

"We seem to have a surplus of this Toppa the Mornin' coffee (not sponsored)
because SOMEONE (kevin) CAN'T DO THEIR JOB RIGHT.
Take one on the house... please... we beg you"

~ SetHasCoffee()
~ HAS_COFFEE = true
-> END

=== nothing ===
-> END