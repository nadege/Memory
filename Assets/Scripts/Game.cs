using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    public enum GameState
    {
        Menu,
        Game,
        Win
    }

    public enum GameMode
    {
        Hard,
        Medium,
        Easy
    }

    [SerializeField]
    private float m_BoardWidth = 20f;
    [SerializeField]
    private float m_BoardHeight = 12.5f;
    [SerializeField]
    private float m_ShowBeforeHide = 0.5f;
    [SerializeField]
    private float m_CardOffset = 3;
    [SerializeField]
    private float m_CardWidth = 0;
    
    private int m_Width;
    private int m_Height;

    public Vector3 StartPositionHard;
    public Vector3 StartPositionMedium;
    public Vector3 StartPositionEasy;

    private int m_WidthHard = 5;
    private int m_HeightHard = 4;
    private int m_WidthMedium = 3;
    private int m_HeightMedium = 4;
    private int m_WidthEasy = 3;
    private int m_HeightEasy = 2;


    public GameObject CardPrefab;
    public GameObject StarParticulePrefab;
    public List<Sprite> Sprites;
    public Sprite BackSprite;

    public int PairCount;

    public readonly List<Card> ShowingCards = new List<Card>();
    private List<GameObject> Cards;

    public GameState State { get; private set; }
    public GameMode Difficulty;

    private float delay;
    private int failCount;
    private int remainingCards;

   // public GUIText FailCount;
    
    
    void Start()
    {
        delay = 0.0f;
        failCount = 0;
        remainingCards = 0;
        ShowingCards.Clear();
        State = GameState.Menu;
     //   FailCount.enabled = false;

        Object.DontDestroyOnLoad(this.gameObject);
    }

    void StartGame ()
    {
        switch (Difficulty)
        {
            case GameMode.Hard:
                this.m_Width = m_WidthHard;
                this.m_Height = m_HeightHard;
                break;
            case GameMode.Medium:
                this.m_Width = m_WidthMedium;
                this.m_Height = m_HeightMedium;
                break;
            case GameMode.Easy:
                this.m_Width = m_WidthEasy;
                this.m_Height = m_HeightEasy;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }


        // Scale
        float scaleValue = 0.8f;

        List<int> spriteIndex = new List<int>();
        for (int i = 0; i < this.Sprites.Count; i++)
	    {
	        spriteIndex.Add(i);
	    }

        this.remainingCards = PairCount * 2;

        Cards = this.GenerateCards(spriteIndex);
        var cards = new List<GameObject>(Cards);

        float widthSpaceNeeded = (m_Width - 1) * (m_CardOffset + m_CardWidth) * scaleValue;
        float heigthSpaceNeeded = (m_Height - 1) * (m_CardOffset + m_CardWidth) * scaleValue;

        Vector3 startingPosition = new Vector3((m_BoardWidth - widthSpaceNeeded) / 2, (m_BoardHeight - heigthSpaceNeeded) / 2);

        Vector3 scale = new Vector3(scaleValue, scaleValue);
        foreach (var card in Cards)
        {
            card.transform.localScale = scale;
        }

        int cardToPlace = Cards.Count;
        Debug.Log("Starting position " + startingPosition);
        for (int i = 0; i < this.m_Height && cardToPlace > 0; i++)
	    {
            for (int j = 0; j < this.m_Width && cardToPlace > 0; j++)
            {
                Vector3 position = startingPosition + new Vector3((m_CardOffset + m_CardWidth) * scaleValue * j, (m_CardOffset + m_CardWidth) * scaleValue * i);

                int index = Random.Range(0, cards.Count - 1);
                GameObject card = cards[index];
                card.transform.position = position;
                cards.Remove(card);
                cardToPlace--;
            }    
	    }

        State = GameState.Game;
     //   FailCount.enabled = true;
	}

    private List<GameObject> GenerateCards(List<int> spriteIndex)
    {
        List<GameObject> cards = new List<GameObject>();
        for (int i = 0;  i < this.PairCount; i++)
        {
            int index = Random.Range(0, spriteIndex.Count - 1);
            Sprite texture = this.Sprites[spriteIndex[index]];
            spriteIndex.Remove(spriteIndex[index]);

            GameObject cardGameObject = Instantiate(this.CardPrefab) as GameObject;
            cardGameObject.transform.parent = this.transform;
            Card card = cardGameObject.GetComponent<Card>();
            card.Picture.sprite = texture;
            cards.Add(cardGameObject);

            cardGameObject = Instantiate(this.CardPrefab) as GameObject;
            cardGameObject.transform.parent = this.transform;
            card = cardGameObject.GetComponent<Card>();
            card.Picture.sprite = texture;
            cards.Add(cardGameObject);
        }
        return cards;
    }

    void Update () 
    {
        if (Application.loadedLevelName == "Game" && State == GameState.Menu)
        {
            StartGame();
        }

		if (ShowingCards.Count == 2)
		{
            if (this.delay > 0.0f)
            {
                this.delay -= Time.deltaTime;
            }

            if (this.delay <= 0.0f)
		    {
                if (ShowingCards[0].Picture.sprite != ShowingCards[1].Picture.sprite)
		        {
		            ShowingCards[0].Show(false);
		            ShowingCards[1].Show(false);
		            ShowingCards.Clear();
		            this.failCount++;
		//            FailCount.text = this.failCount.ToString();
		        }
		    }
		}

        if (this.remainingCards == 0 && State == GameState.Game)
        {
            // Victory ! Tap for new game
            State = GameState.Win;
        }
	}

    public void Reset()
    {
        foreach (var card in Cards)
        {
            Destroy(card);
        }  
    }

    public void NotifyCardShowing(Card card)
    {
        ShowingCards.Add(card);

        if (ShowingCards.Count == 2)
        {
            if (ShowingCards[0].Picture.sprite == ShowingCards[1].Picture.sprite)
            {
                this.remainingCards -= 2;

                // spawn stars
                GameObject stars = Instantiate(StarParticulePrefab) as GameObject;
                stars.transform.position = ShowingCards[0].transform.position;
                stars = Instantiate(StarParticulePrefab) as GameObject;
                stars.transform.position = ShowingCards[1].transform.position;
                ShowingCards.Clear();
                Card.ShowingCardsCount = 0;
            }
            else
            {
                this.delay = m_ShowBeforeHide;
            }
        }
    }
}
