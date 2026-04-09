using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

namespace Assignment2
{
    public class UIManager : MonoBehaviour
    {
        [Header("HUD")]
        [SerializeField] private TextMeshProUGUI redBucketText;
        [SerializeField] private TextMeshProUGUI blueBucketText;
        [SerializeField] private TextMeshProUGUI feedbackText;

        [Header("Pop up")]
        [SerializeField] private GameObject popupPanel;
        [SerializeField] private TextMeshProUGUI popupTitleText;
        [SerializeField] private TextMeshProUGUI popupDescText;

        [Header("Finish Screen")]
        [SerializeField] private GameObject finishScreenPanel;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI congratulationText;

        [Header("Review Scroll Views")]
        [SerializeField] private GameObject correctParentContainer;
        [SerializeField] private GameObject correctScrollView;
        [SerializeField] private Transform correctListParent;

        [SerializeField] private GameObject incorrectParentContainer;
        [SerializeField] private GameObject incorrectScrollView;
        [SerializeField] private Transform incorrectListParent;

        [SerializeField] private GameObject resultCardPrefab;

        private void Start()
        {
            popupPanel.SetActive(false);
            finishScreenPanel.SetActive(false);
            feedbackText.text = "";
        }

        private void OnEnable()
        {
            QuizEvents.OnCategorySelected += HandleCategorySelected;
            QuizEvents.OnCardDropped += HandleCardDropped;
            QuizEvents.OnCardClicked += HandleCardClicked;
            QuizEvents.OnQuizFinished += HandleQuizFinished;
        }

        private void OnDisable()
        {
            QuizEvents.OnCategorySelected -= HandleCategorySelected;
            QuizEvents.OnCardDropped -= HandleCardDropped;
            QuizEvents.OnCardClicked -= HandleCardClicked;
            QuizEvents.OnQuizFinished -= HandleQuizFinished;
        }

        private void HandleCategorySelected(QuizCategory category)
        {
            switch (category)
            {
                case QuizCategory.FlyingVsNonFlying:
                    redBucketText.text = "Flying";
                    blueBucketText.text = "Non-Flying";
                    break;
                case QuizCategory.InsectVsNonInsect:
                    redBucketText.text = "Insect";
                    blueBucketText.text = "Non-Insect";
                    break;
                case QuizCategory.OmnivorousVsHerbivorous:
                    redBucketText.text = "Omnivorous";
                    blueBucketText.text = "Herbivorous";
                    break;
                case QuizCategory.GroupVsSolo:
                    redBucketText.text = "Lives in Group";
                    blueBucketText.text = "Solo";
                    break;
                case QuizCategory.EggsVsBirth:
                    redBucketText.text = "Lays Eggs";
                    blueBucketText.text = "Gives Birth";
                    break;
            }
        }

        private void HandleCardDropped(AnimalDataSO animal, bool isCorrect)
        {
            if (isCorrect)
                ShowFeedback("Correct!", Color.green);
            else
                ShowFeedback("Incorrect...", Color.red);
        }

        private void HandleCardClicked(AnimalDataSO animal)
        {
            popupTitleText.text = animal.animalName;
            popupDescText.text = animal.description;
            popupPanel.SetActive(true);
        }

        private void HandleQuizFinished(QuizResult result)
        {
            finishScreenPanel.SetActive(true);
            scoreText.text = $"Final Score: {result.Score} / {result.Total}";

            if (result.Score == result.Total)
                congratulationText.text = "Perfect Score! Amazing!";
            else if (result.Score > result.Total / 2)
                congratulationText.text = "Great job! Keep practicing.";
            else
                congratulationText.text = "Good effort! Let's review your mistakes.";

            PopulateScrollView(result.CorrectAnimals, correctParentContainer, correctScrollView, correctListParent);
            PopulateScrollView(result.IncorrectAnimals, incorrectParentContainer, incorrectScrollView, incorrectListParent);
        }

        private void PopulateScrollView(List<AnimalDataSO> animals, GameObject parentContainer, GameObject scrollView, Transform listParent)
        {
            bool hasItems = animals != null && animals.Count > 0;

            if (parentContainer != null) parentContainer.SetActive(hasItems);
            if (scrollView != null) scrollView.SetActive(hasItems);

            if (!hasItems) return;

            foreach (Transform child in listParent) Destroy(child.gameObject);

            foreach (var animal in animals)
            {
                GameObject cardObj = Instantiate(resultCardPrefab, listParent);
                var card = cardObj.GetComponent<CardDragHandler>();
                if (card == null) continue;

                card.Initialize(animal);
                card.enabled = false;
            }
        }

        private void ShowFeedback(string message, Color color)
        {
            feedbackText.text = message;
            feedbackText.color = color;
            CancelInvoke(nameof(ClearFeedback));
            Invoke(nameof(ClearFeedback), 2f);
        }

        private void ClearFeedback()
        {
            feedbackText.text = "";
        }

        public void ClosePopup()
        {
            popupPanel.SetActive(false);
        }
    }
}
