namespace Wager.Ben.LastOneOutLoses
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ZombieController : MonoBehaviour
    {
        public static ZombieController Instance;

        // Tranform to move a zombie to when clicked
        public Transform saveTarget;

        // Explosion effect
        public GameObject explosionPrefab;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.name.Contains("Mummy"))
                    {
                        if (GameController.Instance.CanSaveZombie())
                        {
                            Zombie zombie = hit.collider.GetComponent<Zombie>();

                            if(!zombie.IsDead)
                            {
                                GameController.Instance.SaveZombie();
                                zombie.MoveToTarget(saveTarget, explosionPrefab);
                            }
                        }
                    }
                }
            }
        }

        public void PickRandom()
        {
            Zombie[] zombies1 = FindObjectsOfType<Zombie>();
            List<Zombie> zombies = new List<Zombie>();
            foreach(Zombie zombie in zombies1)
            {
                if(!zombie.IsDead)
                {
                    zombies.Add(zombie);
                }
            }
            GameController.Instance.SaveZombie();
            zombies[Random.Range(0,zombies.Count)].MoveToTarget(saveTarget, explosionPrefab);

        }
    }
}