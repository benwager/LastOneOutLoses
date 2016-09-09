namespace Wager.Ben.LastOneOutLoses
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.AI;

    public class Zombie : MonoBehaviour
    {
        public bool IsDead = false;

        Animator anim;
        bool boolper, boolper2, boolper3;

        private Transform target;
        private GameObject explosion;
        private GameObject explosionInstance;

        void Awake()
        {
            anim = GetComponentInChildren<Animator>();

            if (UnityEngine.Random.Range(0, 2) >= 1)
            {
                OtherIdle();
            }
        }

        public void Walk()
        {
            boolper = anim.GetBool("isWalk");
            anim.SetBool("isWalk", !boolper);
            anim.SetBool("isRun", false);
            anim.SetBool("isAnother", false);
            anim.SetBool("Attack", false);
            anim.SetBool("LowKick", false);
            anim.SetBool("isDeath", false);
            anim.SetBool("isDeath2", false);
            anim.SetBool("HitStrike", false);
        }

        public void Run()
        {
            boolper2 = anim.GetBool("isRun");
            anim.SetBool("isRun", !boolper2);
            anim.SetBool("isWalk", false);
            anim.SetBool("isAnother", false);
            anim.SetBool("Attack", false);
            anim.SetBool("LowKick", false);
            anim.SetBool("isDeath", false);
            anim.SetBool("isDeath2", false);
            anim.SetBool("HitStrike", false);

        }

        public void OtherIdle()
        {
            boolper3 = anim.GetBool("isAnother");
            anim.SetBool("isAnother", !boolper3);
            anim.SetBool("isWalk", false);
            anim.SetBool("isRun", false);
            anim.SetBool("Attack", false);
            anim.SetBool("LowKick", false);
            anim.SetBool("isDeath", false);
            anim.SetBool("isDeath2", false);
            anim.SetBool("HitStrike", false);

        }
        public void Attack()
        {
            anim.SetBool("Attack", true);
        }

        public void LowKick()
        {
            anim.SetBool("LowKick", true);
        }

        public void Death()
        {
            anim.SetBool("isDeath", true);
        }
        public void Death2()
        {
            anim.SetBool("isDeath2", true);
        }
        public void Strike()
        {
            anim.SetBool("HitStrike", true);
        }

        public void Damage()
        {
            anim.SetBool("isDamage", true);
        }

        public void MoveToTarget(Transform target, GameObject explosion)
        {
            IsDead = true;
            this.target = target;
            this.explosion = explosion;
            Attack();
            StartCoroutine(DoMoveToTarget());
        }

        private IEnumerator DoMoveToTarget()
        {
            yield return new WaitForSeconds(.8f);
            GetComponent<NavMeshAgent>().SetDestination(target.position);
            if (Random.Range(0, 3) >= 1)
            {
                Run();
            }
            else
            {
                Walk();
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.name == "Boat")
            {
                OtherIdle();
                int rnd = Random.Range(0, 8);
                if (rnd < 2)
                {
                    Death();
                }
                else if (rnd < 3)
                {
                    Death2();
                }
                else
                {
                    LowKick();
                }
                StartCoroutine(WaitToExplode());
            }
        }

        private IEnumerator WaitToExplode()
        {
            yield return new WaitForSeconds(1.4f);
            explosionInstance = Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}