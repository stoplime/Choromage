using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EquipmentType
{
    Armor,
    Weapon,
    Null
}

public class ExtraItemData_JSON {
    public string EquipmentTypeString { get; set; }
    public Dictionary<string, string> ElementStrings { get; set; }
    public int MaxHealthIncrease { get; set; }
    public int MaxManaIncrease { get; set; }
    public double ManaRegenIncrease { get; set; }
    public double HealthRegenIncrease { get; set; }
    public string CastSpeedString { get; set; }
}

public class ExtraItemData {
    public EquipmentType EquipmentType { get; set; }
    public Dictionary<Element, float> Elements { get; set; }
    public int MaxHealthIncrease { get; set; }
    public int MaxManaIncrease { get; set; }
    public float ManaRegenIncrease { get; set; }
    public float HealthRegenIncrease { get; set; }
    public float CastSpeed { get; set; }

    public override string ToString()
    {
        string temp = EquipmentType+"\t"+CastSpeed;
        return temp;
    }

    public void JSON_To_ExtraItemData(ExtraItemData_JSON eid)
    {
        this.MaxHealthIncrease = eid.MaxHealthIncrease;
        this.MaxManaIncrease = eid.MaxManaIncrease;
        this.ManaRegenIncrease = (float)eid.ManaRegenIncrease;
        this.HealthRegenIncrease = (float)eid.HealthRegenIncrease;

        ConvertStringToElements(eid.ElementStrings);
        ConvertStringToEquipmentType(eid.EquipmentTypeString);
        ConvertStringToCastSpeed(eid.CastSpeedString);
    }

    public ExtraItemData_JSON ToExtraItemData_JSON()
    {
        ExtraItemData_JSON eid = new ExtraItemData_JSON();
        eid.MaxHealthIncrease = this.MaxHealthIncrease;
        eid.MaxManaIncrease = this.MaxManaIncrease;
        eid.ManaRegenIncrease = this.ManaRegenIncrease;
        eid.HealthRegenIncrease = this.HealthRegenIncrease;
        
        eid.ElementStrings = new Dictionary<string, string>();
        if (this.Elements != null)  //TODO: Check to see if enclosing in if statement messed up anything~~~~~~~~~~
        {
            foreach (KeyValuePair<Element, float> element in this.Elements)
            {
                eid.ElementStrings.Add(element.Key.ToString(), element.Value.ToString());
            }
        }

        eid.EquipmentTypeString = EquipmentType.ToString();
        eid.CastSpeedString = this.CastSpeed.ToString();

        return eid;
    }

    public void ConvertStringToElements(Dictionary<string, string> ElementStrings)
    {
        //MonoBehaviour.print("convert");
        if (ElementStrings != null)
        {
            //MonoBehaviour.print("not null");
            if (ElementStrings.Count > 0)
            {
                // MonoBehaviour.print(ElementStrings.Count);
                Elements = new Dictionary<Element, float>();
                //Elements = new Dictionary<Element, string>();
                foreach (KeyValuePair<string, string> elem in ElementStrings)
                {
                    Element newElem;
                    switch (elem.Key.ToLower())
                    { 
                        case "water":
                            newElem = Element.water;
                            break;
                        case "earth":
                            newElem = Element.earth;
                            break;
                        case "fire":
                            newElem = Element.fire;
                            break;
                        case "air":
                            newElem = Element.air;
                            break;
                        case "life":
                            newElem = Element.life;
                            break;
                        case "death":
                            newElem = Element.death;
                            break;
                        default:
                            newElem = Element.neutral;
                            break;
                    }
                    //Help.print(newElem ,elem.Value);
                    float elemVal = float.Parse(elem.Value);
                    Elements.Add(newElem, float.Parse(elem.Value));
                }
            }
        }
    }

    void ConvertStringToEquipmentType(string EquipmentTypeString)
    {
        if (EquipmentTypeString != null)
        {
            switch (EquipmentTypeString.ToLower())
            { 
                case "armor":
                    EquipmentType = EquipmentType.Armor;
                    break;
                case "weapon":
                    EquipmentType = EquipmentType.Weapon;
                    break;
                default:
                    EquipmentType = EquipmentType.Null;
                    break;
            }
        }
    }

    void ConvertStringToCastSpeed(string CastSpeedString)
    {
        if (CastSpeedString != null && CastSpeedString != "")
        {
            CastSpeed = float.Parse(CastSpeedString);
        }
    }
    public string ExtraItemDescription()
    {
        string tip = "";
        if(MaxHealthIncrease!=0)
        {
            if (MaxHealthIncrease >= 0)
            {
                tip += string.Format("\nMax Health: +{0}", MaxHealthIncrease);
            }
            else
            { 
                tip += string.Format("\nMax Health: {0}", MaxHealthIncrease);
            }
        }
        if(HealthRegenIncrease!=0)
        {
            if (HealthRegenIncrease >= 0)
            {
                tip += string.Format("\nHealth Regen: +{0}", HealthRegenIncrease);
            }
            else
            { 
                tip += string.Format("\nHealth Regen: {0}", HealthRegenIncrease);
            }
        }
        if(MaxManaIncrease!=0)
        {
            if (MaxManaIncrease >= 0)
            {
                tip += string.Format("\nMax Mana: +{0}", MaxManaIncrease);
            }
            else
            { 
                tip += string.Format("\nMax Mana: {0}", MaxManaIncrease);
            }
        }
        if(ManaRegenIncrease!=0)
        {
            if (ManaRegenIncrease >= 0)
            {
                tip += string.Format("\nMana Regen: +{0}", ManaRegenIncrease);
            }
            else
            {
                tip += string.Format("\nMana Regen: {0}", ManaRegenIncrease);
            }
        }
        if(CastSpeed!=0)
        {
            if (CastSpeed >= 0)
            {
                tip += string.Format("\nCasting Speed: +{0}", CastSpeed);
            }
            else
            { 
                tip += string.Format("\nCasting Speed: {0}", CastSpeed);
            }
        }
        if (Elements != null)
        {
            if (EquipmentType == EquipmentType.Weapon)
            {
                tip += ManaCostDescription();
            }
            else 
            {
                tip += ArmorElementDescription();
            }
        }
        return tip;
    }
    string ManaCostDescription()
    {
        float manaDicount = 1f - Elements[Element.neutral];
        manaDicount *= 100f;
        if (manaDicount > 0)
        {
            return string.Format("\nMana Cost: -{0}%", manaDicount);
        }
        else if (manaDicount == 0f)
        {
            return "";
        }
        else
        {
            return string.Format("\nMana Cost: +{0}%", manaDicount);

        }

    }
    string ArmorElementDescription()
    {
        string defense = "";
        // foreach (var item in collection)
        // {
        if (Elements.ContainsKey(Element.neutral))
        {
            defense += string.Format("\nDefense: +{0}", Elements[Element.neutral]);
        }
        if (Elements.ContainsKey(Element.fire))
        {
            defense += string.Format("\nFire Defense: +{0}", Elements[Element.fire]);
        }
        return defense;
        // }
    }
}