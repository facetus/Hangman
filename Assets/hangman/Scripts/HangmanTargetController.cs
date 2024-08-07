using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DTT.MinigameBase.LevelSelect;
using Unity.VisualScripting;
using DTT.Hangman.Demo;

// THE FOLLOWING SCRIPT HANDLES THE HANGMAN GAME TARGETS
// IN V.01 THE GAME HAS TWO HANGMAN GAMES: FOR HERBS AND FOR MEDICINE
// IT TAKES AS AN INPUT THE NAME OF THE GAMEOBJECT THE SCRIPT IS ATTACHED 
// TO AND PERFORMS THE CORRESPONDING ACTION ACCORDING TO THAT
namespace DTT.Hangman
{
    public class HangmanTargetController : MonoBehaviour
    {



        public virtual bool Complete { get; set; } // whether the game is completed or not

        private GameObject achievementManagerObject;

        //[SerializeField] private GameObject budgeBeltManagerObject;
        [SerializeField] private GameObject scenarioControllerObject;

        /// <summary>
        /// Requires: Achievement Manager script
        /// </summary>
        //private AchievementManager achievementManager;
       
        //private achievementsManaget budgeBeltManager;


        private ScenarioController scenarioController;
        public bool completedSuccessfully;
        public bool completedUnSuccessfully;
        [SerializeField] private GameObject popUp;
        private bool gameConcluded = false;
        public HangmanService hangmanService;

        bool endScreenActivated = false;

        /// <summary>
        /// Requires: FadingComponentscript
        /// </summary>
        
        //FadingComponent fm;

        [SerializeField] private LetterSectionController letterSectionController;

        private bool hasWon;
        [SerializeField]
        private HintButton hint;

        // END SCREENS 
        // HERBS END SCENES
        // [0] -> Δίκταμο
        // [1] -> Βαλσαμόχορτο
        // [2] -> Μανδραγόρας
        // [3] -> Χαμομήλι

        // MEDICINE END SCENES
        // [0] -> Παρακεταμόλη
        // [1] -> Ασπιρίνη

        public GameObject[] SuccessScreen;
        public GameObject[] FailureScreen;
        string[] herbPhrases = { "ΤΣΙΓΚΕΛΙΑ", "ΡΟΔΕΣΙΑ", "ΜΑΝΔΡΑΓΟΡΑΣ", "ΧΑΜΟΜΗΛΙ" };
        string[] testPhrases = { "TSIGΚELIA", "RODESIA", "MANDRAKE", "CHAMOMILE" };


        string[] medicinePhrases = { "TEST", "ENGLISH" };

        public int score;

        /// <summary>
        /// Requires: ScoreSystemscript
        /// </summary>
        //private ScoreSystem scoreSystem;

        //GAME SCREEN 
        [SerializeField] private GameObject MainDismissButton;
        [SerializeField] private GameObject MainPauseButton;
        [SerializeField] private GameObject PhraseSection;
        [SerializeField] private GameObject HintSection;
        [SerializeField] private GameObject LetterSection;
        [SerializeField] private GameObject ScenarioController;

        /// <summary>
        /// Requires: Gamehunting script
        /// </summary>
        //private GameHunting gameHunting2;
        int prefabIndex = -1;

        [SerializeField] private HangmanLevelSelect hangmanLevelSelect;

        public string sceneToDisplay;

        [SerializeField] private string sceneToActivate = "_LEVEL_SELECT";
        [SerializeField] private string achievementKey = "escapee";
        [SerializeField] int budgeIndex;

        /// <summary>
        /// Requires: BadgeCaseHanlder script
        /// </summary>
        //private BagdeCaseHandler bagdeCaseHandler;


        void Start()
        {
            gameConcluded = false;
            Complete = false;

            //Initialize Achivement Manager;
            achievementManagerObject = GameObject.Find("Achievement Manager");

            //bagdeCaseHandler = FindObjectOfType<BagdeCaseHandler>();

            //Initialize Score System;
            //scoreSystem = FindObjectOfType<ScoreSystem>();

            //gameHunting2 = FindObjectOfType<GameHunting>();
            //fm = GetComponent<FadingComponent>();

            string gameObjectName = gameObject.name;

            if (gameObjectName == "pf_hangman_game_tsiggelia_controller" || gameObjectName == "pf_hangman_game_tsiggelia_controller(Clone)" || gameObjectName == "pf_hangman_game_tsiggelia_controller_en")
            {
                sceneToDisplay = "_LEVEL_SELECT_TSIGGELIA";
            }

            if (gameObjectName == "pf_hangman_game_rodesia_controller" || gameObjectName == "pf_hangman_game_rodesia_controller(Clone)" || gameObjectName == "pf_hangman_game_rodesia_controller_en")
            {
                sceneToDisplay = "_LEVEL_SELECT_RODESIA";
                prefabIndex = 2;
            }

            if (achievementManagerObject != null)
            {
                //achievementManager = achievementManagerObject.GetComponent<AchievementManager>();

                //budgeBeltManager = budgeBeltManagerObject.GetComponent<achievementsManaget>();

                scenarioController = scenarioControllerObject.GetComponent<ScenarioController>();
                if (scenarioController != null)
                {
                    // on start of the game set both value to false in order for the game to be marked as new
                    scenarioController.completedSuccessfully = false;
                    scenarioController.completedUnSuccessfully = false;
                    completedSuccessfully = false;
                    completedUnSuccessfully = false;
                    scenarioController._completed = false;
                    scenarioController.resetLives();

                    // this restarts the game?
                    hangmanService.Restart();
                }
            }
        }

