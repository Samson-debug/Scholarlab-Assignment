using System;
using System.Collections.Generic;

namespace Assignment2
{
    public struct QuizResult
    {
        public int Score;
        public int Total;
        public List<AnimalDataSO> CorrectAnimals;
        public List<AnimalDataSO> IncorrectAnimals;
    }

    public static class QuizEvents
    {
        public static event Action<QuizCategory> OnCategorySelected;
        public static event Action<AnimalDataSO, bool> OnCardDropped;
        public static event Action<AnimalDataSO> OnCardClicked;
        public static event Action<QuizResult> OnQuizFinished;

        public static void CategorySelected(QuizCategory category) => OnCategorySelected?.Invoke(category);
        public static void CardDropped(AnimalDataSO animal, bool isCorrect) => OnCardDropped?.Invoke(animal, isCorrect);
        public static void CardClicked(AnimalDataSO animal) => OnCardClicked?.Invoke(animal);
        public static void QuizFinished(QuizResult result) => OnQuizFinished?.Invoke(result);
    }
}
