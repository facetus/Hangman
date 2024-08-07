using System;
using UnityEngine;

namespace DTT.Hangman.Demo
{
    /// <summary>
    /// Controls the scenario for the sunflower demo scene.
    /// </summary>
    public class SunflowerScenarioController : ScenarioController
    {
        /// <summary>
        /// The sprite renderer of the leaves.
        /// </summary>
        [SerializeField]
        private SpriteRenderer _leavesRenderer;

        /// <summary>
        /// The prefab of the petals.
        /// </summary>
        [SerializeField]
        private GameObject _petalPrefab;

        /// <summary>
        /// The transform of the crown of the sunflower.
        /// </summary>
        [SerializeField]
        private Transform _crownTransform;

        /// <summary>
        /// The sprite renderer of the leaves.
        /// </summary>
        public SpriteRenderer LeavesRenderer => _leavesRenderer;

        /// <summary>
        /// Generates the petal behaviours.
        /// </summary>
        /// <param name="index">The index in the ordered array.</param>
        /// <param name="settings">The hangman settings.</param>
        /// <returns>The scenario part behaviour.</returns>
        protected override ScenarioPartBehaviour GeneratePart(int index, HangmanSettings settings)
            => Instantiate(_petalPrefab, _crownTransform).GetComponent<PetalBehaviour>();

        /// <summary>
        /// Clears the petals by destroying them and resetting the ordered parts to an empty array.
        /// </summary>
        public override void Clear()
        {
            for(int i = _crownTransform.childCount - 1; i >= 0; i--)
                Destroy(_crownTransform.GetChild(i).gameObject);

            _orderedParts = Array.Empty<ScenarioPartBehaviour>();
        }
    }
}
