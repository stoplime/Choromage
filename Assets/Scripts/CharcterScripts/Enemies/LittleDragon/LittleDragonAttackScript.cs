using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDragonAttackScript : EnemySpellCaster
{
    //private Spell fireSpittle;
    public override void Start () {
        spellName = "FireSpittle";
        base.Start();
        // fireSpittle = SpellManager.FindSpell("FireSpittle");
        // fireSpittle.GetScript(enemiesAttack.gameObject);
        // fireSpittle.SetCasterAndFriends(gameObject);
        //fireSpittle.SpellScript.Caster = gameObject;
        //fireSpittle.SpellScript.Caster = gameObject;
    }

    // public override void CueAttack()
	// {
    //     fireSpittle.SpellScript.InitiateAttack(gameObject);
    // }
    // public override void EndAttack()
    // {
    //     fireSpittle.SpellScript.OnEndAttack();
    // }
}
