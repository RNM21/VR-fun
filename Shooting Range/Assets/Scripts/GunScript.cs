using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    [RequireComponent(typeof(AudioSource))]

    public class GunScript : MonoBehaviour
    {
        public bool singleFire = false;
        public int speed = 40;
        public float fireRate = 0.1f;
        public GameObject bulletPrefab;
        public Transform firePoint;
        public int bulletsPerMagazine = 30;
        public float timeToReload = 1.5f;
        public float weaponDamage = 15; //How much damage should this weapon deal
        public AudioClip fireAudio;
        public AudioClip reloadAudio;

        private bool stream = false;
        float nextFireTime = 0;
        bool canFire = true;
        int bulletsPerMagazineDefault = 0;
        AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            bulletsPerMagazineDefault = bulletsPerMagazine;
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            //Make sound 3D
            audioSource.spatialBlend = 1f;
        }

        // Update is called once per frame
        void Update()
        {
            if (stream)
            {
                Fire();
            }
        }

        public void Fire()
        {
            if (canFire)
            {
                if (Time.time > nextFireTime)
                {
                    nextFireTime = Time.time + fireRate;

                    if (bulletsPerMagazine > 0)
                    {
                        GameObject bulletObject = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                        //bulletObject.GetComponent<Rigidbody>().velocity = speed * firePoint.forward;
                        BulletScript bullet = bulletObject.GetComponent<BulletScript>();

                        //Set bullet damage according to weapon damage value
                        bullet.SetDamage(weaponDamage);

                        bulletsPerMagazine--;
                        audioSource.clip = fireAudio;
                        audioSource.Play();
                    }
                    else
                    {
                        StartCoroutine(Reload());
                    }
                }
            }
            stream = !singleFire;
        }
        
        public void stopFire()
        {
            stream = false;
        }
        

        IEnumerator Reload()
        {
            canFire = false;

            audioSource.clip = reloadAudio;
            audioSource.Play();

            yield return new WaitForSeconds(timeToReload);

            bulletsPerMagazine = bulletsPerMagazineDefault;

            canFire = true;
        }

        //Called from SC_WeaponManager
        public void ActivateWeapon(bool activate)
        {
            StopAllCoroutines();
            canFire = true;
            gameObject.SetActive(activate);
        }
    }

