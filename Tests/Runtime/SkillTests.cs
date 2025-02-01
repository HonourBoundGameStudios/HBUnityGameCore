using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace HBUnityGameCore
{
    [TestFixture]
    public class SkillTests
    {
        [Test]
        public void Skill_WhenAChargeIsAvailable_CanUsed()
        {
            // Arrange
			Skill skill = ScriptableObject.CreateInstance<Skill>();
			skill.Initialize(1, 1, 1);

			// Act/Assert
			Assert.True(skill.CanUse());
			skill.Use();

			Assert.False(skill.CanUse());
        }

        [Test]
        public void Skill_WhenMultipleChargesAreAvailable_CanUseMultipleTimes()
        {
            // Arrange
            Skill skill = ScriptableObject.CreateInstance<Skill>();
            skill.Initialize(3, 3, 1);

            // Act/Assert
            Assert.True(skill.CanUse());
            skill.Use();
            Assert.True(skill.GetCurrentCharges() == 2);
            Assert.True(skill.CanUse());

            skill.Use();
            Assert.True(skill.GetCurrentCharges() == 1);
            Assert.True(skill.CanUse());

            skill.Use();
            Assert.True(skill.GetCurrentCharges() == 0);
            Assert.False(skill.CanUse());
        }

        [UnityTest]
        public IEnumerator Skill_WhenRecharged_CanUse()
        {
            // Arrange
            Skill skill = ScriptableObject.CreateInstance<Skill>();
            skill.Initialize(0, 1, 2);
            yield return new WaitForSeconds(2);
            skill.Recharge();
            Assert.True(skill.CanUse());
        }

        [UnityTest]
        public IEnumerator Skill_WhenNotRechargedFully_CannotUse()
        {
            // Arrange
            Skill skill = ScriptableObject.CreateInstance<Skill>();
            skill.Initialize(0, 1, 1);

            // Act
            yield return new WaitForSeconds(1);
            skill.Recharge();

            // Assert
            Assert.False(skill.CanUse());
        }

        [UnityTest]
        public IEnumerator Skill_WhenRechargedWithBooster_CanUse()
        {
            // Arrange
            float booster = 2;
            Skill skill = ScriptableObject.CreateInstance<Skill>();
            skill.Initialize(1, 1, 2, booster);

            // Act
            skill.Use();
            yield return new WaitForSeconds(1);
            skill.Recharge();

            // Assert
            Assert.True(skill.CanUse());
        }
    }
}
