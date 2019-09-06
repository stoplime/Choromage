using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class QuestManager {
	public static List<Quest> ListOfQuests = new List<Quest>();

	public static void Initialize() 
	{
        ListOfQuests.Clear();
		ListOfQuests.Add(new QuestDragonHide());
		ListOfQuests.Add(new QuestFur());
        ListOfQuests.Add(new ThorinIntroQuest());
        ListOfQuests.Add(new RoseQuest1());
        ListOfQuests.Add(new FindRose());
        ListOfQuests.Add(new DragonQuest2());
        ListOfQuests.Add(new ThorinNecromancyQuest());
        ListOfQuests.Add(new TavernKeeperQuest());

        QuestAttention.Initialize();
    }

    public static Quest GetQuestByID(string ID)
    {
        foreach (Quest q in QuestManager.ListOfQuests)
        {
            if(q.ID == ID)
                return q;
        }
        return null;
    }
	
}
