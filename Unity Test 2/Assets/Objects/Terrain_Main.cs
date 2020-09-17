using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;

public class Terrain_Main : MonoBehaviour
{
    public GameObject prefPoint;
    public int gridX = 100;
    public int gridY = 100;
    private float spaceBetween = 0.1f;
    
    public Color[] allColors = new Color[1];

    void Start()
    {
        spaceBetween = 10.0f / gridX;
        Camera.main.transform.position = new Vector3(spaceBetween * gridX / 2, spaceBetween * (gridY-1) / 2, -10.0f);
        for (int i = 0; i < gridX * gridY; i++) CreatePoint(i);
        foreach (Color oneColor in allColors) Terrain_Points.allColorsList.Add(oneColor);
    }

    void CreatePoint(int index)
    {
        int posX = index % gridX;
        int posY = Mathf.FloorToInt(index / gridY);
        GameObject newPoint = Instantiate(prefPoint, new Vector2(posX * spaceBetween, posY * spaceBetween), quaternion.identity) as GameObject;
        newPoint.name = "Point(" + posX + "," + posY + ")";
        newPoint.transform.localScale = Vector3.one * spaceBetween / 0.1f;
    }
}

public static class Terrain_Points      //koristi za postavljanje boje na point u sceni
{
    public static List<Color> allColorsList = new List<Color>();

    public static void SetColor(Point thisPoint, int colorID)
    {
        Color colorSwitch = Terrain_Points.allColorsList[colorID];

        GameObject.Find(thisPoint.toStr()).transform.Find("Sprite").GetComponent<SpriteRenderer>().color = colorSwitch;
    }

    public static void SetColor(GameObject thisPoint, int colorID)
    {
        Color colorSwitch = Terrain_Points.allColorsList[colorID];

        thisPoint.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = colorSwitch;
    }

    public static void ResetColorsInScene() //osim starting & ending pointa
    {
        Player playerScr = GameObject.Find("GameManager").GetComponent<Player>();
        GameObject[] allPoints = GameObject.FindGameObjectsWithTag("Point");
        foreach (GameObject onePoint in allPoints)
        {
            if (onePoint == playerScr.StartPoint) Terrain_Points.SetColor(onePoint, 1);
            else if (onePoint == playerScr.EndPoint) Terrain_Points.SetColor(onePoint, 2);
            else Terrain_Points.SetColor(onePoint, 0);
        }
    }
}
