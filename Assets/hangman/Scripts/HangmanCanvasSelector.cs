using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Enneas.Hangman
{
    public class HangmanCanvasSelector : MonoBehaviour
    {
        public GameObject titeTsisggelia;
        public GameObject budgeTsiggelia;
        public GameObject descriptionTsiggelia;

        public GameObject titleRodesia;
        public GameObject budgeRodesia;
        public GameObject descriptionRodesia;

        private bool tsiggeliaInstantiated = false;
        private bool rodesiaInstantiated = false;
        public bool gameQuitted = false;

        /// <summary>
        /// Requires: Gamehunting script
        /// </summary>
        //public GameHunting gameHunting;


        public GameObject gameController;
        void Start()
        {
            gameController = GameObject.Find("GameController");
            //gameHunting = gameController.GetComponent<GameHunting>();
            tsiggeliaInstantiated = false;
            gameQuitted = false;
        }

        void Update()
        {

            if (gameController == null)
            {
                gameController = GameObject.Find("GameHuntingController");
            }

            //if (gameHunting == null) {
            //    gameController.GetComponent<GameHunting>();
            //}

            if ((GameObject.Find("pf_hangman_game_tsiggelia") != null && GameObject.Find("pf_hangman_game_tsiggelia").activeSelf) || (GameObject.Find("pf_hangman_game_tsiggelia(Clone)") != null && GameObject.Find("pf_hangman_game_tsiggelia(Clone)").activeSelf))
            {
                if (tsiggeliaInstantiated == false)
                {
                    titeTsisggelia.SetActive(true);
                    budgeTsiggelia.SetActive(true);
                    descriptionTsiggelia.SetActive(true);

                    titleRodesia.SetActive(false);
                    budgeRodesia.SetActive(false);
                    descriptionRodesia.SetActive(false);
                    tsiggeliaInstantiated = true;
                }

            }
            if ((GameObject.Find("pf_hangman_game_rodesia") != null && GameObject.Find("pf_hangman_game_rodesia").activeSelf) || (GameObject.Find("pf_hangman_game_rodesia(Clone)") != null && GameObject.Find("pf_hangman_game_rodesia(Clone)").activeSelf))
            {
                if (rodesiaInstantiated == false)
                {
                    titleRodesia.SetActive(true);
                    budgeRodesia.SetActive(true);
                    descriptionRodesia.SetActive(true);

                    titeTsisggelia.SetActive(false);
                    budgeTsiggelia.SetActive(false);
                    descriptionTsiggelia.SetActive(false);
                    rodesiaInstantiated = true;
                }
            }
        }

        public void QuitGame()
        {
            StartCoroutine(QuitGameCo());
        }

        IEnumerator QuitGameCo()
        {
            gameQuitted = true;
            //if ((GameObject.Find("/pf_medicine_hangman_controller") != null && GameObject.Find("/pf_medicine_hangman_controller").activeSelf) || (GameObject.Find("/pf_medicine_hangman_controller(Clone)") != null && GameObject.Find("/pf_medicine_hangman_controller(Clone)").activeSelf))
            //{
            //    gameHunting.FinishCurrentGame(1, false);
            //}

            //if ((GameObject.Find("/pf_hangman_controller") != null && GameObject.Find("/pf_hangman_controller").activeSelf) || (GameObject.Find("/pf_hangman_controller(Clone)") != null && GameObject.Find("/pf_hangman_controller(Clone)").activeSelf))
            //{
            //    gameHunting.FinishCurrentGame(2, false);
            //}

            Scene startScene = SceneManager.GetSceneByName("_LEVEL_SELECT");
            if (startScene.isLoaded)
            {
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync("_LEVEL_SELECT");
                // Wait until the scene is unloaded.
                while (!asyncUnload.isDone)
                {
                    yield return null;
                }
            }
        }
    }
}
