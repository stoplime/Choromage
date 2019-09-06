using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class ThorinStateMachine : MonoBehaviour
{
    public enum TutorialStatus
    { 
        intialize,
        playerBeingAttacked,
        attackingWolf,
        teachingFirstSpell,
        playerLearned,
        tutorialDone
    }

    private bool casting;

	// public bool Casting
	// {
    //     set { casting = value;
    //         if (casting == true)
    //         {
    //             anime.SetTrigger("Attack");
    //         }
    //     }
    // }
    bool moving;
    //private JsonData thorinsSpells;
    TutorialStatus status;
    bool taughtSpell;
    float maxWolfDistance =7.5f;
    float walkingSpeed =3;

    float innterventionTimer = 5f;

    bool WolfDead
    {
        get
        {
            if (wolf == null)
            {
                // if (wolf.GetComponent<Stats>().Health <= 2.5f)
                // {
                    return true;
                // }
            }
            return false;
        }
    }

    GameObject wolf;

    bool PlayerNeedsHelp
    {
        get
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            if(GameManager.player.GetComponent<PlayerStats>().Health <GameManager.player.GetComponent<PlayerStats>().MaxHealth*(3/4))
            {
                return true;
            }
            if (GeometryUtility.TestPlanesAABB(planes, gameObject.GetComponent<Collider>().bounds))
            {
                return true;
            }
            if (innterventionTimer <= 0)
            {
                return true;
            }
            return false;
        }
    }

    Dictionary<string,Spell> ThorinSpellBook = new Dictionary<string,Spell>();
    Animator anime;
    string currentSpell;

    // Start is called before the first frame update
    void Start()
    {
        status = TutorialStatus.intialize;
        Quest q = QuestManager.GetQuestByID("ThorinIntroQuest");
        if(q.ID != null)
        {
            if (q.Completed)
            {
                status = TutorialStatus.tutorialDone;
            }
        }
        anime = GetComponent<Animator>();
        // InitalizeSpells();
        // finishedTutorial = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if(GameManager.DialogueBox.Variables.ContainsKey("QuestStatus_ThorinIntroQuest"))
        // print(GameManager.DialogueBox.Variables["QuestStatus_ThorinIntroQuest"]);
        // Quest q = QuestManager.GetQuestByID("ThorinIntroQuest");
        // if(q.ID != null)
        // {
        //     Help.print("Thorin Quest Status", q.Status);
        // }
        innterventionTimer -= Time.deltaTime;
        if (currentSpell != null)
        {
            ThorinSpellBook[currentSpell].CoolDownTimer();
        }
        // print(status);
        switch (status)
        {
            case TutorialStatus.intialize:
                InitalizeSpells();
                moving = false;
                break;
            case TutorialStatus.playerBeingAttacked:
                if (PlayerNeedsHelp)
                {
                    wolf = GameObject.Find("Tutorial Wolf");
                    status = TutorialStatus.attackingWolf;
                }
                break;
            case TutorialStatus.attackingWolf:
                if (WolfDead)
                {
                    InitateConversation();
                    status = TutorialStatus.teachingFirstSpell;
                    //TODO: make sure status updates if quits during the tutorial
                    // GameManager.player.GetComponent<PlayerStats>().FullHeal();
                }
                else
                {
                    AttackingWolf();
                }
                break;
            case TutorialStatus.teachingFirstSpell:
                moving = false;
                CheckWolfRespawn();
                break;
            default:
                moving = false;
                CheckWolfRespawn();
                break;
        }
        UpdateAnimator();
    }
    void CheckWolfRespawn()
    {
        if (GameObject.Find("Tutorial Wolf"))
        { 
            wolf = GameObject.Find("Tutorial Wolf");
            AttackingWolf();
        }
    }
    void UpdateAnimator()
    {
        // print("hello");
        // if (casting)
        // { 
        //     anime.SetTrigger("Attack")
        // }
        anime.SetBool("Moving", moving);
    }
    void InitalizeSpells()
    {
        List<GameObject> friends = new List<GameObject>();
        friends.Add(GameManager.player);
        JsonData thorinsSpells = JsonMapper.ToObject(File.ReadAllText(Application.dataPath + "/StreamingAssets/bin/ThorinsSpells.json"));
        for (int i = 0; i < thorinsSpells.Count; i++)
		{
            Spell temp = SpellManager.FindSpell(thorinsSpells[i]["Name"].ToString());

            temp.GetScript(gameObject.transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R/ThorinsMagicStaff/OtherCantisStone").gameObject);

            temp.SetCasterAndFriends(gameObject, friends);
            ThorinSpellBook.Add(temp.Name, temp);
        }
        status = TutorialStatus.playerBeingAttacked;
        // spell.GetScript(enemiesAttack.gameObject);
        // spell.SetCasterAndFriends(gameObject);
    }
    void AttackingWolf()
    {
        if (Vector2.Distance(gameObject.transform.position, wolf.transform.position) > maxWolfDistance)
        {
            moving = true;
            WalkToWolf();
        }
        else
        {
            moving = false;
            currentSpell = "ThorinInferno";
            FireBallWolf();
        }
    }
    void AttackingAnimationDone()
    { 
        
    }
    void FireBallWolf()
    { 
        if (ThorinSpellBook[currentSpell].ReadyToCast)
        {
            ThorinSpellBook[currentSpell].ResetTimer();
            ThorinSpellBook[currentSpell].SpellScript.InitiateAttack(gameObject, wolf);
            casting = true;
            anime.SetTrigger("Attack");
        }
    }
    public void EndAttack()
    {
        ThorinSpellBook[currentSpell].SpellScript.OnEndAttack();
        //currentSpell.OnEndAttack();
        //currentSpell = null;
        casting = false;
        
            // print("done");
    }
    void WalkToWolf()
    {
        FaceWolf();
        WalkForward();
    }
    void InitateConversation()
    {
        gameObject.GetComponent<NPCDialogueDetector>().StartTalking(gameObject.name);
        GameManager.player.GetComponent<PlayerStats>().FullHeal();
        // SpellManager.UnlockNewSpell("Fireball");
    }
    void WalkForward()
    {
        moving = true;
        transform.Translate(Vector3.forward * Time.deltaTime* walkingSpeed);
    }
    void FaceWolf()
    {
        Vector3 tempPlayerPos = wolf.transform.position;
        tempPlayerPos.y = transform.position.y;
        transform.rotation = Quaternion.LookRotation(tempPlayerPos- transform.position);
    }
    void TutorialDone()
    {
        GameManager.InTutorial = false;
    }

    // void FacePlayer()
    // {
    //     Vector3 tempPlayerPos = GameManager.PlayerPos;
    //     tempPlayerPos.y = transform.position.y;
    //     transform.rotation = Quaternion.LookRotation(tempPlayerPos- transform.position);
    // }
    void Hit()
	{
	}

	void FootR()
	{
	}

	void FootL()
	{
	}
    public void Despawn()
    {
        Destroy(gameObject);
    }
    public void SetCurrentState()
    {
        
    }
}
