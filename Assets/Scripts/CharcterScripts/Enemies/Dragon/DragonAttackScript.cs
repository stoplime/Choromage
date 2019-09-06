using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAttackScript : EnemySpellCaster
{
    //Spell fireBlast;
    //bool attackFinished;
    public override void Start () {
        spellName = "EnemyFireball";
        base.Start();
        // fireBlast = SpellManager.FindSpell("EnemyFireball");
        // fireBlast.GetScript(enemiesAttack.gameObject);
        // fireBlast.SetCasterAndFriends(gameObject);
        //fireSpittle.SpellScript.Caster = gameObject;
        //fireSpittle.SpellScript.Caster = gameObject;
        
    }

    // public override void CueAttack()
	// {
    //     fireBlast.SpellScript.InitiateAttack(gameObject, FindTarget());
    // }
    // public override void EndAttack()
    // {
    //     print("end attack");
    //     fireBlast.SpellScript.OnEndAttack();
    // }
    // public void attackHit()
    // {
    //     print("Attck");
    //     CueAttack();
    // }
}

    
