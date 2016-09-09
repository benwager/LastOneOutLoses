namespace Wager.Ben.LastOneOutLoses
{
    using System.Collections;
    using UnityEngine;

    public class Slerper : MonoBehaviour
    {

        public static Slerper Instance;

        void Awake()
        {
            Instance = this;
        }

        public void MoveTo(Transform transform, Vector3 newPosition, float lerpSpeed = 6)
        {
            StartCoroutine(MoveTransformTo(transform, newPosition, lerpSpeed));
        }

        public void MoveTo(RectTransform transform, Vector3 newPosition, float lerpSpeed = 6)
        {
            StartCoroutine(MoveRectTransformTo(transform, newPosition, lerpSpeed));
        }


        private IEnumerator MoveTransformTo(Transform transform, Vector3 newPosition, float lerpSpeed)
        {
            float t = 0.0f;
            Vector3 startingPos = transform.position;
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / lerpSpeed);

                transform.position = Vector3.Slerp(startingPos, newPosition, t);
                yield return 0;
            }

            transform.position = newPosition;
        }

        private IEnumerator MoveRectTransformTo(RectTransform transform, Vector3 newPosition, float lerpSpeed)
        {
            float t = 0.0f;
            Vector3 startingPos = transform.position;
            while (t < 1.0f)
            {
                t += Time.deltaTime * (Time.timeScale / lerpSpeed);

                transform.position = Vector3.Slerp(startingPos, newPosition, t);
                yield return 0;
            }

            transform.position = newPosition;
        }
    }
}
