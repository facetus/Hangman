using System;
using System.Collections.Generic;
using System.Diagnostics;
using DTT.MinigameBase;
using DTT.MinigameBase.UI;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace DTT.Hangman
{
   /// <summary>
   /// Provides the core gameplay services usable for a hangman game.
   /// </summary>
   public class HangmanService : MonoBehaviour, IMinigame<HangmanSettings, HangmanResult>, IFinishedable, IRestartable
   {

      /// <summary>
      /// The controller managing the letter section.
      /// </summary>
      [SerializeField]
      private LetterSectionController _letterSection;

      /// <summary>
      /// The controller managing the phrase section.
      /// </summary>
      [SerializeField]
      private PhraseSectionController _phraseSection;

      /// <summary>
      /// The controller managing the scenario.
      /// </summary>
      [SerializeField]
      private ScenarioController _scenario;

      /// <summary>
      /// The default settings used for the game.
      /// </summary>
      [SerializeField]
      private HangmanSettings _settings;

      /// <summary>
      /// Whether the game is currently paused.
      /// </summary>
      public bool IsPaused => _isPaused;

      /// <summary>
      /// Whether the game is currently active.
      /// </summary>
      public bool IsGameActive => !_isPaused && _isActive;

      /// <summary>
      /// The elapsed amount of seconds since the game has started.
      /// 0 if no time constrained has been set.
      /// </summary>
      public double ElapsedSeconds => _stopwatch.Elapsed.TotalSeconds;

      /// <summary>
      /// The settings currently used for the game.
      /// </summary>
      public HangmanSettings Settings => _activeSettings != null ? _activeSettings : _settings;
      public HangmanSettings SettingsEng => _activeSettings != null ? _activeSettings : _settings;
        /// <summary>
        /// Called when the game has finished returning the result of the game.
        /// </summary>
        public event Action<HangmanResult> Finish;
      
      /// <summary>
      /// Called when the game has started.
      /// </summary>
      public event Action Started;

      /// <summary>
      /// Called when the game is paused.
      /// </summary>
      public event Action<bool> Paused;
      
      /// <summary>
      /// Called when the game is finished.
      /// </summary>
      public event Action Finished;

      /// <summary>
      /// The stopwatch used when a time constrained has been set.
      /// </summary>
      private readonly Stopwatch _stopwatch = new Stopwatch();

      /// <summary>
      /// The sections to be controlled for the game.
      /// </summary>
      private readonly List<IGameplaySectionController> _sections = new List<IGameplaySectionController>();

      /// <summary>
      /// Whether the game is currently paused.
      /// </summary>
      private bool _isPaused = false;

      /// <summary>
      /// Whether the game is active.
      /// </summary>
      private bool _isActive = false;

      /// <summary>
      /// The settings currently used for the game.
      /// </summary>
      private HangmanSettings _activeSettings;

      /// <summary>
      /// The current phrase used for the game.
      /// </summary>
      public Phrase CurrentPhrase { get; private set; }

      [SerializeField] private GameObject hintSection;
      [SerializeField] private GameObject backgroundUI;
      [SerializeField] private GameObject elapsedSecUI;
      /// <summary>
      /// Adds required sections.
      /// </summary>
      protected virtual void Awake()
      {
         AddSection(_letterSection);
         AddSection(_phraseSection);
         AddSection(_scenario);

         hintSection.SetActive(false);
         backgroundUI.SetActive(false);
         elapsedSecUI.SetActive(false);
         
         _phraseSection.PhraseCompleted += ForceFinish;
         _scenario.Completed += ForceFinish;
      }

      /// <summary>
      /// Starts the game using given settings.
      /// </summary>
      /// <param name="settings">The settings to use for the game.</param>
      public void StartGame(HangmanSettings settings)
      {
         if (IsGameActive)
         {
            Debug.LogWarning("Can't start if a game is already active.");
            return;
         }
         
         if (settings == null)
         {
            Debug.LogWarning("Can't start when when the game settings are null.");
            return;
         }

         PhraseList list = RetrieveList(settings.PhraseLists);
         Phrase phrase = RetrieveWord(list);

         if (settings.ShowVowels)
            phrase = phrase.Expose(HangmanSettings.VOWELS.ToCharArray());
         
         // Generate sections.
         for(int i = 0; i < _sections.Count; i++)
            _sections[i].Generate(this, list, phrase);

         if(settings.UsesTimeConstrained)
            _stopwatch.Restart();

         CurrentPhrase = phrase;
                  
         _isActive = true;
         _activeSettings = settings;

         Started?.Invoke();
         hintSection.SetActive(true);
         backgroundUI.SetActive(true);
         elapsedSecUI.SetActive(true);
      }

      /// <summary>
      /// Adds a section to the hangman service to receive callbacks.
      /// </summary>
      /// <param name="controller">The gameplay part</param>
      public void AddSection(IGameplaySectionController controller) => _sections.Add(controller);

      /// <summary>
      /// Starts the game using the default settings.
      /// </summary>
      public void StartGame() => StartGame(_settings);

      /// <summary>
      /// Restarts the game using the default settings.
      /// </summary>
      public void Restart() => Restart(_activeSettings);

      /// <summary>
      /// Restarts the game using given settings.
      /// </summary>
      /// <param name="settings">The settings to use for the game.</param>
      public void Restart(HangmanSettings settings)
      {
         Cleanup();
         Continue();
         StartGame(settings);
      }

      /// <summary>
      /// Clears the game sections.
      /// </summary>
      public void Cleanup()
      {
         _isActive = false;
         
         for(int i = 0; i < _sections.Count; i++)
            _sections[i].Clear();
         hintSection.SetActive(false);
         backgroundUI.SetActive(false);
         elapsedSecUI.SetActive(false);
      }

      /// <summary>
      /// Does the stopwatch update.
      /// </summary>
      private void Update() => OnStopWatchUpdate();

      /// <summary>
      /// Forces a finish if the stopwatch is running and the total amount of seconds
      /// has become greater than the time constrained set.
      /// </summary>
      private void OnStopWatchUpdate()
      {
         if (!_stopwatch.IsRunning)
            return;
         
         if(_stopwatch.Elapsed.TotalSeconds >= _settings.TimeConstrained)
            ForceFinish();
      }

      /// <summary>
      /// Pauses the game.
      /// </summary>
      public void Pause()
      {
         if (_isPaused)
            return;
         
         if(_settings.UsesTimeConstrained)
            _stopwatch.Stop();

         _isPaused = true;
         Paused?.Invoke(true);
      }

      /// <summary>
      /// Continues the game.
      /// </summary>
      public void Continue()
      {
         if (!_isPaused)
            return;
         
         if(_settings.UsesTimeConstrained)
            _stopwatch.Start();

         _isPaused = false;
         Paused?.Invoke(false);
      }
      
      /// <summary>
      /// Forces the game to finish invoking all relevant events.
      /// </summary>
      public void ForceFinish()
      {
         if(_settings.UsesTimeConstrained)
            _stopwatch.Stop();
         
         _isActive = false;

         Finished?.Invoke();
         Finish?.Invoke(new HangmanResult
         {
            finishedPhrase = _phraseSection.IsPhraseCompleted,
            wrongGuesses = _scenario.UnlockedCount,
            timeTaken = ElapsedSeconds
         });
      }

      /// <summary>
      /// Returns a phrase list to play the game with out of a given array of lists.
      /// By default this will return a random list from the array.
      /// </summary>
      /// <param name="lists">The lists to choose from.</param>
      /// <returns>The list to play the game with.</returns>
      protected virtual PhraseList RetrieveList(PhraseList[] lists) => lists[Random.Range(0, lists.Length)];

      /// <summary>
      /// Returns a phrase to play the game with out of a given list of phrases.
      /// By default this will return a random phrase from the list.
      /// </summary>
      /// <param name="list">The list to choose a phrase from.</param>
      /// <returns>The phrase to play the game with.</returns>
      protected virtual Phrase RetrieveWord(PhraseList list) => list[Random.Range(0, list.PhraseCount)];
   }
}