        private void OnEnable()
        {
            hangmanService.Continue();


            //We place this on enable because we only want to check the language
            //when the game starts, no need to be checking it every frame;
            //if (LanguageManager.CurrentLanguage == Language.Greek)
            //{
            //    letterSectionController.isGreek = true;
            //    //hangmanLevelSelect._configs = [0];

            //}
            //else
            //{
            //    letterSectionController.isGreek = false;
            //}


            //string gameObjectName = gameObject.name;
            //gameHunting2.CheckIfGameHasAlreadyCompleted(gameObjectName);
            StartCoroutine(StartGame());
        }

        IEnumerator StartGame() {

            yield return new WaitForSeconds(1.0f);

            Scene startScene = SceneManager.GetSceneByName(sceneToActivate);
            if (!startScene.isLoaded)
            {
                gameConcluded = false;
                Complete = false;
                scenarioController = scenarioControllerObject.GetComponent<ScenarioController>();
                scenarioController.completedSuccessfully = false;
                scenarioController.completedUnSuccessfully = false;
                completedSuccessfully = false;
                completedUnSuccessfully = false;
                scenarioController._completed = false;
                scenarioController.resetLives();

                CleanLevels();
            }
        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log("Current words: " + scenarioController.GetPhrase());
            if (endScreenActivated == false && gameConcluded == false) {
                if (scenarioController != null)
                {
                    completedSuccessfully = scenarioController.completedSuccessfully;
                    completedUnSuccessfully = scenarioController.completedUnSuccessfully;
                    // check if hangman game is concluded
                    ShowEndScreen(completedSuccessfully, completedUnSuccessfully);
                }
                else
                {
                    scenarioController = scenarioControllerObject.GetComponent<ScenarioController>();
                }
            }
        }

        public void ShowEndScreen(bool completedSuccessfully, bool completedUnSuccessfully)
        {
            //Debug.Log("Completed Unsuccessfully: " + completedSuccessfully);
            string gameObjectName = gameObject.name;
            Phrase phrase = scenarioController.GetPhrase();
            if (completedSuccessfully || completedUnSuccessfully)
            {
                SwitchGameView(false);
            }

            if (gameObjectName == "pf_hangman_game_tsiggelia_controller")
            {
                for (int i = 0; i < herbPhrases.Length; i++)
                {
                    if (phrase.value == herbPhrases[i])
                    {
                        if (completedSuccessfully)
                        {
                            SuccessScreen[i].SetActive(true);
                        }
                        if (completedUnSuccessfully)
                        {
                            FailureScreen[i].SetActive(true);
                            endScreenActivated = true;
                        }
                    }
                }
            }
            else
            {
                if (gameObjectName == "pf_hangman_game_tsiggelia_controller_en")
                {
                    for (int i = 0; i < testPhrases.Length; i++)
                    {
                        if (phrase.value == testPhrases[i])
                        {
                            if (completedSuccessfully)
                            {
                                Debug.Log("Unsuccessful");
                                SuccessScreen[i].SetActive(true);
                            }
                            if (completedUnSuccessfully)
                            {
                                Debug.Log("successful");

                                FailureScreen[i].SetActive(true);
                                endScreenActivated = true;
                            }
                        }
                    }
                }
            }

            if (gameObjectName == "pf_hangman_game_rodesia_controller" || gameObjectName == "pf_hangman_game_rodesia_controller(Clone)")
            {
                for (int i = 0; i < herbPhrases.Length; i++)
                {

                    if (phrase.value == herbPhrases[i])
                    {
                        if (completedSuccessfully)
                        {
                            SuccessScreen[i].SetActive(true);
                        }
                        if (completedUnSuccessfully)
                        {
                            FailureScreen[i].SetActive(true);
                            endScreenActivated = true;
                        }
                    }
                }
            }
            if (gameObjectName == "pf_hangman_game_rodesia_controller_en" || gameObjectName == "pf_hangman_game_rodesia_controller_en(Clone)")
            {
                for (int i = 0; i < testPhrases.Length; i++)
                {

                    if (phrase.value == testPhrases[i])
                    {
                        if (completedSuccessfully)
                        {
                            SuccessScreen[i].SetActive(true);
                        }
                        if (completedUnSuccessfully)
                        {
                            FailureScreen[i].SetActive(true);
                            endScreenActivated = true;
                        }
                    }
                }
            }
        }

