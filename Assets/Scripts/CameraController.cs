namespace Wager.Ben.LastOneOutLoses
{
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        public static CameraController Instance;

        public Vector3 menuPosition;
        public Vector3 gamePosition;

        void Awake()
        {
            Instance = this;
        }

        public void GotoMenuPosition()
        {
            Slerper.Instance.MoveTo(Camera.main.transform, menuPosition);
        }

        public void GotoGamePosition()
        {
            Slerper.Instance.MoveTo(Camera.main.transform, gamePosition);
        }
    }
}