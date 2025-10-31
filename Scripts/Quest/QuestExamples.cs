using System;
using UnityEngine;

namespace VRGame.QuestSystem
{
    public class QuestExamples
    {
        // Example of creating complex quests
        public static Quest CreateTutorialChain()
        {
            return new QuestBuilder("VR Tutorial", "Learn the basics of VR interaction", QuestType.Tutorial)
                            .SetAutoAccept(true)
                            .SetAutoComplete(false) // Player needs to manually complete
                            .AddInteractionObjective("tutorial_button", "Press the Tutorial Button", "Find and press the glowing button")
                            .AddCollectionObjective("tutorial_gem", 1, "Collect the Tutorial Gem", "Pick up the floating gem")
                            .AddLocationObjective("finish_area", new Vector3(10, 0, 10), 2f, "Reach the Finish Area", "Walk to the marked circle")
                            .AddExperienceReward(200, "Tutorial completion")
                            .AddItemReward("starter_weapon", 1, "Beginner's weapon")
                            .AddFollowUpQuest("combat_tutorial")
                            .Build();
        }
        
        public static Quest CreateDynamicKillQuest(string enemyType, int amount, string area, int playerLevel)
        {
            // Scale rewards based on player level
            int goldReward = 50 * playerLevel;
            int expReward = 100 * playerLevel;
            
            return new QuestBuilder($"Hunt {enemyType.ToUpper()}", $"Eliminate {amount} {enemyType} creatures", QuestType.SideQuest)
                            .SetRequiredLevel(Math.Max(1, playerLevel - 2))
                            .AddKillObjective(enemyType, amount, area)
                            .AddCurrencyReward("gold", goldReward)
                            .AddExperienceReward(expReward)
                            .Build();
        }
        
        public static Quest CreateMultiStageQuest()
        {
            return new QuestBuilder("The Ancient Artifact", "Recover the lost artifact", QuestType.MainStory)
                            .AddInteractionObjective("sage_npc", "Speak to the Sage", "Talk to the village sage")
                            .AddCollectionObjective("ancient_key", 1, "Find the Ancient Key", "Locate the key to the temple")
                            .AddLocationObjective("ancient_temple", new Vector3(50, 0, 50), 5f, "Reach the Ancient Temple", "Travel to the temple location")
                            .AddKillObjective("temple_guardian", 1, "ancient_temple", "Defeat the Temple Guardian", "Battle the guardian protecting the artifact")
                            .AddCollectionObjective("ancient_artifact", 1, "Retrieve the Artifact", "Take the ancient artifact")
                            .AddCurrencyReward("gold", 1000)
                            .AddExperienceReward(500)
                            .AddItemReward("artifact_power", 1, "Ancient Power")
                            .Build();
        }
    }
}
