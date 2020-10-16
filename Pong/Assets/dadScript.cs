using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]

public class dadScript : MonoBehaviour, IEntity
{
    public float movementSpeed = 4f;
    public Transform throwPoint;
    public GameObject ball;

    [HideInInspector]
    //public Transform ballTransform;
    NavMeshAgent agent;
    Rigidbody r;
    GameObject dad;
    float walker = 0f;
    private bool walkable;
    private Animator dadAnimator;
    public GameObject player;
    //private bool aggro = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("VR Camera");
        walkable = true;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = movementSpeed;
        r = GetComponent<Rigidbody>();
        r.useGravity = false;
        r.isKinematic = true;
        dad = GameObject.Find("Son");
        dadAnimator = dad.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //UnityEngine.Debug.Log(Vector3.Distance(agent.transform.position, player.transform.position));
        updateWalking();
        //Always look at ball or me
        if(walkable)
        {
            transform.LookAt(new Vector3(ball.transform.transform.position.x, transform.position.y, ball.transform.position.z));
        }
        else
        {
            transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        }
    }

    public void updateWalking()
    {
        
        if (ball.transform.position.y <= 0.0523 && walkable)
        {
            if(walker != 1)
            {
                walker += 0.02f;
            }
            dadAnimator.SetFloat("Walking", walker);
            agent.destination = ball.transform.position;
        }
        else if(!walkable)
        {
            walker = 1;
            dadAnimator.SetFloat("Walking", walker);
            agent.destination = player.transform.position;
        }
        else if (Vector3.Distance(agent.transform.position, player.transform.position) <= 4f)
        {
            walker = 0f;
            dadAnimator.SetFloat("Walking", walker);
            agent.destination = dad.transform.position;
        }
        else
        {
            walker = 0f;
            dadAnimator.SetFloat("Walking", walker);
            agent.destination = dad.transform.position;
        }
    }

    public void canWalk(bool w)
    {
        walkable = w;
    }


    public void ApplyDamage(float points)
    {
       
    }

}