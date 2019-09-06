using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Anne McCranie
class Ammo : MonoBehaviour
{
    public float ammoMass;
    public float ammoAcceleration;
    public float initialAmmoForce;
    public float ammoSpeed;
    public float time;
    float maxRange = 200;
    bool ammoLaunched;
    bool ammoCollided;
    private Rigidbody rb;
    private float accelerationDueToGravity;
    private float speedDueToGravity;
    private Vector3 gravity;
    public GameObject projectile;
    GameObject player;
    public float timeOfImpact;
    float despawnTime;
    bool slowedDown;
    static bool freindly;
    private Color[] rainbow = { Color.magenta, Color.red, new Color(1, 0.46f, 0.008f, 1), Color.yellow, Color.green, Color.cyan, Color.blue, new Color(0.76f, 0.666666f, 0.894118f, 1) };
    float drag;
    bool aiming;
    GameObject shootyThing;
    SphereCollider sc;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("HitBox");
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (GameObject g in pickups)
        {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), g.GetComponent<Collider>());
        }
        accelerationDueToGravity = 9.81f;
        ammoSpeed = 0;
        time = 0;
        drag = 1;
        shootyThing = GameObject.FindGameObjectWithTag("gun");
    }
    void FixedUpdate()
    {
        if (ammoLaunched == true)
        {
            time += (Time.deltaTime);
            drag -= .0000001f;
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        Collider other = collision.collider;
        if (GetComponents<Collider>().Length == 1)
        {
            sc = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
            sc.isTrigger = true;
            if (this.gameObject.name.Split('(')[0] == "Marble")
            {
                sc.radius = 30;
            }
            else
            {
                sc.radius = 3;
            }
        }
        if (ammoLaunched)
        {
            if (freindly == true && other.tag != "HitBox")
            {
                if (projectile.name == "Paint")
                {
                    if (other.tag != "Ground")
                    {
                        other.GetComponent<Renderer>().material.color = projectile.GetComponent<Renderer>().material.color;
                    }
                    if (other.tag != "Player" && other.tag != "Main Camera")
                    {
                        timeOfImpact = time;
                        ammoCollided = true;
                    }
                }
                else if (other.tag == "Ground")
                {
                    if (projectile.name == "Book")
                    {
                        Destroy(projectile.gameObject);
                    }
                    else
                    {
                        initialAmmoForce = initialAmmoForce / 4;
                        ammoCollided = true;
                        timeOfImpact = time;
                        slowedDown = true;
                    }
                }
                else if (ammoCollided == false)
                {
                    if (other.tag == "Ground")
                    {
                        if (projectile.tag == "Rock")
                        {
                            Destroy(projectile.gameObject);
                        }
                    }
                    else
                    {
                        timeOfImpact = time;
                        ammoCollided = true;
                    }
                }
            }
            else if (freindly == false && ammoCollided == false)
            {
                timeOfImpact = time;
                ammoCollided = true;
            }
        }
    }
    void Update()
    {
        if (!aiming)
        {
            if (ammoLaunched)
            {
                if (slowedDown == true)
                {
                    despawnTime = 90f;
                }
                ammoAcceleration = initialAmmoForce / ammoMass;
                if (time >= (timeOfImpact + despawnTime) && ammoCollided == true)
                {
                    Destroy(projectile.gameObject);
                }
                if (ammoLaunched == true && ammoCollided == false)
                {
                    rb.AddForce(projectile.transform.forward * 4 * ammoSpeed * drag);
                    rb.AddForce(-gravity * 5);
                }
                else if (ammoLaunched == true)
                {
                    rb.AddForce(transform.forward * ammoSpeed * drag);
                    rb.AddForce(-gravity * 5);
                }
                ammoSpeed = ammoAcceleration;
                speedDueToGravity = accelerationDueToGravity * time;
                gravity = new Vector3(0.0f, speedDueToGravity, 0.0f);
                if (projectile.transform.position.x > maxRange || projectile.transform.position.x < -maxRange || projectile.transform.position.z > maxRange || projectile.transform.position.z < -maxRange || projectile.transform.position.y < -5)
                {
                    Destroy(projectile.gameObject);
                }
            }
        }
    }
    public void Launch(GameObject p, float f, float despawningTime)
    {
        ammoLaunched = true;
        projectile = p;
        despawnTime = despawningTime;
        if (projectile.tag=="Pickup")
        {
            freindly = true;
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GameObject.FindGameObjectWithTag("HitBox").GetComponent<Collider>());
            Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Player").GetComponent<Collider>());
        }
        else
        {
            freindly = false;
            if (projectile.name== "Venom")
            {
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GameObject.FindGameObjectWithTag("Serpent").GetComponent<Collider>());
            }
        }
        initialAmmoForce = f;
        rb = projectile.GetComponent<Rigidbody>();

        ammoMass = rb.mass;
    }
    void AimAmmo(string a, GameObject g)
    {
        aiming = true;
    }
}