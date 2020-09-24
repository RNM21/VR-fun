using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    public int speed = 40;
    public GameObject bullet;
    public Transform barrel;

    //private bool hasHit = false;
    //public AudioSource audioSource;
    //public AudioClip AudioClip;

    public void fireBullet()
    {
        GameObject SpawnedBullet = Instantiate(bullet, barrel.position, barrel.rotation);
        SpawnedBullet.GetComponent<Rigidbody>().velocity = speed * barrel.forward;
       // audioSource.PlayOneShot(audioClip);
        Destroy(SpawnedBullet, 2);
    }

    //IEnumerator DestroyBullet()
    //{
    //    hasHit = true;
    //    yield return new WaitForSeconds(0.5f);
    //    Destroy(gameObject);
    //}

}
