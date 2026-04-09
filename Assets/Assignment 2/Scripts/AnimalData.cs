using UnityEngine;

namespace Assignment2
{
    [CreateAssetMenu(fileName = "NewAnimal", menuName = "Animal Data")]
    public class AnimalDataSO : ScriptableObject
    {
        public string animalName;
        public string description;
        public Sprite animalSprite;

        [Header("Traits")]
        public bool isFlying;
        public bool isInsect;
        public bool isOmnivorous;
        public bool isGroup;
        public bool isEggLaying;

        public bool MatchesCategory(QuizCategory category)
        {
            switch (category)
            {
                case QuizCategory.FlyingVsNonFlying: return isFlying;
                case QuizCategory.InsectVsNonInsect: return isInsect;
                case QuizCategory.OmnivorousVsHerbivorous: return isOmnivorous;
                case QuizCategory.GroupVsSolo: return isGroup;
                case QuizCategory.EggsVsBirth: return isEggLaying;
                default: return false;
            }
        }
    }

    public enum QuizCategory
    {
        FlyingVsNonFlying,
        InsectVsNonInsect,
        OmnivorousVsHerbivorous,
        GroupVsSolo,
        EggsVsBirth
    }
}
