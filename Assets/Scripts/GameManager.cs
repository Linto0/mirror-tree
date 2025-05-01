using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardFaces;
    public Transform cardParent;
    public Vector2 gridSize = new Vector2(2, 2); // for 2x2 layout

    private List<Card> flippedCards = new List<Card>();

    void Start()
    {
        GenerateCards();
    }

    void GenerateCards()
    {
        List<int> ids = new List<int>();
        int totalCards = (int)(gridSize.x * gridSize.y);

        for (int i = 0; i < totalCards / 2; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        Shuffle(ids);

        GridLayoutGroup grid = cardParent.GetComponent<GridLayoutGroup>();
        if (grid != null)
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = (int)gridSize.x;
        }

        for (int i = 0; i < totalCards; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, cardParent);
            Card card = newCard.GetComponent<Card>();
            card.SetCard(ids[i], cardFaces[ids[i]]);
        }
    }

    public void CardRevealed(Card card)
    {
        flippedCards.Add(card);

        if (flippedCards.Count == 2)
        {
            StartCoroutine(CheckMatch());
        }
    }

    System.Collections.IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(0.5f);

        if (flippedCards[0].cardId == flippedCards[1].cardId)
        {
            flippedCards[0].Disable();
            flippedCards[1].Disable();
        }
        else
        {
            flippedCards[0].HideCard();
            flippedCards[1].HideCard();
        }

        flippedCards.Clear();
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }
}
