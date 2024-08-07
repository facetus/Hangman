This is the main repository for the mini game Hangman that can easily be imported to other projects.

How to change the phrases:
  1.In the HangmanService gameobject you can see the "Settings" variable, follow that which will take you to the Scriptable Object
  2.Duplicate the Scriptable Object, and then doubleclick the "Phrase Lists" Element.
  3.This element contains all the words for our current list. Duplicate it and insert your own words
  4. In the "Game" gameobject set the "Configs" variable to the new Scriptable Object that you created.

How to change the keyboard:
  1. In the LetterSectionController, look for the GetLettersForGeneration function, this returns an array based on whether the game is in Greek or English(can add more languages).
  2. To create a new array: Go to HangmanSettings script and create a new constant string;
