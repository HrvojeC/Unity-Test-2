using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    //PLAYER klikom (LMB) na neko polje ubacuje to polje u ENDPOINT.
    //ako je ENDPOINT već postojao, prošli sadržaj ubacuje u STARTPOINT, a novi klik je novi ENDPOINT.
    //Klikom (RMB) uništava polje i time stvara prepreku
    //Pritiskom tipke (SPACE) odrađuje se jedan korak više pathfindinga

    public GameObject StartPoint;
    public GameObject EndPoint;

    private Pathfinding pathfinding = new Pathfinding();

    public int brojProlaza = 1;

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Point")
            {
                if (EndPoint != null)
                {
                    StartPoint = EndPoint;
                }
                EndPoint = hit.collider.gameObject;
            }
            brojProlaza = 1;
            Terrain_Points.ResetColorsInScene();
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null && hit.collider.tag == "Point")
            {
                Destroy(hit.collider.gameObject);
            }
            brojProlaza = 1;
            Terrain_Points.ResetColorsInScene();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (EndPoint != null && StartPoint != null)
            {
                Point newStartPoint = NameToPoint(StartPoint.name);
                Point newEndPoint = NameToPoint(EndPoint.name);
                pathfinding.brojProlaza = brojProlaza;
                List<Point> newPath = pathfinding.FindPath(newStartPoint, newEndPoint);
                if (newPath != null)
                {
                    brojProlaza = 1;
                    Terrain_Points.ResetColorsInScene();
                    foreach (Point pathPoint in newPath) Terrain_Points.SetColor(pathPoint, 4);
                    Terrain_Points.SetColor(newPath[0], 1);
                    Terrain_Points.SetColor(newPath[newPath.Count-1], 2);
                }
                else brojProlaza++;
            }
        }
    }

    private Point NameToPoint (string name)
    {
        string[] split1 = name.Split(new char[] { '(', ',', ')' });
        int x = 0;
        int y = 0;
        if (int.TryParse(split1[1], out x) && int.TryParse(split1[2], out y))
        {
            Point newPoint = new Point(x, y);
            return newPoint;
        }
        return null;
    }
}
