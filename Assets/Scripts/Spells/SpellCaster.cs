using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will check the spell cost and compare it to the current amount of mana. TODO: Add algorithm to calculate mana discount for spell based on environtment
/// </summary>
public class SpellCaster : MonoBehaviour {

    /// <summary>
    /// Multiplitied to regular mana costs. Based off of surrounding environment.
    /// TODO: implent a way to calculate this.
    /// </summary>
    Dictionary<Element, float> ElementalDiscount
    {
        get { return castersStats.EquipmentSpellStats.ManaDiscounts; }
    }

    //float localElementalDiscount=1f;
    Dictionary<Element, float> localElementalDiscount =new Dictionary<Element, float>();
    float timer;
    public float manaRegen;
    public float manaRegenSpeed = 8;
    float manaDifference;
    /// <summary>
    /// Spell caster's current mana
    /// </summary>
    float castersMana;
    //public float maxMana = 50;
	/// <summary>
	/// How much mana a spell costs including environmental boosts.
	/// </summary>
	float manaCost;
    //bool currentlyCasting;
    Stats castersStats;
    Animator anime;

    // Use this for initialization
    void Start () {
        castersStats = GetComponent<Stats>();
        localElementalDiscount.Add(Element.neutral, 1);
        anime = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        //RegenMana();
	}

	/// <summary>
	/// Checks to see if enough mana to cast spell. Returns true if spell can be cast. Returns false if it can't.
	/// </summary>
	/// <param name="spell"></param>
	/// <returns></returns>
    public bool CheckMana(Spell spell)
    {
		manaCost = CalculteManaCost(spell);
		
		//if (castersStats.Mana >= manaCost && spell.SpellScript.CooledDown)
		if (castersStats.Mana >= manaCost)
		{
			if (Time.timeScale!=0)
			{
				castersStats.Mana -= manaCost;
				//print(castersMana);
				return true;
			}
		}
		//print(castersMana);
		return false;
    }

    float CalculteManaCost(Spell spell)
    {
        float tempManaCost =spell.ManaCost;
        //implement casters discount
        if (ElementalDiscount != null)
        {
			
            if (ElementalDiscount.ContainsKey(spell.SpellElement) && localElementalDiscount.ContainsKey(spell.SpellElement))
            {
                tempManaCost *= (ElementalDiscount[spell.SpellElement] + localElementalDiscount[spell.SpellElement]);
            }
            else if (ElementalDiscount.ContainsKey(spell.SpellElement))
            {
                tempManaCost *= ElementalDiscount[spell.SpellElement];
            }
            else if (localElementalDiscount.ContainsKey(spell.SpellElement))
            {
                tempManaCost *= localElementalDiscount[spell.SpellElement];
            }
        }
		else if (localElementalDiscount.ContainsKey(spell.SpellElement))
		{
			tempManaCost *= localElementalDiscount[spell.SpellElement];
		}
        return tempManaCost;
    }


    /// <summary>
    /// Checks to see if certain spell can be cast given its cooldown and if the caster is busy casting a different spell. True means spell can be cast false means it can't.
    /// </summary>
    /// <param name="hotSpells"></param>
    /// <param name="spell"></param>
    /// <returns></returns>
    public bool CheckCastable(List<Splash> hotSpells, Splash spell)
	{
		if (spell.CoolDownText=="Ready")	//spell not cooling down or being cast
		{
			
			foreach(Splash spelly in hotSpells)	//checks to see if caster is already casting a spell
			{
				if(spelly !=null)
				{
					//if already casting a spell return false
					if (spelly.Casting)
					{
						//if caster is busy casting any spell is being cast then caster can't cast spell
						return false;
					}
				}
			}
			
			//if false hasn't returned for any of the spells and spell isn't cooling down, then return true
			return true;
		}
		//if spell is cooling down or is being cast, return false
		return false;
	}

	/// <summary>
	/// Checks to see if certain spell can be cast given its cooldown. True means spell can be cast false means it can't.
	/// </summary>
	/// <param name="spell"></param>
	/// <returns></returns>
	public bool CheckCastable(Splash spell)
	{
		if (spell.CoolDownText=="Ready")	//spell not cooling down or being cast
		{
			return true;
		}
		//if spell is cooling down or is being cast, return false
		return false;
	}
	public bool CheckCastable(Spell spell)
	{
		if (spell.ReadyToCast)	//spell not cooling down or being cast
		{
			return true;
		}
		//if spell is cooling down or is being cast, return false
		return false;
	}

    // public void CastSpell(Spell spell)
    // { 
	// 	spell.SpellScript.InitiateAttack(GameManager.player);
	// 	print(spell.CastTime);
    //     //print(spell.CastTime * castersStats.CastSpeed);
    //     anime.SetFloat("castTime", spell.CastTime);
    //     anime.SetTrigger("Attack");
	// }
	// public void CastSpell(Spell spell, RaycastHit target)
    // { 
	// 	spell.SpellScript.InitiateAttack(GameManager.player, target.point);
	// 	print(spell.CastTime);
    //     // if (castersStats.CastSpeed + spell.CastTime >= .1f)
    //     // { 
	// 		//anime.SetFloat("castTime", castersStats.CastSpeed+spell.CastTime);
	// 		anime.SetFloat("castTime", castersStats.CastSpeed);
	// 	// }
    //     // else
    //     // { 
	// 		// anime.SetFloat("castTime", .1f);
	// 	// }
    //     anime.SetTrigger("Attack");
	// } 
    // public float LocalManaCost(float spellMana, Element e)
    // {
    // }		
}
