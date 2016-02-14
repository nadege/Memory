using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GUIStyle MainButtonsStyle;
    public GUIStyle TitleStyle;
    public GUIStyle CreditStyle;
    public Game Game;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        const float labelWidth = 200;
        const float labelHeight = 70;

        GUI.Label(new Rect(Screen.width/2f - 200f, 50f, 400f, 140f), "Memory !", this.TitleStyle);
        
        if (GUI.Button(new Rect(Screen.width / 2f - labelWidth / 2f, Screen.height / 2f - (labelHeight * 2.5f) / 2f, labelWidth, labelHeight),
                "Easy", this.MainButtonsStyle))
        {
            Game.PairCount = 3;
            Game.Difficulty = Game.GameMode.Easy;
            Application.LoadLevel("Game");
        }

        if (GUI.Button(new Rect(Screen.width / 2f - labelWidth / 2f, Screen.height / 2f, labelWidth, labelHeight),
                "Medium", this.MainButtonsStyle))
        {
            Game.PairCount = 6;
            Game.Difficulty = Game.GameMode.Medium;
            Application.LoadLevel("Game");
        }

        if (GUI.Button(new Rect(Screen.width / 2f - labelWidth / 2f, Screen.height / 2f + (labelHeight * 2.5f) / 2f, labelWidth, labelHeight),
                "Hard", this.MainButtonsStyle))
        {
            Game.PairCount = 10;
            Game.Difficulty = Game.GameMode.Hard;
            Application.LoadLevel("Game");
        }

        GUI.Label(new Rect(Screen.width / 2f - 230f / 2f, Screen.height - 100f, 230f, 70f), "Made by Nadège Michel - nashella.itch.io - Icons from game-icons.net", this.CreditStyle);

    }
}
