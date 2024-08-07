using UnityEngine;

namespace DTT.Hangman
{
    /// <summary>
    /// The settings usable for a hangman game.
    /// </summary>
    [CreateAssetMenu(menuName = "DTT/Hangman/Settings")]
    public class HangmanSettings : ScriptableObject
    {
        /// <summary>
        /// All the lower case letters of the alphabet.
        /// We use 2 different alphabets, one for greek and one for english;
        /// </summary>
        /// 

        public const string ALPHABET = "αβγδεζηθικλμνξοπρστυφχψω";
        public const string ALPHABETENG = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// All lower case vowels.
        /// </summary>
        public const string VOWELS = "αεηιουω";
        public const string VOWELSENG = "AEIOU";

        /// <summary>
        /// Whether the hangman game should use a time constrained.
        /// </summary>
        public bool UsesTimeConstrained => _timeConstrained != 0.0f;
        
        /// <summary>
        /// Whether to show the vowels at the start of the game.
        /// </summary>
        [Header("Display")]
        [Tooltip("Whether to show the vowels at the start of the game.")]
        [SerializeField]
        private bool _showVowels = false;

        /// <summary>
        /// Whether to show the theme the word belongs to.
        /// </summary>
        [Tooltip("Whether to show the theme the word belongs to.")]
        [SerializeField]
        private bool _showTheme = true;

        /// <summary>
        /// Whether to still show the letters after they are selected or not.
        /// </summary>
        [Tooltip("Whether to still show the letters after they are selected or not.")]
        [SerializeField]
        private bool _removeLettersOnSelected = false;

        /// <summary>
        /// The time constrained in seconds. Setting this to a value other
        /// than 0 will tell the game service to use a timer.
        /// </summary>
        [Tooltip("The time constrained in seconds. Setting this to a value other than 0 will tell the game service to use a timer.")]
        [SerializeField]
        private float _timeConstrained = 300.0f;

        /// <summary>
        /// The casing to use for letters in the game.
        /// </summary>
        [Header("Styling")]
        [Tooltip("The casing to use for letters in the game.")]
        [SerializeField]
        private LetterCasing _casing;
        
        /// <summary>
        /// Whether to use all letters of the alphabet to choose from.
        /// </summary>
        [Header("User Input")]
        [Tooltip("Whether to use all letters of the alphabet to choose from.")]
        [SerializeField]
        private bool _useAlphabet = true;
        
        /// <summary>
        /// The amount of additional letters to use when not using the alphabet to choose from.
        /// </summary>
        [Min(0)]
        [Tooltip("The amount of additional letters to use when not using the alphabet to choose from.")]
        [SerializeField]
        private int _additionalLetters = 0;

        /// <summary>
        /// The characters to include in the keyboard to choose from.
        /// </summary>
        [Tooltip("The characters to include in the keyboard to choose from.")]
        [SerializeField]
        private string _customCharacters;

        /// <summary>
        /// Determines whether the scenario parts should be generated based on the amount of lives set,
        /// or set the amount of lives to the scenario parts set up in the scene.
        /// </summary>
        [Header("Scenario")]
        [SerializeField, Tooltip("Determines whether the scenario parts should be generated based on the amount of lives set, or set the amount of lives to the scenario parts set up in the scene.")]
        private bool _baseLivesOnScenarioParts;

        /// <summary>
        /// The amount of lives the user has visualized in the scenario degeneration process.
        /// </summary>
        [Tooltip("The amount of lives the user has visualized in the scenario degeneration process.")]
        [Min(1)]
        [SerializeField]
        private int _lives = 12;

        /// <summary>
        /// The lists of phrases to choose from.
        /// </summary>
        [Header("Phrases")]
        [SerializeField, Tooltip("The lists of phrases to choose from.")]
        public PhraseList[] _phraseLists;

        /// <summary>
        /// The amount of lives the user has visualized in the scenario degeneration process.
        /// </summary>
        public int Lives
        {
            get => _lives;
            internal set => _lives = value;
        }

        /// <summary>
        /// Whether to show the vowels at the start of the game.
        /// </summary>
        public bool ShowVowels => _showVowels;

        /// <summary>
        /// Whether to show the theme the word belongs to.
        /// </summary>
        public bool ShowTheme => _showTheme;

        /// <summary>
        /// Whether to still show the letters after they are selected or not.
        /// </summary>
        public bool RemoveLettersOnSelected => _removeLettersOnSelected;

        /// <summary>
        /// The time constrained in seconds. Setting this to a value other
        /// than 0 will tell the game service to use a timer.
        /// </summary>
        public float TimeConstrained => _timeConstrained;

        /// <summary>
        /// Whether to use all letters of the alphabet to choose from.
        /// </summary>
        public bool UseAlphabet => _useAlphabet;

        /// <summary>
        /// The amount of additional letters to use when not using the alphabet to choose from.
        /// </summary>
        public int AdditionalLetters => _additionalLetters;

        /// <summary>
        /// <see cref="_customCharacters"/>
        /// </summary>
        public string CustomCharacters => _customCharacters;
        
        /// <summary>
        /// Determines whether the scenario parts should be generated based on the amount of lives set,
        /// or set the amount of lives to the scenario parts set up in the scene.
        /// </summary>
        public bool BaseLivesOnScenarioParts => _baseLivesOnScenarioParts;

        /// <summary>
        /// The lists of phrases to choose from.
        /// </summary>
        public PhraseList[] PhraseLists => _phraseLists;
        
        /// <summary>
        /// The casing to use for letters in the game.
        /// </summary>
        public LetterCasing Casing => _casing;
    }
}
