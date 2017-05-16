using UnityEngine;
using System.Collections.Generic;

public static class RandomColor {
    private static List<Color> playerColors;


    private static void SetupPlayerColors()
    {
        playerColors = new List<Color>();

        playerColors.Add(new Color(255, 0, 0));
        playerColors.Add(new Color(255, 255, 0));
        playerColors.Add(new Color(255, 0, 255));

        playerColors.Add(new Color(0, 255, 0));
        playerColors.Add(new Color(0, 255, 255));

        playerColors.Add(new Color(0, 0, 255));
        playerColors.Add(new Color(255, 0, 255));
        playerColors.Add(new Color(0, 255, 255));
        
    }

    public static Color GetRandPlayerColor()
    {
        if(playerColors == null)
        {
            SetupPlayerColors();
        }

        int randColor = Random.Range(0, playerColors.Count - 1);

        Color c = playerColors[randColor];
        playerColors.RemoveAt(randColor);

        return c;
    }



}
