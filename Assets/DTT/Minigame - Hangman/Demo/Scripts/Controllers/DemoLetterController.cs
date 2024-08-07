using System;
using UnityEngine;
using UnityEngine.UI;

//namespace DTT.Hangman.Demo
namespace Enneas.Hangman.DTT.Hangman
{
    /// <summary>
    /// A behaviour controlling the letters in the demo scene.
    /// </summary>
    public class DemoLetterController : LetterSectionController
    {
        /// Represents a letter to displayed in the scene.
        private struct Letter
        {
            /// The prefab used by the letters.
            public static GameObject prefab;

            /// The parent used by the letters.
            public static Transform parent;

            /// A callback for the letter being clicked.
            private Action<char> _clickedCallback;

            /// The letter game object instance.
            private GameObject _instance;

            /// The letter character value.
            private char _value;
            

            /// Creates a new instance of the letter.
            /// <param name="value">The letter value.</param>
            /// <param name="clickedCallback">A callback for the letter being clicked.</param>
            /// <param name="removeOnClick">Whether the letter should disappear on click.</param>
            public Letter(char value, Action<char> clickedCallback, bool removeOnClick)
            {
                _clickedCallback = clickedCallback;
                _value = value;

                _instance = Instantiate(prefab, parent);
                _instance.GetComponent<SelectableLetterDisplayer>().Setup(value, OnLetterClick, removeOnClick);
            }
            /// Destroys the letter.
            public void Destroy() => GameObject.Destroy(_instance);

            /// Called when the letter is being clicked to Invoke the callback and destroy the letter.
            private void OnLetterClick() => _clickedCallback.Invoke(_value);
        }

        /// <summary>
        /// The letters being displayed for the user to be clicked.
        private Letter[] _letters;

        /// <summary>
        /// Sets the static letter references.
        /// </summary>
        private void Awake()
        {
            // Set shared values between letters.
            Letter.prefab = LetterPrefab;
            Letter.parent = transform;
        }

        /// <summary>
        /// Called when the letters should be regenerated, it settings up
        /// the letters based on the game settings and letter characters.
        /// </summary>
        /// <param name="settings">The game settings.</param>
        /// <param name="letters">The letters to choose from.</param>
        protected override void OnGenerateLetters(HangmanSettings settings, char[] letters)
        {
            _letters = new Letter[letters.Length];
            for (int i = 0; i < letters.Length; i++)
            {
                char characterValue = settings.Casing.ApplyTo(letters[i]);
                _letters[i] = new Letter(characterValue, OnLetterClicked, settings.RemoveLettersOnSelected);
            }
        }

        /// <summary>
        /// Destroys old letters on the field.
        /// </summary>
        public override void Clear()
        {
            if (_letters == null)
                return;

            for (int i = 0; i < _letters.Length; i++)
                _letters[i].Destroy();

            _letters = null;
        }
    }
}


