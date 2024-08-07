using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;


namespace Enneas.Hangman.DTT.Hangman
{

    public class ScenarioController : MonoBehaviour, IGameplaySectionController
    {
        [SerializeField] private int lives;

        // The letter controller.
        [SerializeField] private LetterSectionController _letterController;
        // The behaviours for the scenario parts in order.
        [SerializeField] protected ScenarioPartBehaviour[] _orderedParts;
        // The behaviours for the scenario parts in order.
        public ScenarioPartBehaviour[] OrderedParts => _orderedParts;
        // The amount of unlocked parts.
        public int UnlockedCount => _orderedParts.Count(part => part.Unlocked);
        // The letter controller.
        public LetterSectionController LetterController => _letterController;
        // Fired when the scenario has been completed.
        public event Action Completed;
        // The phrase used for the game.
        protected Phrase p_phrase;
        // The settings used for the game.
        protected HangmanSettings p_settings;
        // The service used for the game.
        protected HangmanService p_service;
        // Whether the scenario has been completed.
        public bool _completed;
        // Starts listening to letter click events.
        private void OnEnable() => _letterController.LetterClicked += OnLetterClicked;
        // Stops listening to letter click events.
        private void OnDisable() => _letterController.LetterClicked -= OnLetterClicked;
        private int _correctGuessCount = 0;
        public int CorrectGuessCount => _correctGuessCount;
        public bool completedSuccessfully = false;
        public bool completedUnSuccessfully = false;
        public int unlockedCount;


        [SerializeField] private GameObject wrongLetter;
        private Coroutine co;
        private Transform wrongLetterInitialPos;
        [SerializeField]
        private Text _hintRenderer;

        [SerializeField] private TextMeshProUGUI livesToDisplay;

        void Start()
        {
            _correctGuessCount = 0;
            completedSuccessfully = false;
            completedUnSuccessfully = false;
            wrongLetterInitialPos = wrongLetter.transform;

            //if (!p_phrase.IsValid)
            //{
            //    Debug.LogError("p_phrase is not initialized. Ensure Generate method is called before Start.");
            //}
        }
        private void Update()
        {
            if (!p_phrase.IsValid)
            {
                //Debug.LogError("p_phrase is invalid in Update method. Ensure Generate method is called before Update.");
                return;
            }

            if (_correctGuessCount == p_phrase.Length)
            {
                completedSuccessfully = true;
                completedUnSuccessfully = false;
                return;
            }


            //Check that again in future?
            /*if (unlockedCount + 1 == p_settings.Lives)
            //if (unlockedCount + 1 == _orderedParts.Length)
            {
                completedUnSuccessfully = true;
                completedSuccessfully = false;
                return;
            }*/

            // Do nothing if the scenario is already completed.
            if (_completed)
                return;

            // Do nothing if no ordered parts are part of the scenario.
            if (_orderedParts.Length == 0)
                return;

            // Check whether all ordered parts have been unlocked and return if any is not.
            for (int i = 0; i < _orderedParts.Length; i++)
                if (!_orderedParts[i].Unlocked)
                    return;
            _completed = true;
            OnCompletion();
            Completed?.Invoke();
        }

        // Regenerates the scenario.
        /// <param name="service">The game service.</param>
        /// <param name="list">The list the phrase is chosen from.</param>
        /// <param name="phrase">The phrase used for the game.</param>
        public void Generate(HangmanService service, PhraseList list, Phrase phrase)
        {
            _completed = false;
            p_service = service;
            p_settings = service.Settings;
            p_phrase = phrase;

            PhraseList[] phraseLists = p_settings.PhraseLists;
            if (p_settings.BaseLivesOnScenarioParts)
            {
                //This sets the amount of lives to be equal to our parts(the sprites);
                //p_settings.Lives = _orderedParts.Length;
                p_settings.Lives = lives;
            }
            else
            {
                GeneratePartsBasedOnLives(p_settings);
            }
            foreach (var part in _orderedParts)
            {
                part.gameObject.SetActive(false);
            }
            _hintRenderer.text = phrase.descriptions[0];
        }

        // Locks all scenario parts.
        public virtual void Clear()
        {
            for (int i = 0; i < _orderedParts.Length; i++)
                _orderedParts[i].Lock();

            Debug.Log("Ordered Parts: " + _orderedParts.Length);
        }

