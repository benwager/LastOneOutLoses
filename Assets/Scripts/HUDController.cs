namespace Wager.Ben.LastOneOutLoses
{
    using UnityEngine;
    using UnityEngine.UI;

    public class HUDController : MonoBehaviour
    {
        public static HUDController Instance;

        public GameObject hudCanvas;

        public Button doneButton;

        public Button okButton;

        public Text playerText;

        public Text messageText;

        public void Start()
        {
            Instance = this;

            // done button simply ends current player's turn
            doneButton.onClick.AddListener(DoneButtonClick);

            // ok button starts player's turn
            okButton.onClick.AddListener(OKButtonClick);

            HideHUD();
        }

        public void ShowHUD()
        {
            hudCanvas.SetActive(true);
        }

        public void HideHUD()
        {
            hudCanvas.SetActive(false);
        }

        public void ShowDoneButton()
        {
            doneButton.gameObject.SetActive(true);
        }

        public void HideDoneButton()
        {
            doneButton.gameObject.SetActive(false);
        }

        public void ShowOKButton()
        {
            okButton.gameObject.SetActive(true);
        }

        public void HideOKButton()
        {
            okButton.gameObject.SetActive(false);
        }

        public void DisplayMessage(string msg)
        {
            messageText.text = msg;
        }

        public void UpdatePlayerText(string txt)
        {
            playerText.text = txt;
        }

        private void DoneButtonClick()
        {
            GameController.Instance.EndTurn();
        }

        private void OKButtonClick()
        {
            GameController.Instance.StartTurn();
        }
    }
}