using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Card Setup")]
    public GameObject cardPrefab;
    public Sprite[] cardFaces;
    public Transform cardParent;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public GameObject winPanel;

    [Header("Audio Clips")]
    public AudioClip flipSound;
    public AudioClip matchSound;
    public AudioClip mismatchSound;
    public AudioClip winSound;

    private AudioSource audioSource;

    private List<Card> flippedCards = new List<Card>();
    private List<int> matchedCardIds = new List<int>();
    private int score = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        winPanel.SetActive(false);
        GenerateCards();
        UpdateScoreUI();
    }

    void GenerateCards()
    {
        List<int> cardIds = new List<int> { 0, 0, 1, 1 };
        Shuffle(cardIds);

        foreach (Transform child in cardParent)
            Destroy(child.gameObject);

        foreach (int id in cardIds)
        {
            GameObject cardGO = Instantiate(cardPrefab, cardParent);
            Card card = cardGO.GetComponent<Card>();
            card.SetCard(id, cardFaces[id]);
        }
    }

    public void CardRevealed(Card card)
    {
        if (flippedCards.Contains(card) || matchedCardIds.Contains(card.cardId))
            return;

        flippedCards.Add(card);
        PlayFlipSound();

        if (flippedCards.Count == 2)
            StartCoroutine(CheckMatch());
    }

    IEnumerator CheckMatch()
    {
        yield return new WaitForSeconds(1f);

        if (flippedCards[0].cardId == flippedCards[1].cardId)
        {
            PlayMatchSound();
            flippedCards[0].DisableCard();
            flippedCards[1].DisableCard();

            matchedCardIds.Add(flippedCards[0].cardId);
            score++;
            UpdateScoreUI();

            if (matchedCardIds.Count == cardFaces.Length)
                ShowWinScreen();
        }
        else
        {
            PlayMismatchSound();
            flippedCards[0].HideCard();
            flippedCards[1].HideCard();
        }

        flippedCards.Clear();
    }

    void ShowWinScreen()
    {
        winPanel.SetActive(true);
        PlayWinSound();
    }

    public void ResetGame()
    {
        matchedCardIds.Clear();
        flippedCards.Clear();
        score = 0;
        winPanel.SetActive(false);
        GenerateCards();
        UpdateScoreUI();
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("Score", score);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    // Sound Methods
    void PlayFlipSound()
    {
        if (flipSound != null && audioSource != null)
            audioSource.PlayOneShot(flipSound);
    }

    void PlayMatchSound()
    {
        if (matchSound != null && audioSource != null)
            audioSource.PlayOneShot(matchSound);
    }

    void PlayMismatchSound()
    {
        if (mismatchSound != null && audioSource != null)
            audioSource.PlayOneShot(mismatchSound);
    }

    void PlayWinSound()
    {
        if (winSound != null && audioSource != null)
            audioSource.PlayOneShot(winSound);
    }

    void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(0, list.Count);
            int temp = list[i];
            list[i] = list[rnd];
            list[rnd] = temp;
        }
    }
}
