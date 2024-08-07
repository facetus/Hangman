using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Enneas.Hangman.DTT.Hangman.Demo
{
    /// <summary>
    /// A behaviour managing a hint button.
    /// </summary>
    public class HintButton : MonoBehaviour
    {
        /// <summary>
        /// The game service.
        /// </summary>
        [SerializeField]
        private HangmanService _service;

        /// <summary>
        /// The hint renderer.
        /// </summary>
        [SerializeField]
        public Text _hintRenderer;
        
        /// <summary>
        /// Whether the hint should be random. If set to false, the first hint will always be used.
        /// </summary>
        [SerializeField]
        private bool _randomHint = true;
        
        /// <summary>
        /// The button to be clicked.
        /// </summary>
        private Button _button;

        /// <summary>
        /// Starts listening to the button.
        /// </summary>
        private void Awake()
        {
            //_button = GetComponent<Button>();
            //_button.onClick.AddListener(OnClick);

            string[] descriptions = _service.CurrentPhrase.descriptions;
            if (descriptions == null || descriptions.Length == 0)
            {
                _hintRenderer.text = "No hint available";
            }
            else
            {
                string hint = _randomHint ? descriptions[Random.Range(0, descriptions.Length)] : descriptions[0];

                _hintRenderer.text = hint;
            }
        }

        /// <summary>
        /// Called when the button is clicked, it will get the
        /// the phrase descriptions and display one as hint.
        /// </summary>
        public void OnClick()
        {
            string[] descriptions = _service.CurrentPhrase.descriptions;
            if (descriptions == null || descriptions.Length == 0)
            {
                _hintRenderer.text = "No hint available";
            }
            else
            {
                string hint = _randomHint ? descriptions[Random.Range(0, descriptions.Length)] : descriptions[0];
                _hintRenderer.text = hint;
            }
        }
    }
}
