using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedStats : MonoBehaviour
{

    // Use this for initialization
    /// <summary>
    /// key is element, value is multiplied by damage type to determine actual damage.virtual 1 is neutral, 0- complete resistance/negates it, 2 weak/doubles it.
    /// </summary>
    /// <typeparam name="Element"></typeparam>
    /// <typeparam name="float"></typeparam>
    /// <returns></returns>
    private Dictionary<Element, float> totalResistances = new Dictionary<Element, float>();

    private Dictionary<Item.EquipmentTag, ExtraItemData> equipmentsResistances = new Dictionary<Item.EquipmentTag, ExtraItemData>();

    //private Dictionary<Element, float> castTimes = new Dictionary<Element, float>();
    private float castSpeed;
    private Dictionary<Element, float> manaDiscounts = new Dictionary<Element, float>();
    private int maxHealthIncrease;
    private int maxManaIncrease;
    private float healthRegen;
    private float manaRegen;
    public Dictionary<Element, float> TotalResistances
    {
        get { return totalResistances; }
    }
    // public Dictionary <Element, float> CastTimess
    // {
    //     get { return castTimes; }
    // }
    public float CastSpeed
    {
        get{ 
            if (gameObject==GameManager.player && GameManager.FastCasting)
            {
                return .1f;
            }
            return castSpeed;
            }
    }
    public Dictionary <Element, float> ManaDiscounts
    {
        get { return manaDiscounts; }
    }
    public bool HasArmor
    {
        get
        {
            if (totalResistances.Count==0)
            {
                return false;
            }
            return true;
        }
    }
    public int MaxHealthIncrease
    {
        get{return maxHealthIncrease;}
    }
    public int MaxManaIncrease
    {
        get{return maxManaIncrease;}
    }
    public float HealthRegen
    {
        get { return healthRegen; }
    }

    public float ManaRegen 
    { 
        get => manaRegen; 
    }
    public bool HasStaff
    {
        get { return equipmentsResistances.ContainsKey(Item.EquipmentTag.Staff); }
    }

    void Start () {
        // print(gameObject.name);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Equip(Item.EquipmentTag equipmentType, ExtraItemData eid)
    {
        // print("equipped");
        equipmentsResistances.Add(equipmentType, eid);
        UpdateTotals(eid, true);
    }

	public void Unequip(Item.EquipmentTag equipmentType)
    {
        // print("unequipped");
        UpdateTotals(equipmentsResistances[equipmentType], false);
        equipmentsResistances.Remove(equipmentType);
    }

    // void UpdateTotals()
    // {
    //     foreach (KeyValuePair <Item.EquipmentTag, ExtraItemData> equipment in equipmentsResistances)
    //     {
    //         if (equipment.Value != null)
    //         {
    //             if (equipment.Value.Elements != null)
    //             {
    //                 if (equipment.Value.Elements.Count > 0)
    //                 {
    //                     foreach (KeyValuePair<Element, float> equipElem in equipment.Value.Elements)
    //                     {
    //                         if (equipment.Value.EquipmentType == EquipmentType.Armor)
    //                         {
    //                             if (!totalResistances.ContainsKey(equipElem.Key))
    //                             {
    //                                 totalResistances.Add(equipElem.Key, equipElem.Value);
    //                             }
    //                             else
    //                             {
    //                                 totalResistances[equipElem.Key] += equipElem.Value;
    //                             }
    //                         }
    //                         else 
    //                         { 
    //                             //manacost
                                
    //                         }
    //                     }
    //                 }
    //             }
    //         }
    //         //if ()
    //     }
    // }

	void UpdateTotals(ExtraItemData eid, bool added)
    {
        if (eid != null)
        {
            if (added)
            {
                // Help.print("equipped",eid.MaxHealthIncrease);
                castSpeed += eid.CastSpeed;
                maxHealthIncrease += eid.MaxHealthIncrease;
                maxManaIncrease += eid.MaxManaIncrease;
                healthRegen += eid.HealthRegenIncrease;
                manaRegen += eid.ManaRegenIncrease;
            }
            else 
            { 
                castSpeed -= eid.CastSpeed;
                maxHealthIncrease -= eid.MaxHealthIncrease;
                maxManaIncrease -= eid.MaxManaIncrease;
                healthRegen -= eid.HealthRegenIncrease;
                manaRegen -= eid.ManaRegenIncrease;
            }
            if (eid.Elements != null)
            {
                Dictionary<Element, float> equipmentStats = eid.Elements;
                if (equipmentStats.Count > 0)
                {   
                    foreach (KeyValuePair<Element, float> equipElem in equipmentStats)
                    {
                        if (eid.EquipmentType == EquipmentType.Armor)
                        {
                            if (!totalResistances.ContainsKey(equipElem.Key))
                            {
                                totalResistances.Add(equipElem.Key, equipElem.Value);
                            }
                            else
                            {
                                if (added)
                                {
                                    totalResistances[equipElem.Key] += equipElem.Value;
                                }
                                else
                                {
                                    totalResistances[equipElem.Key] -= equipElem.Value;
                                }

                            }
                        }
                        else 
                        { 
                            if (!manaDiscounts.ContainsKey(equipElem.Key))
                            {
                                manaDiscounts.Add(equipElem.Key, equipElem.Value);
                            }
                            else
                            {
                                //curently manadiscounts is multiplied so maks no sense to add/subtract
                                if (added)
                                {
                                    //manaDiscounts[equipElem.Key] += equipElem.Value;
                                    manaDiscounts[equipElem.Key] *= equipElem.Value;
                                }
                                else
                                {
                                    //manaDiscounts[equipElem.Key] -= equipElem.Value;
                                    manaDiscounts[equipElem.Key] /= equipElem.Value;
                                }
                            }
                        }
                    }
                }

            }
        }
        if (GetComponent<PlayerUIScript>()!=null)
        {
            GetComponent<PlayerUIScript>().UpdateEquippmentStats();
        }
    }

}
