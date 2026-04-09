using UnityEngine;
using UnityEngine.EventSystems;

namespace Assignment2
{
    public class Bucket : MonoBehaviour, IDropHandler
    {
        [SerializeField] private bool isTrueBucket;

        public void OnDrop(PointerEventData eventData)
        {
            if (!eventData.pointerDrag) return;

            var card = eventData.pointerDrag.GetComponent<CardDragHandler>();
            if (!card) return;

            QuizManager.Instance.EvaluateDrop(card.AnimalData, isTrueBucket);
            card.HideCard();
        }
    }
}
