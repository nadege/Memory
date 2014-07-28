using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
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
    [SerializeField]
    private int m_Width = 6;
    [SerializeField]
    private int m_Height = 4;

    public GameObject CardPrefab;
    public GameObject StarParticulePrefab;
    public List<Sprite> Sprites;

    public readonly List<Card> ShowingCards = new List<Card>();

    private float m_Delay;

    private int m_FailCount;
    public GUIText FailCount;

    void Start () 
    {
        List<int> spriteIndex = new List<int>();
        for (int i = 0; i < Sprites.Count; i++)
	    {
	        spriteIndex.Add(i);
	    }

        var cards = this.GenerateCards(spriteIndex);

        float widthSpaceNeeded = (m_Width - 1) * (m_CardOffset + m_CardWidth);
        float heigthSpaceNeeded = (m_Height - 1) * (m_CardOffset + m_CardWidth);

        Vector3 startingPosition = new Vector3((m_BoardWidth - widthSpaceNeeded) / 2, (m_BoardHeight - heigthSpaceNeeded) / 2);

        /*if (startingPosition.x <= 0 || startingPosition.y <= 0)
        {
           Scale down
        }*/

        Debug.Log("Starting position " + startingPosition);
        for (int i = 0; i < this.m_Width; i++)
	    {
            for (int j = 0; j < this.m_Height; j++)
            {
                Vector3 position = startingPosition +
                                   new Vector3((m_CardOffset + m_CardWidth)*i, (m_CardOffset + m_CardWidth)*j);

                int index = Random.Range(0, cards.Count - 1);
                GameObject card = cards[index];
                card.transform.position = position;
                cards.Remove(card);
            }    
	    }
	}

    private List<GameObject> GenerateCards(List<int> spriteIndex)
    {
        List<GameObject> cards = new List<GameObject>();
        for (int i = 0; i < (this.m_Width*this.m_Height)/2; i++)
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
		if (ShowingCards.Count == 2)
		{
            if (m_Delay > 0.0f)
            {
                m_Delay -= Time.deltaTime;
            }

            if (m_Delay <= 0.0f)
		    {
                if (ShowingCards[0].Picture.sprite != ShowingCards[1].Picture.sprite)
		        {
		            ShowingCards[0].Show(false);
		            ShowingCards[1].Show(false);
		            ShowingCards.Clear();
		            m_FailCount++;
		            FailCount.text = m_FailCount.ToString();
		        }
		    }
		}
	}

    public void NotifyCardShowing(Card card)
    {
        ShowingCards.Add(card);

        if (ShowingCards.Count == 2)
        {
            if (ShowingCards[0].Picture.sprite == ShowingCards[1].Picture.sprite)
            {
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
                m_Delay = m_ShowBeforeHide;
            }
        }
    }
}
