using System;
using UnityEngine;

namespace DTT.Hangman
{
    /// <summary>
    /// Represents a behaviour that controls a phrase section.
    /// </summary>
    public abstract class PhraseSectionController : MonoBehaviour, IGameplaySectionController
    {   
        /// <summary>
        /// Whether the phrase is completely displayed.
        /// </summary>
        public bool IsPhraseCompleted { get; private set; }

        /// <summary>
        /// Fired when the phrase for the game has been completely displayed.
        /// </summary>
        public event Action PhraseCompleted;

        /// <summary>
        /// The phrase for the game.
        /// </summary>
        protected Phrase p_phrase;

        /// <summary>
        /// The game settings.
        /// </summary>
        protected HangmanSettings p_settings;

        /// <summary>
        /// The game service.
        /// </summary>
        protected HangmanService p_service;
        
        /// <summary>
        /// Checks whether the phrase has been completed and fires the phrase-completed
        /// event if detected.
        /// </summary>
        private void Update()
        {
            if (p_service == null || !p_service.IsGameActive)
                return;
            
            if (IsPhraseCompleted)
                return;

            if (GetIsPhraseCompleted())
            {
                IsPhraseCompleted = true;
                PhraseCompleted?.Invoke();
            }
        }
        
        /// <summary>
        /// Regenerates the phrase section.
        /// </summary>
        /// <param name="service">The game service.</param>
        /// <param name="list">The list the phrase is chosen from.</param>
        /// <param name="phrase">The phrase for the game.</param>
        public void Generate(HangmanService service, PhraseList list, Phrase phrase)
        {
            IsPhraseCompleted = false;
            
            p_service = service;
            p_settings = service.Settings;
            p_phrase = phrase;
            
            phrase.value = p_settings.Casing.ApplyTo(phrase.value);
            OnGeneratePhrase(service, list, phrase);
        }

        /// <summary>
        /// Clears the section.
        /// </summary>
        public abstract void Clear();

        /// <summary>
        /// Returns whether the phrase has been completed.
        /// </summary>
        /// <returns>Whether the phrase has been completed.</returns>
        protected abstract bool GetIsPhraseCompleted();

        /// <summary>
        /// Called to start regenerating the phrase section.
        /// </summary>
        /// <param name="service">The game service.</param>
        /// <param name="list">The phrase list the phrase was chosen from.</param>
        /// <param name="phrase">The phrase for the game.</param>
        protected internal abstract void OnGeneratePhrase(HangmanService service, PhraseList list, Phrase phrase);
    }
}
