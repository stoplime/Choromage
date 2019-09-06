using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{

    protected float maxHealth =50f;
    protected float health;
    protected int gold = 13;

    Rigidbody rb;

    //protected float defense;
    //protected float attack;
    protected float healthRegen;
    protected float healthRegenSpeed = 10;
    protected float healthDifference;
    protected float healthTimer;

    protected float baseDefense;
    protected float armorDefense;

    //protected float baseWeapon;

    protected float mana;
    protected float manaRegen;
    protected float maxMana = 0;
    protected float manaRegenSpeed = 10;
    protected float manaTimer;
    protected float manaDifference;
    bool HasArmor
    {
        get
        {
            if (gameObject.GetComponent<EquippedStats>() != null)
            {
                return gameObject.GetComponent<EquippedStats>().HasArmor;
            }
            return false;
        }
    }


    /// <summary>
    /// how many seconds it takes to cast in addition to base cast time, because they way the animator works it multiplies, so the inverse must be used, 0 will break it. sNegative means slower cas time
    /// </summary>
    //protected float castSpeed;
    /// <summary>
    /// how many seconds it takes to cast in addition to base cast time, because they way the animator works it multiplies, so the inverse must be used, 0 will break it. sNegative means slower cas time
    /// </summary>
    public float CastSpeed
    {
        get
        {
            if (gameObject.GetComponent<EquippedStats>() != null)
            { return gameObject.GetComponent<EquippedStats>().CastSpeed; }
            else { return 0; }
        }
    }
    protected float TotalMaxHealth
    {
        get
        {
            if (gameObject.GetComponent<EquippedStats>() != null)
            { return gameObject.GetComponent<EquippedStats>().MaxHealthIncrease+maxHealth; }
            else { return maxHealth; }
        }
    }
    protected float TotalMaxMana
    {
        get
        {
            if (gameObject.GetComponent<EquippedStats>() != null)
            { return gameObject.GetComponent<EquippedStats>().MaxManaIncrease+maxMana; }
            else { return maxMana; }
        }
    }

    protected float manaEfficiency;


    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public float Mana
    {
        get { return mana; }
        set { mana = value; }
    }

    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    public float TotalDefense
    {
        get { return ArmorDefense + baseDefense; }
    }
    public float ArmorDefense
    {
        get
        {
            if (gameObject.GetComponent<EquippedStats>() != null)
            {
                if (gameObject.GetComponent<EquippedStats>().HasArmor)
                {
                    if (gameObject.GetComponent<EquippedStats>().TotalResistances.Count > 0)
                    {
                        if (gameObject.GetComponent<EquippedStats>().TotalResistances.ContainsKey(Element.neutral))
                        {
                            return gameObject.GetComponent<EquippedStats>().TotalResistances[Element.neutral];
                        }
                    }
                }
            }
            return 0f;
        }
    }
    public EquippedStats EquipmentSpellStats{
        get
        {
            if (gameObject.GetComponent<EquippedStats>() != null)
            {
                return gameObject.GetComponent<EquippedStats>();
            }
            return null;
        }
    }

    protected float HealthRegen 
    {
        get
        {
            if (EquipmentSpellStats!=null)
            {
                return healthRegen + EquipmentSpellStats.HealthRegen;
            }
            return healthRegen;
        }
    }
    public float HealthRegenStat
    {
        get {
            return HealthRegen / healthRegenSpeed;
        }
    }
    protected float ManaRegen 
    {
        get
        {
            if (EquipmentSpellStats!=null)
            {
                return manaRegen + EquipmentSpellStats.ManaRegen;
            }
            return manaRegen;
        }
    }
    public float ManaRegenStat
    {
        get {
            return ManaRegen / manaRegenSpeed;
        }
    }
    /// <summary>
    /// Set health and mana to max values
    /// </summary>
    public virtual void Start()
    {
        mana = TotalMaxMana;
        health = TotalMaxHealth;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RegenHealth();
        RegenMana();
    }

    void ElementalResitanceDamage(ref float tempDamage, Element elem)
    {
        if (GetComponent<ElementalResistances>())
        {
            //print("elem resist");
            Dictionary<Element, float> elementalResistWeakness = GetComponent<ElementalResistances>().ElementalBuffsDebuffs;
            //print(elementalResistWeakness[elem]);
            if (elementalResistWeakness.ContainsKey(elem))
            {
                tempDamage *= elementalResistWeakness[elem];
            }
            //print(tempDamage);
        }
    }

    void ElementalArmor(ref float tempDamage, Element elem)
    {
        //print(HasArmor);
        if (HasArmor)
        {
            //TODO: have getcomponent use armor dictionary
            //TODO: scale for armor stats and base stats
            //TODO: see if neutral needs to be balanced out/changed with TotalDefense
                               
            Dictionary<Element, float> elementalBuffsDebuffs = GetComponent<EquippedStats>().TotalResistances;
            if (elementalBuffsDebuffs.ContainsKey(elem))
            {
                tempDamage -= elementalBuffsDebuffs[elem];
            }
        }
    }

    //currently (multiplies restistance subtracts armor/buffs) per element then subtracts total defence from the total damage. Overtime
    public bool TakeDamage(Dictionary<Element, float> elems)
    {
        //print("takdmg");
        float totalDamage = 0;
        foreach (KeyValuePair<Element, float> elem in elems)
        {
            float tempDamage = elem.Value;
            ElementalResitanceDamage(ref tempDamage, elem.Key);
            ElementalArmor(ref tempDamage, elem.Key);
            // print(tempDamage);
            totalDamage += (tempDamage* Time.deltaTime);
        }
        //TODO: check total defnse math & balancing
        totalDamage -= TotalDefense* Time.deltaTime;
        totalDamage += (Time.deltaTime * HealthRegen / healthRegenSpeed);
        //amount -= TotalDefense;
        if ((int)health -totalDamage >=1 &&totalDamage>0)        
        // if (totalDamage < (int)health&&totalDamage>0)
        {
            health -= totalDamage;
            return true;
        }
        else if (totalDamage>0)
        {
            health = 0;
            return false;
        }
        else
        {
            return true;
        }
        //}
        // else 
        // {
        //     Help.print("Attack absorbed", TotalDefense, amount);
        //     return true;
        // }
    }

    /// <summary>
    /// Send a message to this method from other scripts to lower health of characters or enemies. The bool return represents whether the being (or thing) that took damage is still alive after taking said damage. (true == alive; false == dead), flat rate
    /// </summary>
    /// <param name="amount"></param>
    /// <returns>true == alive; false == dead</returns>
    public bool TakeDamage(float amount)
    {
        //print(":(");
        // print(amount);
        //amount *= Time.deltaTime;
        //if damage does not kill (or less than health left)
        //amount -= (TotalDefense*Time.deltaTime);
        amount -= TotalDefense;
        if (amount < health&&amount>0)
        {
            health -= amount;
            return true;
        }
        else if (amount > 0)
        {
            health = 0;
            return false;
        }
        else 
        {
            Help.print("Attack absorbed", TotalDefense, amount);
            return true;
        }
    }
    
    void HealDamage(float amount)
    {
        if (amount < (TotalMaxHealth - health))
        {
            health += amount;
        }
        else
        {
            health = TotalMaxHealth;
        }
    }
    void RegainMana(float amount)
    { 
        if (amount < (TotalMaxMana - mana))
        {
            mana += amount;
        }
        else
        {
            mana = TotalMaxMana;
        }
    }


    void RegenHealth ()
    {
        if (health < TotalMaxHealth)
        {
            if (health+(Time.deltaTime*HealthRegen/healthRegenSpeed)>=TotalMaxHealth)
            {
                health =TotalMaxHealth;
            }
            else
            {
                health+=(Time.deltaTime*HealthRegen/healthRegenSpeed);
            }
            // healthTimer += Time.deltaTime;
            // if (healthTimer > healthRegenSpeed)
            // {
            //     health += healthRegen;
            //     if (health > TotalMaxHealth)
            //     {
            //         health = TotalMaxHealth;
            //     }
            //     healthTimer = 0f;
            // }
        }
        else if(health>TotalMaxHealth)
        {
            health = TotalMaxHealth;
        }
    }

    void RegenMana()
    {
        if (mana < TotalMaxMana)
        {
            if (mana+(Time.deltaTime*ManaRegen/manaRegenSpeed)>=TotalMaxMana)
            {
                mana =TotalMaxMana;
            }
            else
            {
                mana+=(Time.deltaTime*ManaRegen/manaRegenSpeed);
            }
            // manaTimer += Time.deltaTime;
            // if (manaTimer > manaRegenSpeed)
            // {
            //     mana += manaRegen;
            //     if (mana > TotalMaxMana)
            //     {
            //         mana = TotalMaxMana;
            //     }
            //     manaTimer = 0f;
            // }
        }
        else if (mana>TotalMaxMana)
        {
            mana = TotalMaxMana;
        }
    }
    // public void WindPush()
    // { 
    //     //  rigidbody.MovePosition(rigidBodyToMimic.position);
    // }
}
