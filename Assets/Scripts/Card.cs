using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    private float TurningSpeed = 2;
    public static int ShowingCardsCount; // animating or not

    public SpriteRenderer Visual;
    public SpriteRenderer Picture;
    public SpriteRenderer Back;

    //[SerializeField]
    private bool m_MustAnimate;
    private bool m_IsAnimating;

    private SpriteRenderer m_SpriteToHide;
    private SpriteRenderer m_SpriteToShow;

    private Game m_Game;

    private bool m_Showing;

    void Start()
    {
        if (Visual == null || Back == null)
        {
            Debug.LogError("Card is missing a sprite.");
        }

        m_Game = transform.parent.GetComponent<Game>();
    }

    void Update()
    {
        if (m_MustAnimate)
        {
            // start showing/hifing sprites
            StartAnimation(this.m_Showing);
            return;
        }

        if (m_IsAnimating)
        {
            m_IsAnimating = UpdateAnimation();
        }
    }



    private void StartAnimation(bool showing)
    {
        m_IsAnimating = true;

        m_SpriteToHide = (showing ? Back : Visual);
        m_SpriteToShow = (showing ? Visual : Back);

        m_MustAnimate = false;
    }

    private bool UpdateAnimation()
    {
        if (m_SpriteToHide.transform.localScale.y <= 0.0f)
        {
            Vector3 showingScale = m_SpriteToShow.transform.localScale;
            showingScale.y = Mathf.Clamp01(showingScale.y + (Time.deltaTime * TurningSpeed));
            m_SpriteToShow.transform.localScale = showingScale;

            if (m_SpriteToShow.transform.localScale.y >= 1.0f)
            {
                if (this.m_Showing)
                {
                    m_Game.NotifyCardShowing(this);
                }
                else
                {
                    ShowingCardsCount--;
                }

                return false;
            }
        }
        else
        {
            Vector3 hidingScale = m_SpriteToHide.transform.localScale;
            hidingScale.y = Mathf.Clamp01(hidingScale.y - (Time.deltaTime * TurningSpeed));
            m_SpriteToHide.transform.localScale = hidingScale;        
        }


        return true;
    }

    public void Show(bool show)
    {
        this.m_Showing = show;
        m_MustAnimate = true;
    }

    void OnMouseUpAsButton()
    {
        if (m_IsAnimating || m_MustAnimate || this.m_Showing || ShowingCardsCount >= 2)
        {
            return;
        }
        Show(true);
        ShowingCardsCount++;
    }
}