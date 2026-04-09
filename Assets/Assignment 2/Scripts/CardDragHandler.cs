using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assignment2
{
    [RequireComponent(typeof(CanvasGroup))]
    public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image animalImage;

        private const float HoverScale = 1.2f;

        private AnimalDataSO animalData;
        private Vector3 startPosition;
        private Transform startParent;
        private int startSiblingIndex;
        private CanvasGroup canvasGroup;
        private bool isDragging;

        public AnimalDataSO AnimalData => animalData;

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Initialize(AnimalDataSO data)
        {
            animalData = data;
            if (animalImage != null && data.animalSprite != null)
                animalImage.sprite = data.animalSprite;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isDragging = true;
            transform.localScale = Vector3.one * HoverScale;

            startPosition = transform.position;
            startParent = transform.parent;
            startSiblingIndex = transform.GetSiblingIndex();
            transform.SetAsLastSibling();

            canvasGroup.alpha = 0.6f;
            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            isDragging = false;
            transform.localScale = Vector3.one;

            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;

            transform.position = startPosition;
            transform.SetParent(startParent);
            transform.SetSiblingIndex(startSiblingIndex);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!eventData.dragging)
                QuizEvents.CardClicked(animalData);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.localScale = Vector3.one * HoverScale;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isDragging)
                transform.localScale = Vector3.one;
        }

        public void HideCard()
        {
            gameObject.SetActive(false);
        }
    }
}
