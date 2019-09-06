using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// controls the gameobjects that splash instantiated from prefabs
/// </summary>
public class SplashObject : MonoBehaviour {

    /// <summary>
    /// How long it lingers. Different from timer and AttackDurtion which will represent the length of the attack animation
    /// </summary>
    protected float lingeringDuration;// =5;

    /// <summary>
    /// represets how much it grows or shrinks over time. Higher numbers = faster. for (duration) {diameter += growth}. 0 means constant size, + means grows, - means shrinks.
    /// </summary>
    protected float growth;

    /// <summary>
    /// Max diameter spell will grow to.false Will stay at the same size if it reaches max before lingeringDuration runs out
    /// </summary>
    protected float maxDiameter;//= 25;

    /// <summary>
    /// Where is the attack aimed and will end up
    /// </summary>
    protected Vector3 targetPos;
    protected GameObject homingTarget;
    bool isHoming;

    /// <summary>
    /// Where the attack starts from. Should probably be set from attacker's position (good for if it attack moves along line) or the targets position (just the target area is affected). 
    /// </summary>
    protected Vector3 startPos;

/// <summary>
/// How quickly attack moves towards target
/// </summary>
    protected float speed = 1;

    /// <summary>
    /// Damage taken per second (will call lose health in Update function foreach object touching)
    /// </summary>
    protected float Damage=10;
    protected ElementalDamage elements;

    /// <summary>
    /// A list of all objects touching attack so they can continually take damage if needbe. Will call lose health in Update function foreach object touching. Each is added in OnTriggerEnter and each is removed in OnTriggerExit
    /// </summary>
    protected List<Collider> allWithinRange = new List<Collider>();

    protected List <Collider> casterAndFriends = new List<Collider>();
    private float splashingTimer=0;

    protected bool casterSet;

    /// <summary>
    /// How much time is left before splash despawns. Has a getter that returns lingeringDuration - splashingTimer. In other words: amount of time the spell lasts - how long it has lasted so far.
    /// </summary>
    public float LingeringTimeRemaining
    {
        get { return lingeringDuration - splashingTimer; }
    }

    // Use this for initialization
    public virtual void Start()
    {
        growth = maxDiameter/lingeringDuration;
        SetElements();
    }

    public virtual void SetElements()
    { 
        //elements.Add(Element.neutral ,Damage);

    }

    public virtual void SetVariables(Vector3 targetP, GameObject spellCaster)
	{
        targetPos = targetP;
        foreach(Collider casters in spellCaster.GetComponentsInChildren<Collider>())
        {
            casterAndFriends.Add(casters);
        }
        casterSet = true;
    }
    
    /// <summary>
    /// Assigns where the splash is going in a Vector3 (targetPos) and the GameObject of who cast it (caster) so it can disable friendly fire/ enemy heal as well as a float for lingeringDuration (for weaker/stronger variations of spell).
    /// </summary>
    /// <param name="targetP"></param>
    /// <param name="spellCaster"></param>
    /// <param name="maxDiam"></param>
    public virtual void SetVariables(Vector3 targetP, GameObject spellCaster, float lingerTime, float diameter, List<Element> elems, float dps)
	{
        Damage = dps;
        targetPos = targetP;
        casterAndFriends.Add(spellCaster.gameObject.GetComponent<Collider>());
        foreach(Collider casters in spellCaster.GetComponentsInChildren<Collider>())
        {
            casterAndFriends.Add(casters);
        }
        lingeringDuration = lingerTime;
        maxDiameter = diameter;
        //growth = maxDiameter/lingeringDuration;
        casterSet = true;
        CalculateDiameterAndDuration();
        CreateElements(elems);
    }

    /// <summary>
    /// Assigns where the splash is going in a Vector3 (targetPos) and a List of GameObjects of who cast it and their allies (casterAndFriends) so it can disable friendly fire/ enemy heal as well as a float for lingeringDuration (for weaker/stronger variations of spell).
    /// </summary>
    /// <param name="targetP"></param>
    /// <param name="spellCasterAndFriends"></param>
    public virtual void SetVariables(Vector3 targetP, List<GameObject>  spellCasterAndFriends, float lingerTime, float diameter, List<Element> elems, float dps)
	{
        Damage = dps;
        targetPos = targetP;
        foreach(GameObject friend in spellCasterAndFriends)
        {
            foreach (Collider c in friend.GetComponentsInChildren<Collider>())
            { 
                casterAndFriends.Add(c); 
            }
        }
        lingeringDuration = lingerTime;
        maxDiameter = diameter;
        casterSet = true;
        CalculateDiameterAndDuration();
        CreateElements(elems);
    }

