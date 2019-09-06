using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : SplashObject
{
    private ParticleSystem explosion;
    //ElementalDamage elements;
    public override void Start()
    {
        //lingeringDuration = 1.5f;
        
        //maxDiameter = 7.5f;
        // maxDiameter = 7.5f* lingeringDuration;
        // speed = 15;
        // growth = maxDiameter / lingeringDuration;
        //elements = new Dictionary<Element, float>();
        
        explosion = transform.Find("FireExplosion").gameObject.GetComponent<ParticleSystem>();
        //CreateElements();
        //elements.Add(Element.fire,Damage);
        //print(elements.Count);

    }
    // public override void SetVariables(Vector3 targetP, GameObject caster, float linger, float diamter, List<>)
    // {
    //     base.SetVariables(targetP, caster);
    //     // lingeringDuration = linger;
    //     //speed = Mathf.Pow(Vector3.Distance(targetPos, transform.position),1.5f)/ lingeringDuration;
    // }
    public override void CalculateDiameterAndDuration()
    { 
        maxDiameter = 7.5f* lingeringDuration;
        growth = maxDiameter / lingeringDuration;
        speed = (4*maxDiameter)/3;
    }

    public override void Update()
    {
        //Help.print(splashingTimer,lingeringDuration);
        // if (elements == null)
        // { 
        //     CreateElements();
        // }
        // if (elements.Elements.Count == 0)
        // { 
        //     elements.AddElement(Element.fire, Damage);
        // }
        GrowShrink();
        Move();
        //print(transform.position);
        SplashTimer();
        if (casterSet)
        {
            HurtWithinRange();
        }
    }

    public override void SplashTimer()
    {
        if (transform.position == targetPos)
        {
            base.SplashTimer();
        }
    }
    public override void GrowShrink()
    {
        if (transform.position == targetPos)
        {
            if (!explosion.isPlaying)
            {
                explosion.Play(true);
            }
            if (maxDiameter > transform.localScale.x)
            {
                transform.localScale += (new Vector3(growth, growth, growth) * Time.deltaTime);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
        //print("Elements Fireball has: " + elements.Count);
    }
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.GetComponent<TutorialCampfire>() != null)
            {
            // print("foundCampFire");
            //string temp = Help.print(casterAndFriends.ToArray());
            allWithinRange.Add(other);
            }
    }
    //if (targetPos == transform.position && !reachedTarget)
    //{
    //    reachedTarget = true;
    //}
}