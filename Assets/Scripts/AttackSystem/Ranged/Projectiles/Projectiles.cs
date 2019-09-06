using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiles:MonoBehaviour  {

    protected float speed=1;
    /// <summary>
    /// Damage of the projectile itself
    /// </summary>
    protected float damage=4;
    /// <summary>
    /// Amount added to (projectile) damage based on weapon's/launcher's damage stat
    /// </summary>
    protected float baseDamage = 1;
    protected Rigidbody rb;

    /// <summary>
    /// The radius of the projectile
    /// </summary>
    protected float radius = 0.2f;

    /// <summary>
    /// The target location the projectile is aimed at, it will despawn after it has reached it
    /// </summary>
    protected Vector3 targetPos;
    protected Vector3 startPos;

    private bool instantiated;
    List<Collider> launcher = new List<Collider>();
    
    public virtual void Start () {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
        targetPos = GameObject.Find("Player").transform.position;
        // Double the target from the player to show that it doesnt just land on the players foot
        Vector3 dist = targetPos - startPos;
        targetPos += dist;
    }

    // Update is called once per frame
    public virtual void Update () {
        if (transform.parent == null)
        {
            rb.velocity = (transform.forward * speed);
        }

        if (Vector3.Distance(transform.position, startPos) > Vector3.Distance(targetPos, startPos))
        {
            Destroy(gameObject);
        }
    }
    public void SetSpeedAndDamage(float s, float d, Collider luncher)
    {
        launcher.Add(luncher);
        speed = s;
        baseDamage = d;
        rb = GetComponent<Rigidbody>();
        instantiated = true;
    }
    
    public void SetSpeedAndDamage(float s, float d, Collider [] luncher)
    {
        //launcher = luncher;
        foreach (Collider launch in luncher)
        {
            launcher.Add(launch);
        }
        speed = s;
        baseDamage = d;
        rb = GetComponent<Rigidbody>();
        instantiated = true;
    }

    // public virtual void OnCollisionEnter(Collision collision)
    // {
    //     Destroy(gameObject);
    // }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!CheckIfFriend(other)&& other.GetComponent<Stats>() != null)
        {
            //Help.print(launcher,"", other.name);
            other.SendMessage("TakeDamage", damage+baseDamage);
        }
    }

    public bool CheckIfFriend(Collider colliderCheck)
    {
        return launcher.Contains(colliderCheck);
    }
}