    public virtual void SetVariables(GameObject targetP, List<GameObject>  spellCasterAndFriends, float lingerTime, float diameter, List<Element> elems, float dps)
	{
        Damage = dps;
        homingTarget = targetP;
        isHoming = true;
        foreach(GameObject friend in spellCasterAndFriends)
        {
            foreach (Collider c in friend.GetComponentsInChildren<Collider>())
            { 
                casterAndFriends.Add(c); 
            }
        }
        lingeringDuration = lingerTime;
        maxDiameter = diameter;
        casterSet = true;
        CalculateDiameterAndDuration();
        CreateElements(elems);
    }
    public virtual void CalculateDiameterAndDuration()
    {
        growth = maxDiameter/lingeringDuration;
    }
    // Update is called once per frame
    public void CreateElements(List<Element> elems)
    { 
        elements = gameObject.GetComponent<ElementalDamage>();
        if (elements != null)
        {
            foreach (Element elem in elems)
            {
                //Help.print(elem, Damage);
                elements.AddElement(elem, Damage);
            }
        }
    }
    public virtual void Update () {
        //Help.print(splashingTimer,lingeringDuration);
        
        GrowShrink();
        Move();
        //print(transform.position);
        SplashTimer();
        if (casterSet)
        {
            HurtWithinRange();
        }
    }

    /// <summary>
    /// Called in update, it calculates how long the splashobject has been araound and when it should despawn. I separated it from update to make it easier to override certain parts. e.g. I you wanted it to stick aroud x amount of seconds after it reaches its destination.
    /// </summary>
    public virtual void SplashTimer()
    {
        splashingTimer += Time.deltaTime;
        if (splashingTimer >= lingeringDuration)
        {
            //print("derstroy");
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called in update, this controls how the splash grow or shrinks over time. I separated it from update to make it easier to override certain parts. e.g. If you still wanted he splash timem and the movement but wanted it to only grow/shrink after its done moving.
    /// </summary>
    public virtual void GrowShrink()
    {
        if (maxDiameter > transform.localScale.x)
        {
            transform.localScale += (new Vector3(growth, growth, growth) * Time.deltaTime);
        }
    }
    /// <summary>
    /// Called in update, this controls how the splash move. I separated it from update to make it easier to override certain parts. e.g. If you still wanted to make it home in on moving targets but didn't want to charge the timer or how it grows/shrinks.
    /// </summary>
    public virtual void Move()
    //TODO: fix homing after creature dies
    {
        if (!isHoming)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
        }
        else if (homingTarget != null&&isHoming)
        {
            transform.position = Vector3.MoveTowards(transform.position, homingTarget.transform.position, speed * Time.deltaTime);
        }
        else 
        { 
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Add colliders of anything that can be damaged as long as they are not friendly
    /// </summary>
    /// <param name="other"></param>
    public virtual void OnTriggerEnter(Collider other)
    {
        if (casterSet)
        {
            //if other collider has a health stat (the HealthScript) meaning they can take damage
            if (other.GetComponent<Stats>() != null)
            {
                if (!CheckIfFriend(other))
                {
                    //string temp = Help.print(casterAndFriends.ToArray());
                    allWithinRange.Add(other);
                }
            }
        }
    }
    public virtual void OnTriggerExit(Collider other)
    {
        allWithinRange.Remove(other);
    }
    /// <summary>
    /// returns (true: if caster's friend or caster; false: if neither caster nor caster's friend. To check friendly fire
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CheckIfFriend(Collider colliderCheck)
    {
        //print(casterAndFriends.Count);
        return casterAndFriends.Contains(colliderCheck);
    }
    /// <summary>
    /// hurts everything touching splash object. Is called in update so damage is multipleied by deltatTime
    /// </summary>
    public virtual void HurtWithinRange()
    {
        foreach (Collider c in allWithinRange)
        {
            if (c != null)
            {
                if (gameObject.GetComponent<ElementalDamage>()!=null)
                {
                    //print("hes elem");
                    //print(gameObject.GetComponent<ElementalDamage>().Elements.Count);
                    //print(gameObject.GetComponent<ElementalDamage>().ToString());
                    c.SendMessage("TakeDamage", gameObject.GetComponent<ElementalDamage>().Elements);
                }
                else
                {
                    //print("no elem");
                    c.SendMessage("TakeDamage", Damage * Time.deltaTime);
                }
            }
            // else
            // {
            //     allWithinRange.Remove(c);
            // }
        }
    }

    
}
