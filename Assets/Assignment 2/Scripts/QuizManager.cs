using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assignment2
{
    public class QuizManager : MonoBehaviour
    {
        public static QuizManager Instance { get; private set; }

        [Header("References")]
        [SerializeField] private GameObject cardPrefab;
        [SerializeField] private Transform cardSpawnArea;

        [Header("Data")]
        [SerializeField] private List<AnimalDataSO> allAnimals;

        private QuizCategory currentCategory;
        private int totalCards;
        private int currentScore;
        private int cardsProcessed;
        private readonly List<AnimalDataSO> correctAnimals = new List<AnimalDataSO>();
        private readonly List<AnimalDataSO> incorrectAnimals = new List<AnimalDataSO>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            totalCards = allAnimals.Count;
            if (totalCards == 0)
            {
                Debug.LogWarning("No Animal Data assigned to QuizManager!");
                return;
            }

            SelectRandomCategory();
            QuizEvents.CategorySelected(currentCategory);
            SpawnCards();
        }

        private void SelectRandomCategory()
        {
            int categoryCount = Enum.GetValues(typeof(QuizCategory)).Length;
            currentCategory = (QuizCategory)UnityEngine.Random.Range(0, categoryCount);
        }

        private void SpawnCards()
        {
            ShuffleAnimals();

            foreach (var animal in allAnimals)
            {
                GameObject cardObj = Instantiate(cardPrefab, cardSpawnArea);
                var card = cardObj.GetComponent<CardDragHandler>();
                if (card != null) card.Initialize(animal);
            }
        }

        private void ShuffleAnimals()
        {
            for (int i = 0; i < allAnimals.Count; i++)
            {
                int randomIndex = UnityEngine.Random.Range(i, allAnimals.Count);
                (allAnimals[i], allAnimals[randomIndex]) = (allAnimals[randomIndex], allAnimals[i]);
            }
        }

        public void EvaluateDrop(AnimalDataSO animal, bool isPlacedInTrueBucket)
        {
            bool isCorrect = isPlacedInTrueBucket == animal.MatchesCategory(currentCategory);

            if (isCorrect)
            {
                currentScore++;
                correctAnimals.Add(animal);
            }
            else
            {
                incorrectAnimals.Add(animal);
            }

            QuizEvents.CardDropped(animal, isCorrect);
            cardsProcessed++;

            if (cardsProcessed >= totalCards)
            {
                QuizEvents.QuizFinished(new QuizResult
                {
                    Score = currentScore,
                    Total = totalCards,
                    CorrectAnimals = correctAnimals,
                    IncorrectAnimals = incorrectAnimals
                });
            }
        }
    }
}
