using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityStandardAssets.ImageEffects;
using System;

public class control_script : MonoBehaviour
{

	Animator anim;
	bool boolper, boolper2, boolper3;

    public GameObject target;
    public GameObject explosion;
    private GameObject explosionInstance;

	void Awake ()
	{
		anim = GetComponentInChildren<Animator>();

        if(UnityEngine.Random.Range(0,2) >= 1)
        {
            OtherIdle();
        }
	}


	public void Walk ()
	{

		boolper = anim.GetBool("isWalk");
		anim.SetBool ("isWalk", !boolper);
		anim.SetBool ("isRun", false);
		anim.SetBool ("isAnother", false);
		anim.SetBool ("Attack", false);
		anim.SetBool ("LowKick", false);
		anim.SetBool ("isDeath", false);
		anim.SetBool ("isDeath2", false);
		anim.SetBool ("HitStrike", false);




	}

	public void Run ()
	{

		boolper2 = anim.GetBool("isRun");
		anim.SetBool ("isRun", !boolper2);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isAnother", false);
		anim.SetBool ("Attack", false);
		anim.SetBool ("LowKick", false);
		anim.SetBool ("isDeath", false);
		anim.SetBool ("isDeath2", false);
		anim.SetBool ("HitStrike", false);




	}

	public void OtherIdle ()
	{
		
		boolper3 = anim.GetBool("isAnother");
		anim.SetBool ("isAnother", !boolper3);
		anim.SetBool ("isWalk", false);
		anim.SetBool ("isRun", false);
		anim.SetBool ("Attack", false);
		anim.SetBool ("LowKick", false);
		anim.SetBool ("isDeath", false);
		anim.SetBool ("isDeath2", false);
		anim.SetBool ("HitStrike", false);




	}
	public void Attack()
	{
		anim.SetBool ("Attack", true);
	}

	public void LowKick ()
	{
		anim.SetBool ("LowKick", true);
	}

	public void Death ()
	{
		anim.SetBool ("isDeath", true);
	}
	public void Death2 ()
	{
		anim.SetBool ("isDeath2", true);
	}
	public void Strike ()
	{
		anim.SetBool ("HitStrike", true);
	}

	public void Damage ()
	{
		anim.SetBool ("isDamage", true);
	}

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    Attack();
                    StartCoroutine(MoveToBoat());

                }
            }
        }
    }

    private IEnumerator MoveToBoat()
    {
        yield return new WaitForSeconds(.8f);
        GetComponent<NavMeshAgent>().SetDestination(target.transform.position);
        if (UnityEngine.Random.Range(0, 3) >= 1)
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
            int rnd = UnityEngine.Random.Range(0, 8);
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
