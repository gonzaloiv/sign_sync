using System;

namespace DigitalLove.Game.Stats
{
    [Serializable]
    public class Stats
    {
        public const float InitialHealth = 5;

        public int score;
        public float health;

        public float HealthPercentage => health / InitialHealth;

        public void Reset()
        {
            score = 0;
            health = InitialHealth;
        }

        public void IncreaseScore(int value) => score += value;

        public void IncreaseHealth(float value) => health += value;

        public void DecreaseHealth(float value) => health -= value;
    }
}