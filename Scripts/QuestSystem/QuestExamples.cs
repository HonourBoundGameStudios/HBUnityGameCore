using System;
using UnityEngine;

namespace QuestSystem
{
    public class QuestExamples
    {
        // Example of creating complex quests
        public static Quest CreateTutorialChain()
        {
            ObjectiveKey tutorialButtonObjectiveKey = ScriptableObject.CreateInstance<ObjectiveKey>();
            tutorialButtonObjectiveKey.id = "tutorial_button";

            ObjectiveKey tutorialGemObjectiveKey = ScriptableObject.CreateInstance<ObjectiveKey>();
            tutorialGemObjectiveKey.id = "tutorial_gem";

            ObjectiveKey tutorialFishingAreaObjectiveKey = ScriptableObject.CreateInstance<ObjectiveKey>();
            tutorialFishingAreaObjectiveKey.id = "finish_area";

            return new QuestBuilder("VR Tutorial", "Learn the basics of VR interaction", QuestType.Tutorial)
                            .SetAutoAccept(true)
                            .SetAutoComplete(false) // Player needs to manually complete
                            .AddInteractionObjective(tutorialButtonObjectiveKey, "tutorial_button", "Press the Tutorial Button", "Find and press the glowing button")
                            .AddCollectionObjective(tutorialGemObjectiveKey, "tutorial_gem", 1, "Collect the Tutorial Gem", "Pick up the floating gem")
                            .AddLocationObjective(tutorialFishingAreaObjectiveKey, "finish_area", new Vector3(10, 0, 10), 2f, "Reach the Finish Area", "Walk to the marked circle")
                            .AddExperienceReward(200, "Tutorial completion")
                            .AddItemReward("starter_weapon", 1, "Beginner's weapon")
                            .AddFollowUpQuest("combat_tutorial")
                            .Build();
        }
        
        public static Quest CreateDynamicKillQuest(string enemyType, int amount, string area, int playerLevel)
        {
            ObjectiveKey tutorialKillEnemyObjectiveKey = ScriptableObject.CreateInstance<ObjectiveKey>();
            tutorialKillEnemyObjectiveKey.id = "kill_enemy_" + enemyType;

            // Scale rewards based on player level
            int goldReward = 50 * playerLevel;
            int expReward = 100 * playerLevel;
            
            return new QuestBuilder($"Hunt {enemyType.ToUpper()}", $"Eliminate {amount} {enemyType} creatures", QuestType.SideQuest)
                            .SetRequiredLevel(Math.Max(1, playerLevel - 2))
                            .AddKillObjective(tutorialKillEnemyObjectiveKey, enemyType, amount, area)
                            .AddCurrencyReward("gold", goldReward)
                            .AddExperienceReward(expReward)
                            .Build();
        }
    }
}
