using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : Attack {

    private bool waitingForTarget;

    protected float lingeringDuration;
    protected float maxDiameter;
    protected float damage;
    protected List<Element> elements;

    //TODO: Add better structure for mana costs
    //protected int manaCost= 5;
    string splashPrefabName;
    protected GameObject splashPrefab;

    /// <summary>
    /// Where is the attack aimed and will end up
    /// </summary>
    protected Vector3 targetPos;
    protected GameObject homing;

    /// <summary>
    /// Where the attack starts from. Should probably be set from attacker's position (good for if it attack moves along line) or the targets position (just the target area is affected). 
    /// </summary>
    protected Vector3 startPos;

    protected bool cooledDown;

    public bool CooledDown
    {
        get{return cooledDown;}
    }
    /// <summary>
    /// how long it takes for the spell to cool down
    /// </summary>
    //protected float coolDown=3f;
    protected float coolDown;
    protected float coolDownTimer;

    protected GameObject caster;
    protected List<GameObject> friends;

    public GameObject Caster
    {
        get{return caster;}
        //set{caster = value;}
    }
    public List<GameObject> Friends
    {
        get{return friends;}
        //set{caster = value;}
    }
    public bool Casting
    {
        get { return isAttacking;}
    }
    public bool ReadyToCast
    {
        get { return cooledDown&&!isAttacking; }
        //get { return readyToCast; }
    }
    // private bool readyToCast
    // {
    //     get { return cooledDown&&!isAttacking; }
    // }
    public string CoolDownText
    {
        get 
        {
            if (isAttacking)
            {
                return ("Casting ");    
            }
            if (!cooledDown)
            {
                float temp = (coolDown - coolDownTimer);
                string timeLeft;

                if (temp<1)
			    {
				    timeLeft = System.String.Format("{0:0.##}", (temp));
			    }
			    else
			    {
				    timeLeft = System.String.Format("{0:#.##}", (temp));
			    }
                return "Cooldown "+timeLeft;
            }
            else
            {
                return "Ready";
            }
        }   
    }

    //protected float manaCost =5f;
    // public float ManaCost
    // {
    //     get { return manaCost;}
    // }
    public override void Start()
    {
        cooledDown = true;
        // print(AttackDuration);
        base.Start();
        AttackDuration =1f;
        caster = gameObject;
        //startPos = transform.position;
    }

    public void SetSpellComponents(float spellCoolDown, string prefabName, float duration, float diameter, List<Element> elems, float dps)
    {
        coolDown = spellCoolDown;
        splashPrefabName = prefabName;
        splashPrefab = Resources.Load("splashes/" +prefabName) as GameObject;
        lingeringDuration = duration;
        maxDiameter = diameter;
        elements = elems;
        damage = dps;
        //AttackDuration = spellDuration;
    }

    public void SetCaster(GameObject c)
    {
        caster = c;
        friends = new List<GameObject>();
        friends.Add(caster);
    }
    public void SetCasterAndFriends(GameObject c, List <GameObject> allies)
    {
        caster = c;
        friends = allies;
        friends.Add(caster);
    }
    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        CoolDown();
    }
    public virtual void CoolDown()
    {
        if (!cooledDown)
        {
            //print(coolDown);
            coolDownTimer += Time.deltaTime;
			if (coolDownTimer >= coolDown)
			{
				coolDownTimer = 0;
				cooledDown = true;
			}
        }
    }

    public override void InitiateAttack(GameObject attacker, Vector3 pos)
    {
        targetPos = pos;
        InitiateAttack(attacker);
    }
    public override void InitiateAttack(GameObject attacker)
    {
        caster = attacker;
        if (caster.tag == "Enemy")
        {
            targetPos = GameManager.PlayerPos;
        }
        base.InitiateAttack(attacker);
        startPos = Caster.transform.position;
        //cooledDown = false;
    }
    public void InitiateAttack(GameObject attacker, GameObject target)
    {
        //TODO target
        homing = target;
        InitiateAttack(attacker);
    }
    

    public override void OnEndAttack()
    {
        cooledDown = false;
        startPos = transform.position;
        GameObject spellSplash = Instantiate(splashPrefab, startPos, transform.rotation);
        //print(splashPrefabName);
        FaceTargetPos(spellSplash);
        //SplashObject splashObjectScript = spellSplash.GetComponent<SplashObject>();
        //print(spellSplash.GetComponent(splashPrefabName)as SplashObject);
        SplashObject splashObjectScript = spellSplash.GetComponent(splashPrefabName) as SplashObject;
        //print((splashObjectScript!=null));
        //print(targetPos);
        //print(Caster);
        //Help.print("spleah",damage);

        // splashObjectScript.SetVariables(targetPos, Caster, lingeringDuration,maxDiameter, elements,damage);
        if (homing == null)
        {
            splashObjectScript.SetVariables(targetPos, Friends, lingeringDuration, maxDiameter, elements, damage);
        }
        else
        { 
            splashObjectScript.SetVariables(homing, Friends, lingeringDuration, maxDiameter, elements, damage);
        }
        base.OnEndAttack();
    }
    public void BaseOnEndAttack()
    { 
        base.OnEndAttack();
    }

    public void FaceTargetPos(GameObject splashy)
    {
        Vector3 tempPos = targetPos;
        tempPos.y = splashy.transform.position.y;
        splashy.transform.rotation = Quaternion.LookRotation(tempPos - splashy.transform.position);
    }
    
    /// <summary>
    /// Waits for player to click a target before allowing player to cast spell
    /// </summary>
    /// <returns></returns>
    //public IEnumerator WaitForTargetPos()
    //{
    //    waitingForTarget = true;
    //    while (waitingForTarget == true)
    //    {
    //        yield return null;
    //    }
    //}
    /*
    public virtual Vector3 GetTarget()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1500))
            {
                return hit.point;
            }
        }
        return GetTarget();
    }*/
}