        public void CompleteGame()
        {
            CalculateScore();
            hasWon = true;
            Complete = true;
            string gameObjectName = gameObject.name;
            QuitGame(hasWon);
            // To Do: Unload Scene more smoothly
            SceneManager.UnloadSceneAsync(sceneToActivate);



            //achievementManager.Unlock(achievementKey);
            //bagdeCaseHandler.UnlockBudge(budgeIndex);
        }

        public void ResetGame()
        {
            Complete = false;
            for (int i = 0; i < FailureScreen.Length; i++)
            {
                SuccessScreen[i].SetActive(false);
                FailureScreen[i].SetActive(false);
            }

            SwitchGameView(true);
            hangmanService.Restart();
            scenarioController.ResetCorrectGuessCount();
            scenarioController.completedSuccessfully = false;
            scenarioController.completedUnSuccessfully = false;
            scenarioController.unlockedCount = 0;
            scenarioController._completed = false;

            //scenarioController.GenerateGame();
            gameConcluded = false;
            endScreenActivated = false;
            popUp.SetActive(false);
            //
            //removeLife();
        }

        // if game has already restarted once via failure,
        // player have one less life

        public void removeLife() {
            scenarioController.setLives(scenarioController.Lives() - 1);
            Debug.Log(scenarioController.Lives());
        }

        public void SwitchGameView(bool enabled)
        {
            MainDismissButton.SetActive(enabled);
            MainPauseButton.SetActive(enabled);
            PhraseSection.SetActive(enabled);
            HintSection.SetActive(enabled);
            LetterSection.SetActive(enabled);
            ScenarioController.SetActive(enabled);
        }

        public void GameLost()
        {
            hasWon = false;

            //if (fm != null) {
            //    // fm.FadeOut();
            //}

            Complete = true;
            SceneManager.UnloadSceneAsync(sceneToActivate);

            QuitGame(hasWon);
        }

        public void ConcludeGame()
        {
            gameConcluded = true;
        }

        public void CleanLevels()
        {
            Scene startScene = SceneManager.GetSceneByName(sceneToActivate);
            if (startScene == null)
            {
                SceneManager.LoadScene(sceneToActivate, LoadSceneMode.Additive);

                //hangmanService.Restart();
                GameObject levelSelectGO = GameObject.Find("/Level Select");
                if (levelSelectGO != null)
                {
                    LevelSelect levelSelect = levelSelectGO.GetComponent<LevelSelect>();
                    if (levelSelect != null)
                    {
                        levelSelect.ClearLevels();
                        levelSelect.AddLevels();
                    }
                }
            }
            else {
                GameObject levelSelectGO = GameObject.Find("/Level Select");
                if (levelSelectGO != null)
                {
                    LevelSelect levelSelect = levelSelectGO.GetComponent<LevelSelect>();
                    if (levelSelect != null)
                    {
                        levelSelect.ClearLevels();
                        levelSelect.AddLevels();
                    }
                }
            }
        }

        public void QuitGame(bool hasWon)
        {
            StartCoroutine(QuitGameCo(hasWon));
        }

        IEnumerator QuitGameCo(bool hasWon)
        {
            //gameHunting2.FinishCurrentGame(gameHunting2.FindPrefabIndex(transform.parent.name), hasWon);


            hangmanService.Pause();
            StartCoroutine(DisableParent());

            yield return new WaitForSeconds(1f);

            Scene startScene = SceneManager.GetSceneByName(sceneToActivate);
            if (startScene.isLoaded)
            {
                AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneToActivate);
                // Wait until the scene is unloaded.
                while (!asyncUnload.isDone)
                {
                    yield return null;
                }
            }
        }

        IEnumerator DisableParent()
        {
            yield return new WaitForSeconds(1f);
            
            transform.parent.gameObject.SetActive(false);
        }

        private void CalculateScore()
        {
            int curLives = scenarioController.Lives();

            //The score is based on how many lives we got;
            
            if (curLives < 1)
            {
                score = 0;
            }
            else if (curLives < 2)
            {
                score = 1;
            }
            else if (curLives < 3)
            {
                score = 2;
            }
            else if (curLives < 4)
            {
                score = 3;
            }
            else
                score = 4;

            Debug.Log("Score: "+score);

            //We pass our socre(the current lives) into the score system;
            //scoreSystem.AddMinigameScore(score);
        }
    }
}
