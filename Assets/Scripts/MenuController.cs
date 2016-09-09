namespace Wager.Ben.LastOneOutLoses
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    public class MenuController : MonoBehaviour
    {
        public static MenuController Instance;

        public Button singlePlayerModeButton;
        public Button multiplayerModeButton;
        public Button gotitButton;

        public GameObject mainMenuCanvas;

        public GameObject modePanel;
        public GameObject instructionsPanel;

        public GameObject gameOverPanel;
        public Text gameOverWinnerLabel;
        public Button gameOverOKButton;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            singlePlayerModeButton.onClick.AddListener(SinglePlayerClick);
            multiplayerModeButton.onClick.AddListener(MultiPlayerClick);
            gotitButton.onClick.AddListener(GotoGamePlayView);
            gameOverOKButton.onClick.AddListener(GameOverOKClick);
            GotoMainMenu();
        }

        public void GotoMainMenu()
        {
            mainMenuCanvas.SetActive(true);
            modePanel.SetActive(true);
            instructionsPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            CameraController.Instance.GotoMenuPosition();
        }

        private void SinglePlayerClick()
        {
            GameController.Instance.NewGame(GameController.Mode.SINGLE_PLAYER);
            GotoInstructionsView();
        }

        private void MultiPlayerClick()
        {
            GameController.Instance.NewGame(GameController.Mode.MULTI_PLAYER);
            GotoInstructionsView();
        }

        private void GotoInstructionsView()
        {
            modePanel.SetActive(false);
            instructionsPanel.SetActive(true);
        }

        private void GotoGamePlayView()
        {
            CameraController.Instance.GotoGamePosition();
            mainMenuCanvas.SetActive(false);

            StartCoroutine(WaitToDisplayHUD());
        }

        public void GotoGameOverView(string winner)
        {

            HUDController.Instance.HideHUD();

            mainMenuCanvas.SetActive(true);

            modePanel.SetActive(false);
            instructionsPanel.SetActive(false);
            gameOverPanel.SetActive(true);
            gameOverWinnerLabel.text = winner;
        }

        private void GameOverOKClick()
        {
            GotoMainMenu();
        }

        private IEnumerator WaitToDisplayHUD()
        {
            yield return new WaitForSeconds(6);
            HUDController.Instance.ShowHUD();
            GameController.Instance.StartGame();
        }
    }
}