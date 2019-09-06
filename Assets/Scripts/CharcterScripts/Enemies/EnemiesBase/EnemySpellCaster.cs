using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpellCaster : EnemyAttackScript
{
    protected Spell spell;
    protected string spellName;
    //SpellCaster spellCasterScript;
    public override void Start () {
        base.Start();
        spell = new Spell(SpellManager.FindSpell(spellName));
        spell.GetScript(enemiesAttack.gameObject);
        spell.SetCasterAndFriends(gameObject);
        //spellCasterScript = GetComponent<SpellCaster>();
        //fireSpittle.SpellScript.Caster = gameObject;
        //fireSpittle.SpellScript.Caster = gameObject;
    }
    public void Update()
    {
        spell.CoolDownTimer();
    }
    public override void SetUpAttack()
    {
        if (spell != null)
        {
            if (spell.ReadyToCast)
            {
                spell.ResetTimer();
                base.SetUpAttack();
            }
        }
    }

    public override void CueAttack()
	{
        spell.SpellScript.InitiateAttack(gameObject, FindTarget());
    }
    public override void EndAttack()
    {
        spell.SpellScript.OnEndAttack();
    }
}
