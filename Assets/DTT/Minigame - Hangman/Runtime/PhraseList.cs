using System.Collections.Generic;
using UnityEngine;

namespace DTT.Hangman
{
    /// <summary>
    /// Represents a list of phrases usable for a hangman game.
    /// </summary>
    [CreateAssetMenu(menuName = "DTT/Hangman/Wordlist")]
    public class PhraseList : ScriptableObject
    {
        /// <summary>
        /// The theme of the list.
        /// </summary>
        [SerializeField]
        private string _theme;

        /// <summary>
        /// The phrases in the list.
        /// </summary>
        [SerializeField]
        private List<Phrase> _phrases;

        /// <summary>
        /// The theme of the list.
        /// </summary>
        public string Theme => _theme;

        /// <summary>
        /// The amount of phrases in the list.
        /// </summary>
        public int PhraseCount => _phrases.Count;

        /// <summary>
        /// Returns a phrase from the list by index.
        /// </summary>
        /// <param name="index">The index of the phrase element.</param>
        public Phrase this[int index] => _phrases[index];
    }
}
