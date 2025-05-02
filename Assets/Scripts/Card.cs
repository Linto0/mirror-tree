using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public GameObject front;
    public GameObject back;
    public int cardId;

    private Button button;
    private bool isFlipped = false;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(FlipCard);
        HideCard();
    }

    public void SetCard(int id, Sprite frontImage)
    {
        cardId = id;
        front.GetComponent<Image>().sprite = frontImage;
        HideCard();
    }

    public void FlipCard()
    {
        if (isFlipped) return;

        isFlipped = true;
        front.SetActive(true);
        back.SetActive(false);

        // Inform GameManager about the card flip
        FindObjectOfType<GameManager>().CardRevealed(this);
    }

    public void HideCard()
    {
        isFlipped = false;
        front.SetActive(false);
        back.SetActive(true);
    }

    public void DisableCard()
    {
        GetComponent<Button>().interactable = false;
    }
}
