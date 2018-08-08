using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace someNamespace
{
    public class Packages : MonoBehaviour
    {
        public GameObject dogAnims;

        public GameObject goodPackagePrefab;
        public GameObject badPackagePrefab;

        public AudioSource goodPackageSound;
        public AudioSource badPackageSound;

        public AudioSource dropPackageSound;

        public GameObject hero;

        public bool alwaysSpawnGoodPackages;

        //speed up
        public float fastEffectTime; //in seconds
        public float fasterMultipier;

        //slow down
        public float slowEffectTime; //in seconds
        public float slowerMultiplier;

        //public float fastAnimTime;
        //public float slowAnimTime;

        //-------------------------CoRoutine Callers-------------------------

        public void decSpeed()
        {
            StartCoroutine(decreaseSpeed());
        }

        public void incSpeed()
        {
            StartCoroutine(increaseSpeed());
        }

        //-------------------------CoRoutines-------------------------

        IEnumerator decreaseSpeed()
        {
            if(hero.GetComponent<Move>().inCoRoutine == false)
            {
                hero.GetComponent<Move>().inCoRoutine = true;

                dogAnims.GetComponent<Animator>().SetBool("gotBad", true);

                badPackageSound.Play();

                float defaultSpeed = gameObject.GetComponent<Manager>().speed;
                hero.GetComponent<Move>().speed = (defaultSpeed * slowerMultiplier);
                yield return new WaitForSeconds(slowEffectTime);
                hero.GetComponent<Move>().speed = defaultSpeed;

                dogAnims.GetComponent<Animator>().SetBool("gotBad", false);

                hero.GetComponent<Move>().inCoRoutine = false;
            }
        }

        IEnumerator increaseSpeed()
        {
            if (hero.GetComponent<Move>().inCoRoutine == false)
            {
                hero.GetComponent<Move>().inCoRoutine = true;

                dogAnims.GetComponent<Animator>().SetBool("gotGood", true);

                goodPackageSound.Play();

                float defaultSpeed = gameObject.GetComponent<Manager>().speed;
                hero.GetComponent<Move>().speed = (defaultSpeed * fasterMultipier);
                yield return new WaitForSeconds(fastEffectTime);
                hero.GetComponent<Move>().speed = defaultSpeed;

                dogAnims.GetComponent<Animator>().SetBool("gotGood", false);

                hero.GetComponent<Move>().inCoRoutine = false;
            }
        }

        //-------------------------TODO... remove TEMPORARY AUTO SPAWNER-------------------------

        /*
        public float maxTime;
        public float minTime;

        private float time;
        private float spawnTime;
        */

        // Use this for initialization
        void Start()
        {
            fasterMultipier = 1.5f;
            fastEffectTime = .5f; //in seconds

            slowerMultiplier = .5f;
            slowEffectTime = .5f;

            alwaysSpawnGoodPackages = true;

            //maxTime = 5;
            //minTime = 2;

            //calcNewSpawnTime();
        }

        // Update is called once per frame
        void Update()
        {
            //auto spawning is shut off
            /*
            time += Time.deltaTime;

            //Check if its the right time to spawn the object
            if (time >= spawnTime)
            {
                SpawnObject();
                calcNewSpawnTime();
            }
            */
        }

        //called by cones when a collision is detected
        public void SpawnObject()
        {
            //read in randomizer
            packageType p = gameObject.GetComponent<Randomizer>().dropPackage();
            if (p != packageType.none)
            {
                Vector3 behindVan = gameObject.GetComponent<Randomizer>().van.transform.position; //tail tip of van
                behindVan.x += (gameObject.GetComponent<Randomizer>().van.GetComponent<SpriteRenderer>().bounds.extents.x); //shift the package slightly into the van
                float packageExtentY;

                //spawn the package on truck
                GameObject newPack;

                if (p == packageType.good || alwaysSpawnGoodPackages)
                {
                    newPack = Instantiate(goodPackagePrefab, Vector3.zero, Quaternion.identity);
                    newPack.GetComponent<Package>().pT = packageType.good;
                }
                else
                {
                    newPack = Instantiate(badPackagePrefab, Vector3.zero, Quaternion.identity);
                    newPack.GetComponent<Package>().pT = packageType.bad;
                }

                packageExtentY = newPack.GetComponent<SpriteRenderer>().bounds.extents.y;
                behindVan.y += packageExtentY;
                newPack.transform.position = behindVan;

                dropPackageSound.Play();
                newPack.GetComponent<Rigidbody2D>().angularVelocity = Random.Range(25, 100);
            }
        }

        /*
        void calcNewSpawnTime()
        {
            spawnTime = Random.Range(minTime, maxTime);
            time = 0;
        }
        */
    }
}