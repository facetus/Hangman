using System;
using UnityEngine;

namespace Enneas.Hangman.DTT.Hangman
{
    /// <summary>
    /// An abstract representation of a behaviour that displays a selectable letter.
    /// </summary>
    public abstract class SelectableLetterDisplayer : MonoBehaviour
    {
        /// <summary>
        /// Whether the letter has been selected.
        /// </summary>
        public bool IsSelected { get; private set; }

        /// <summary>
        /// The displayed letter.
        /// </summary>
        public char Letter { get; private set; }

        /// <summary>
        /// Whether the letter should remove itself after being selected.
        /// </summary>
        protected bool _removeOnClick;

        /// <summary>
        /// Callback when the letter has been selected.
        /// </summary>
        private Action _onClickCallback;

        /// <summary>
        /// Sets required variables.
        /// </summary>
        /// <param name="letter">The to be displayed letter.</param>
        /// <param name="onClickCallback">Callback for when the letter is selected.</param>
        /// <param name="removeOnClick">Whether the letter should remove itself after being pressed.</param>
        public virtual void Setup(char letter, Action onClickCallback, bool removeOnClick)
        {
            _removeOnClick = removeOnClick;
            _onClickCallback = onClickCallback;
            Letter = letter;
        }

        /// <summary>
        /// Called when the button is clicked.
        /// </summary>
        protected virtual void OnClicked()
        {
            if (IsSelected)
                return;

            IsSelected = true;
            _onClickCallback?.Invoke();

            OnSelected();
        }

        /// <summary>
        /// Called when the letter has been clicked for the first time.
        /// </summary>
        protected abstract void OnSelected();
    }
}