using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Pathfinding
{
    public int costStr = 10;
    public int costDia = 14;

    public List<Point> openList = new List<Point>();
    public List<Point> closedList = new List<Point>();

    public int brojProlaza = 1;

    public List<Point> FindPath(Point StartPoint, Point EndPoint)   //Vraća listu točaka po kojoj će objekt ići
    {
        openList.Clear();
        closedList.Clear();

        openList.Add(StartPoint);

        StartPoint.g_cost = 0;
        StartPoint.h_cost = CalculateFastestRoute(StartPoint, EndPoint);
        StartPoint.g_cost = int.MaxValue;

        //while (openList.Count > 0)
        while (brojProlaza > 0)
        {
            Point curPoint = GetLowestFcostPoint();

            //Debug.Log("prolaz: " + brojProlaza + ", end: " + EndPoint.toStr() + ", cur: " + curPoint.toStr());

            if (curPoint.toStr() == EndPoint.toStr())
            {
                return CalculatePath(curPoint); //ovo je kraj, treba još izračunati path
            }

            openList.Remove(curPoint);
            closedList.Add(curPoint);

            Terrain_Points.SetColor(curPoint, 5);

            foreach (Point nbPoint in AllNeighbourList(curPoint))
            {

                if (closedList.Contains(nbPoint)) continue;

                Terrain_Points.SetColor(nbPoint, 3);
                int pre_gcost = curPoint.g_cost + CalculateFastestRoute(curPoint, nbPoint);

                if (pre_gcost < nbPoint.g_cost)
                {
                    nbPoint.parent = curPoint;
                    nbPoint.g_cost = pre_gcost;
                    nbPoint.h_cost = CalculateFastestRoute(nbPoint, EndPoint);
                    nbPoint.SetF();

                    if (!openList.Contains(nbPoint)) openList.Add(nbPoint);
                }
            }
            brojProlaza--;
        }

        //ako dode do ovdje program znači da je prošao sve pointove
        //Debug.Log("Every point has been checked!");
        return null;
    }

    private List<Point> CalculatePath(Point EndPoint)
    {
        List<Point> pathPoints = new List<Point>();
        pathPoints.Add(EndPoint);
        Point currentPoint = EndPoint;
        while(currentPoint.parent != null)
        {
            pathPoints.Add(currentPoint.parent);
            currentPoint = currentPoint.parent;
        }
        pathPoints.Reverse();
        return pathPoints;
    }

    private int CalculateFastestRoute (Point StartPoint, Point EndPoint)
    {
        int totalX = Mathf.Abs(StartPoint.x - EndPoint.x);
        int totalY = Mathf.Abs(StartPoint.y - EndPoint.y);
        int totalD = Mathf.Abs(totalX - totalY);
        int total = Mathf.Min(totalX, totalY) * costDia + totalD * costStr;
        return total;
    }

    private Point GetLowestFcostPoint ()    //Tražimo point s najmanjim F_cost u openList listi
    {
        Point aPnt = openList[0];                                       
        for (int i = 1; i<openList.Count;i++)                
        {
            if (openList[i].GetF() < aPnt.GetF()) aPnt = openList[i];
        }
        return aPnt;
    }

    private int FindPointInScene (int x, int y) 
    {
        // vraća -1 ako point ne bi trebao niti postojati; vraća 0 ako nije korišten; vraća >0 ako već postoji u nekoj listi
        if (GameObject.Find("Point(" + x + "," + y + ")") != null)  //polje mora postojati u sceni
        {
            foreach (Point a in openList) if (a.x == x && a.y == y)     return 1;      //je li polje već upisano u openList
            foreach (Point a in closedList) if (a.x == x && a.y == y)   return 2;      //je li polje već upisano u closedList
            return 0;
        }
        else return -1;
    }

    private Point FindPointInList(int indexListe, int x, int y)
    {
        //Ako point postoji i već smo ga koristili
        if (indexListe == 1) foreach (Point a in openList) if (a.x == x && a.y == y) return a;      //je li polje već upisano u openList
        if (indexListe == 2) foreach (Point a in closedList) if (a.x == x && a.y == y) return a;    //je li polje već upisano u closedList
        return null;
    }

    private List<Point> AllNeighbourList(Point checkPoint) //vraća susjede, ako postoje u sceni
    {
        List<Point> AllNPoints = new List<Point>();

        //provjeri postojanje susjeda:
        //Ako ne postoji u sceni vozi dalje
        //Ako postoji u sceni a nismo ga koristili napravi njegov List element
        //Ako postoji u sceni i koristili smo ga onda opet koristi isti element iz postojeće liste
        int i = FindPointInScene(checkPoint.x - 1, checkPoint.y);
        if (i == 0) AllNPoints.Add(new Point(checkPoint.x - 1, checkPoint.y));
        else if (i > 0) AllNPoints.Add(FindPointInList(i, checkPoint.x - 1, checkPoint.y));

        i = FindPointInScene(checkPoint.x - 1, checkPoint.y - 1);
        if (i == 0) AllNPoints.Add(new Point(checkPoint.x - 1, checkPoint.y - 1));
        else if (i > 0) AllNPoints.Add(FindPointInList(i, checkPoint.x - 1, checkPoint.y - 1));

        i = FindPointInScene(checkPoint.x, checkPoint.y - 1);
        if (i == 0) AllNPoints.Add(new Point(checkPoint.x, checkPoint.y - 1));
        else if (i > 0) AllNPoints.Add(FindPointInList(i, checkPoint.x, checkPoint.y - 1));

        i = FindPointInScene(checkPoint.x + 1, checkPoint.y - 1);
        if (i == 0) AllNPoints.Add(new Point(checkPoint.x + 1, checkPoint.y - 1));
        else if (i > 0) AllNPoints.Add(FindPointInList(i, checkPoint.x + 1, checkPoint.y - 1));

        i = FindPointInScene(checkPoint.x + 1, checkPoint.y);
        if (i == 0) AllNPoints.Add(new Point(checkPoint.x + 1, checkPoint.y));
        else if (i > 0) AllNPoints.Add(FindPointInList(i, checkPoint.x + 1, checkPoint.y));

        i = FindPointInScene(checkPoint.x + 1, checkPoint.y + 1);
        if (i == 0) AllNPoints.Add(new Point(checkPoint.x + 1, checkPoint.y + 1));
        else if (i > 0) AllNPoints.Add(FindPointInList(i, checkPoint.x + 1, checkPoint.y + 1));

        i = FindPointInScene(checkPoint.x, checkPoint.y + 1);
        if (i == 0) AllNPoints.Add(new Point(checkPoint.x, checkPoint.y + 1));
        else if (i > 0) AllNPoints.Add(FindPointInList(i, checkPoint.x, checkPoint.y + 1));

        i = FindPointInScene(checkPoint.x - 1, checkPoint.y + 1);
        if (i == 0) AllNPoints.Add(new Point(checkPoint.x - 1, checkPoint.y + 1));
        else if (i > 0) AllNPoints.Add(FindPointInList(i, checkPoint.x - 1, checkPoint.y + 1));

        return AllNPoints;
    }
}

public class Point
{
    public int x;
    public int y;

    private int f_cost;     //g+h
    public int g_cost;      //pređeni put
    public int h_cost;      //udaljenost od krajnje točke

    public bool pathAble = true;

    public Point parent = null;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public int GetF()
    {
        return f_cost;
    }

    public void SetF ()
    {
        f_cost = g_cost + h_cost;
    }

    public string toStr ()
    {
        string newStr = "Point(" + x + "," + y + ")";
        return newStr;
    }
}
