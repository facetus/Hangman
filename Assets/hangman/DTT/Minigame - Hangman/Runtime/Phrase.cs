using System;
using System.Linq;

namespace Enneas.Hangman.DTT.Hangman
{
   /// <summary>
   /// Represents a phrase usable for a hangman game.
   /// </summary>
   [Serializable]
   public struct Phrase
   {
      /// <summary>
      /// The phrase value.
      /// </summary>
      public string value;

      /// <summary>
      /// The descriptions of the phrase usable for hints.
      /// </summary>
      public string[] descriptions;

      /// <summary>
      /// The exposed letters of the phrase to be shown
      /// at the start of the game.
      /// </summary>
      public char[] exposedLetters;

      /// <summary>
      /// Returns a character of the phrase based on the character element index.
      /// </summary>
      /// <param name="index">The index.</param>
      public char this[int index] => value[index];

      /// <summary>
      /// The length of the phrase.
      /// </summary>
      public int Length => value.Length;

      /// <summary>
      /// The amount of words in the phrase.
      /// </summary>
      public int WordCount => value.Count(letter => letter == ' ') + 1;

        // Property to check if the phrase is valid (initialized).
        public bool IsValid => !string.IsNullOrEmpty(value);


        /// <summary>
        /// Creates a new phrase instance.
        /// </summary>
        /// <param name="value">The phrase value.</param>
        /// <param name="descriptions">The exposed letters of the phrase to be shown at the start of the game.</param>
        /// <param name="exposedLetters">The exposed letters of the phrase to be shown at the start of the game.</param>
        public Phrase(string value, string[] descriptions = null, char[] exposedLetters = null)
      {
         this.value = value;
         this.descriptions = descriptions;
         this.exposedLetters = exposedLetters;
      }

      /// <summary>
      /// Splits the phrase up in sub phrases. Use this to split up a phrase that is a sentence into words.
      /// </summary>
      /// <returns>The sub phrases.</returns>
      public Phrase[] Split()
      {
         Phrase[] phrases = value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(phrase => new Phrase(phrase))
            .ToArray();

         for (int i = 0; i < phrases.Length; i++)
         {
            phrases[i].descriptions = descriptions;
            phrases[i].exposedLetters = exposedLetters;
         }

         return phrases;
      }

      /// <summary>
      /// Returns a new phrase that exposes the given letters.
      /// </summary>
      /// <param name="exposedLetters">The letters to expose.</param>
      /// <returns>The new phrase exposing the letters.</returns>
      public Phrase Expose(char[] exposedLetters) => new Phrase(value, descriptions, exposedLetters);

      /// <summary>
      /// Returns whether this phrase contains a given letter.
      /// </summary>
      /// <param name="letter">The letter to check for.</param>
      /// <returns>Whether this phrase contains a given letter.</returns>
      public bool Contains(char letter)
      {
         char letterToUpper = char.ToUpperInvariant(letter);
         for (int i = 0; i < value.Length; i++)
            if (char.ToUpperInvariant(value[i]) == letterToUpper)
               return true;

         return false;
      }

      /// <summary>
      /// Returns whether a given letter is exposed by this phrase.
      /// </summary>
      /// <param name="letter">The letter to check for.</param>
      /// <returns>Whether the given letter is exposed by this phrase.</returns>
      public bool Exposes(char letter)
      {
         char letterToUpper = char.ToUpperInvariant(letter);
         for (int i = 0; i < exposedLetters.Length; i++)
            if (char.ToUpperInvariant(exposedLetters[i]) == letterToUpper)
               return true;

         return false;
      }

      /// <summary>
      /// Returns the string value of this phrase.
      /// </summary>
      /// <returns>The string value.</returns>
      public override string ToString() => value;

      public int CountOccurrences(char character)
      {
         return value.Count(c => c == character);
      }


   }
}
