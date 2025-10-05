INCLUDE globals.ink

{ KNOWS_ABOUT_BAKER == false: -> main | -> already_knows }

=== main ===
Man, they really cracked down on waste these days.. Hey kid, you got any food on you?
I usually get something from Bjorn's bakery but today he's not open yet…
I'm wondering what's wrong.
Anyway, if you find any food can you report back to me before I start eating this trashbin?
~ SetKnowsAboutBaker()
~ KNOWS_ABOUT_BAKER = true
-> END

=== already_knows ===
Hey! Didja find anything?

* {HAS_SUGAR == true} [Give him sugar]
  Ew, you expect me to straight shot that? <>Even I have standards! Come back when you find some real food.
  -> END

* {HAS_COFFEE == true} [Give him coffee]
  Nasty, that stuff's for grownups. I don't understand the hype anyway, it's WAY too crunchy for me.
  -> END

* {HAS_COOKIES == true} [Give him cookies]
  Now we're talking! Thanks, kid!
~ HAS_COOKIES = false
~ SetKidFed()
  
  -> END

* [Leave]
  -> END