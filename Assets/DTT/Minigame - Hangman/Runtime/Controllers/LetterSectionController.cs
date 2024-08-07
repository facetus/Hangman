using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using DTT.MinigameBase.LevelSelect;
using Unity.VisualScripting;
using UnityEngine.UI;

namespace DTT.Hangman
{
    /// <summary>
    /// Represents a behaviour that controllers a section of letters for the user to choose from.
    /// </summary>
    public abstract class LetterSectionController : MonoBehaviour, IGameplaySectionController
    {
        public bool isGreek;

         public IEnumerable<char> characters;
        /// <summary>
        /// The prefab of the letter.
        /// </summary>
        [SerializeField]
        private GameObject _letterPrefab;

        public HashSet<char> _guessedLetters = new HashSet<char>();

        public IReadOnlyCollection<char> GuessedLetters => _guessedLetters;

        /// <summary>
        /// The prefab of the letter.
        /// </summary>
        public GameObject LetterPrefab => _letterPrefab;

        /// <summary>
        /// Fired when a letter has been clicked by the user.
        /// </summary>
        public event Action<char> LetterClicked;

        /// <summary>
        /// The phrase of the game.
        /// </summary>
        public Phrase p_phrase;

        /// <summary>
        /// The settings of the game.
        /// </summary>
        public HangmanSettings p_settings;

        /// <summary>
        /// The game service.
        /// </summary>
        protected HangmanService p_service;

        /// <summary>
        /// Regenerates the phrase section.
        /// </summary>
        /// <param name="service">The game service.</param>
        /// <param name="list">The list the phrase is chosen from.</param>
        /// <param name="phrase">The phrase for the game.</param>
        public void Generate(HangmanService service, PhraseList list, Phrase phrase)
        {

            p_service = service;

            p_settings = service.Settings;
            p_phrase = phrase;

            OnBeforeGenerateLetters(p_settings);

            // If the user doesn't use the alphabet, additional letters are taken at random.
            char[] letters = GetLettersForGeneration(p_settings, phrase);

            // Sort the letters alphabetically. 
            Array.Sort(letters);

            OnGenerateLetters(p_settings, letters);
        }

        /// <summary>
        /// Clears the letter section.
        /// </summary>
        public virtual void Clear() { }

        /// <summary>
        /// Called before the letters are regenerated.
        /// </summary>
        /// <param name="settings">The game settings.</param>
        protected virtual void OnBeforeGenerateLetters(HangmanSettings settings) { }

        /// <summary>
        /// Called when letters have for the game have been decided on to regenerate the letter displays.
        /// </summary>
        /// <param name="settings">The game settings.</param>
        /// <param name="letters">The letters to choose from.</param>
        protected abstract void OnGenerateLetters(HangmanSettings settings, char[] letters);

        /// <summary>
        /// Called when a letter has been clicked to invoke the letter-clicked event.
        /// </summary>
        /// <param name="letter">The clicked letter.</param>
        protected void OnLetterClicked(char letter)
        {
            // Do nothing if the service is paused.
            if (p_service.IsPaused)
                return;

            _guessedLetters.Add(letter);
            LetterClicked?.Invoke(letter);
        }

        /// <summary>
        /// Returns the letters selectable by the user based on the game
        /// settings and the phrase used.
        /// </summary>
        /// <param name="settings">The game settings.</param>
        /// <param name="phrase">The phrase to generated the letters for.</param>
        /// <returns>The selectable letters.</returns>
        public char[] GetLettersForGeneration(HangmanSettings settings, Phrase phrase)
        {
            if (settings.UseAlphabet)
            {
                characters = HangmanSettings.ALPHABET.ToCharArray();
                ///Require: Language Manager to use the below. When you insert the script, remove the line 124;
                ////We need to check if we use english or greek;
                //if (isGreek)
                //{
                //    characters = HangmanSettings.ALPHABET.ToCharArray();
                //} else
                //{
                //    characters = HangmanSettings.ALPHABETENG.ToCharArray();

                //}
            }
            else
            {
                string wordToLower = phrase.value.ToLower();
                string wordWithoutWhiteSpace = Regex.Replace(wordToLower, @"\s+", "");
                characters = HangmanSettings.ALPHABET
                    .Except(wordWithoutWhiteSpace)
                    .TakeRandom(p_settings.AdditionalLetters)
                    .Concat(wordWithoutWhiteSpace)
                    .Concat(settings.CustomCharacters)
                    .Distinct();
            }

            return characters.Except(phrase.exposedLetters).ToArray();
        }
    }
}
