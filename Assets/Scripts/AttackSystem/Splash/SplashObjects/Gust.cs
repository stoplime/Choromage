using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gust : SplashObject
{
    Rigidbody rb;
    bool colliderSet;
    public override void Start()
    {
        lingeringDuration = 7f;
        //maxDiameter = 7.5f;
        maxDiameter = lingeringDuration;
        speed = 7;
        growth = maxDiameter / lingeringDuration;
        //CreateElements();
        GetRigidBody();
    }
    public override void Move()
    {
        if (rb == null)
        {
            GetRigidBody();
        }
        // transform.position += (transform.forward *speed*Time.deltaTime);
        // rb.MovePosition(transform.position + (transform.forward * speed * Time.deltaTime));
        // rb.velocity = (transform.forward * speed * Time.deltaTime);
        rb.velocity = (transform.forward * speed/1.5f);

        if (transform.position.y > targetPos.y)
        {
            transform.position -= new Vector3(0, Time.deltaTime, 0);
        }
            //Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }
    public override void CalculateDiameterAndDuration()
    { 
        maxDiameter = lingeringDuration;
        speed = lingeringDuration;
        growth = maxDiameter / lingeringDuration;
    }
    void GetRigidBody()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void Update()
    {
        if (casterSet & !colliderSet)
        {
            colliderSet = true;
            foreach (Collider c in casterAndFriends)
            {
                GetComponent<IgnoreCollider>().Ignore(c.gameObject.name);
            }
        }
        base.Update();
        // if (targetPos == transform.position)
        // {
        //     Destroy(gameObject);
        //     print("far");
        // }
        // if (elements == null)
        // { 
        //     CreateElements();
        // }
    }
    public override void GrowShrink()
    {
        if (maxDiameter > transform.localScale.x)
        {
            transform.localScale += (new Vector3(growth, growth, 0) * Time.deltaTime);
        }
        else
        {
            //print("big");
            Destroy(gameObject);
        }
    }
    public override void HurtWithinRange()
    {
        PushWithinRange();
    }
    void PushWithinRange()
    { 
        foreach (Collider c in allWithinRange)
        {
            if (c != null)
            {
                if (c.GetComponent<Rigidbody>()!=null)
                {
                    if (rb == null)
                    {
                        GetRigidBody();
                    }
                    // c.GetComponent<Rigidbody>().MovePosition(rb.position);
                    print(rb.velocity);
                    print(c.GetComponent<Rigidbody>() == null);
                    c.GetComponent<Rigidbody>().velocity = rb.velocity*5f;
                    //print("hes elem");
                    //print(gameObject.GetComponent<ElementalDamage>().Elements.Count);
                    //print(gameObject.GetComponent<ElementalDamage>().ToString());
                    // c.SendMessage("TakeDamage", gameObject.GetComponent<ElementalDamage>().Elements);
                }
                // else
                // {
                //     //print("no elem");
                //     c.SendMessage("TakeDamage", Damage * Time.deltaTime);
                // }
            }
        }
    }
}