        /// Should return a generated scenario part.
        /// <param name="index">The index of the scenario part in the array.</param>
        /// <param name="settings">The game settings.</param>
        /// <returns>The generated part.</returns>
        protected virtual ScenarioPartBehaviour GeneratePart(int index, HangmanSettings settings) => null;
        // Called when the scenario has been completed.
        protected virtual void OnCompletion() { }
        private void OnLetterClicked(char letter)
        {
            if (p_phrase.Contains(letter))
            {
                _correctGuessCount += p_phrase.CountOccurrences(letter);

                // Check if all letters for this phrase have been correctly guessed
                if (_correctGuessCount == p_phrase.Length)
                {
                    completedSuccessfully = true;
                    completedUnSuccessfully = false;
                    OnCompletion();
                    Completed?.Invoke();
                    return;
                }

                // Find the next scenario part to unlock
                ScenarioPartBehaviour behaviourToUnlock = null;
                for (int i = 0; i < _orderedParts.Length; i++)
                {
                    if (!_orderedParts[i].Unlocked)
                    {
                        behaviourToUnlock = _orderedParts[i];
                        break;
                    }
                }

                if (behaviourToUnlock == null)
                    return;

                // Enable and unlock the next scenario part
                behaviourToUnlock.gameObject.SetActive(true);
                behaviourToUnlock.Unlock(p_service);

                // Disable the previous part if it exists
                int indexOfCurrentPart = Array.IndexOf(_orderedParts, behaviourToUnlock);
                if (indexOfCurrentPart > 0)
                {
                    ScenarioPartBehaviour previousPart = _orderedParts[indexOfCurrentPart - 1];
                    previousPart.gameObject.SetActive(false);
                }

                // Check how many parts have been unlocked
                unlockedCount = _orderedParts.Count(part => part.Unlocked);

                //Debug.Log("Correct letter clicked!");
                return;
            }

            // If the letter is incorrect, handle the wrong letter logic (optional, as per your implementation)
            setLives(Lives() - 1);

            Debug.Log("Lives: " + Lives());
            if (Lives() > 0)
            {
                if (co != null)
                {
                    wrongLetter.transform.position = wrongLetterInitialPos.position;
                    StopCoroutine(co);
                    wrongLetter.transform.DOKill();
                }
                wrongLetter.SetActive(false);
                co = StartCoroutine(DisplayWrongLetter());
            }

            // Handle scenario completion or other logic for incorrect guesses
        }

        IEnumerator DisplayWrongLetter()
        {
            wrongLetter.SetActive(true);

            /// summary
            /// Requires LanguageManager script
            ///
            //if(LanguageManager.CurrentLanguage == Language.Greek)
            //{
            //    Debug.Log(LanguageManager.CurrentLanguage);
            //    if (Lives() != 1)
            //    {
            //        livesToDisplay.text = "Απομένουν " + Lives() + " ζωές!";
            //    }
            //    else
            //        livesToDisplay.text = "Απομένει " + Lives() + " ζωή!";
            //}
            //else
            //{
            //    if (Lives() != 1)
            //    {
            //        livesToDisplay.text = Lives() + " lives remain!";
            //    }
            //    else
            //        livesToDisplay.text = Lives() + " life remain!";
            //}



            Vector3 startPosition = wrongLetter.transform.position;
            Vector3 endPosition = startPosition + new Vector3(0, 40f, 0);

            // Move the wrongLetter GameObject
            wrongLetter.transform.position = startPosition;
            wrongLetter.transform.DOMove(endPosition, 1f).SetEase(Ease.OutQuad);

            yield return new WaitForSeconds(1f);

            // Reset position to start position
            wrongLetter.transform.position = startPosition;
            wrongLetter.SetActive(false);
        }

        // Generates scenario parts based on lives amount set in the settings.
        private void GeneratePartsBasedOnLives(HangmanSettings settings)
        {
            _orderedParts = new ScenarioPartBehaviour[settings.Lives];

            for (int i = 0; i < _orderedParts.Length; i++)
                _orderedParts[i] = GeneratePart(i, settings);
        }

        /// <summary>
        /// Looks for and returns a behaviour to unlock and fills a list of currently unlocking behaviours while doing so.
        /// </summary>
        /// <param name="behavioursUnlocking">The unlocking behaviours found.</param>
        /// <returns>The behaviour to unlock.</returns>
        private ScenarioPartBehaviour FindBehaviourUnlockStates(List<ScenarioPartBehaviour> behavioursUnlocking)
        {
            ScenarioPartBehaviour behaviourToUnlock = null;
            for (int i = 0; i < OrderedParts.Length; i++)
            {
                ScenarioPartBehaviour partBehaviour = _orderedParts[i];
                if (partBehaviour.Unlocking)
                {
                    behavioursUnlocking.Add(partBehaviour);
                    continue;
                }
                if (!partBehaviour.Unlocked)
                {
                    behaviourToUnlock = partBehaviour;
                    break;
                }
            }
            return behaviourToUnlock;
        }

        /// <summary>
        /// Waits for a given list of scenario part behaviours to be unlocked before
        /// calling a given action.
        /// </summary>
        /// <param name="unlockingBehaviours">The list of scenario part behaviours to wait for.</param>
        /// <param name="action">The action to invoke after waiting.</param>
        /// <returns>Waits until all scenario parts are unlocked.</returns>
        private IEnumerator WaitForUnlockingBehaviours(List<ScenarioPartBehaviour> unlockingBehaviours, Action<HangmanService> action)
        {
            while (unlockingBehaviours.All(behaviour => behaviour != null && !behaviour.Unlocked))
                yield return null;

            action.Invoke(p_service);
        }
        public int CountOccurrences(char character)
        {
            return p_phrase.value.Count(c => c == character);
        }

        public Phrase GetPhrase()
        {
            return p_phrase;
        }

        public void ResetCorrectGuessCount()
        {
            _correctGuessCount = 0;
        }

        public int Lives()
        {
            return p_settings.Lives;
        }

        public void setLives(int liveCount)
        {
            p_settings.Lives = liveCount;

            if (p_settings.Lives == 0)
            {
                StopCoroutine(DisplayWrongLetter());
                wrongLetter.SetActive(false);
                //If reach 0 lives then we go to the gameover;
                HandleGameOver();
            }
        }

        public void resetLives()
        {
            if (p_settings != null)
            {
                p_settings.Lives = 4;
            }
        }

        private void HandleGameOver()
        {
            //We mark our variables as we failed to complete the level;
            completedUnSuccessfully = true;
            completedSuccessfully = false;

            //Mark the scenario as completed to stop further updates;
            _completed = true;

        }
    }

    //}
}
