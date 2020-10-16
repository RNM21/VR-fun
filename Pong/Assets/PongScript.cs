using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PongScript : MonoBehaviour
{

    public GameObject ball;
    public GameObject ballResetPos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (ball.transform.position.y <= 0.0525)//ball hits floor
        {
            ball.transform.position = ballResetPos.transform.position;
            ball.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
}
