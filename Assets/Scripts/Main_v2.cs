using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Main_v2 : MonoBehaviour
{

    enum Direction : byte
    {
        NULL=0,Up=1,Down=2,Left=3,Right=4
    }
    Direction direction = Direction.NULL;
    bool newPoint = true, checkedGrid =true;
    int[,] points = new int[4,4];
    public Text[] HudPoint = new Text[16];

    const int size = 4;

    void Start()
    {
        Refresh();
    }
    void Refresh()
    {
        for(int i=0;i<4;i++)
            for(int j=0;j<4;j++)
                points[i,j] = 0;
        direction = Direction.NULL;
        //CreateNewPoint();
        newPoint = true;
        //CreateNewPoint();

        CreateNewPoint(0,0,2);
        CreateNewPoint(0,0,2);
        CreateNewPoint(0,0,2);
        CreateNewPoint(0,0,2);
        
        CreateNewPoint(0,0,2);
        CreateNewPoint(0,0,2);
        CreateNewPoint(0,0,2);
        CreateNewPoint(0,0,2);
        
        ShowGrid();
    }

    void Update()
    {
        PressKey();
    }
    void CreateNewPoint()
    {        
        int rndPosition;
        int posX, posY;
        do
        {
            rndPosition = Random.Range(0,16);
            posX = rndPosition%4;
            posY = rndPosition/4;
        }
        while (points[posX,posY] != 0); 
        int val = Random.Range(0,11);
        points[posX,posY]  = (val<10) ? 2:4;
        newPoint=false;
    }

    void CreateNewPoint(int posX, int posY, int value) //для тестирования
    {                
        points[posX, posY]  = value;
    }
    void CheckGrid()
    {
        foreach (var item in points)        
            if (item == 0) return;   
        CheckGameOver();     
    }

    private struct Point {
        public int X;
        public int Y;

        public Point(int x, int y) {
            X = x;
            Y = y;
        }    
    }
    private Point ConvertUp(int i, int j) {
        return new Point(i,size - j - 1);
    }
    private Point ConvertDown(int i, int j) {
        return new Point(i,j);
    }
    private Point ConvertLeft(int i, int j) {
        return new Point(j, i);
    }
    private Point ConvertRight(int i, int j) {
        return new Point(size - j - 1, i);
    }

    private Point ConvertNextPointUp(Point p)=> new Point(p.X, p.Y+1);
    private Point ConvertNextPointDown(Point p)=> new Point(p.X, p.Y-1);
    private Point ConvertNextPointLeft(Point p)=> new Point(p.X-1, p.Y);
    private Point ConvertNextPointRight(Point p)=> new Point(p.X+1, p.Y);

    void SwapPoints()
    {
        System.Func<int, int, Point> convert;
        System.Func<Point, Point> convertNext;
        switch (direction) {
            case Direction.Up:
                convert = ConvertUp;   
                convertNext = ConvertNextPointUp;             
                break;
            case Direction.Down:
                convert = ConvertDown;
                convertNext = ConvertNextPointDown; 
                break;
            case Direction.Left:
                convert = ConvertLeft;
                convertNext = ConvertNextPointLeft; 
                break;
            case Direction.Right:
                convert = ConvertRight;
                convertNext = ConvertNextPointRight; 
                break;
            default:
                return;
        }
        
        
        for (int i = 0; i < size; i++)
        {
            bool sum = true;
            for (int j = 1; j < size; j++)
            {
                Point point = convert(i, j);
                Point nextP = convertNext(point);
                while ( nextP.X < size && nextP.X>=0 && nextP.Y<size && nextP.Y>=0 && points[point.X, point.Y] != 0)
                {
                    if (points[nextP.X,nextP.Y] == 0)
                        points[nextP.X,nextP.Y] = points[point.X, point.Y];
                    else if (points[point.X, point.Y] == points[nextP.X,nextP.Y] && sum)
                    {
                        points[nextP.X,nextP.Y] += points[point.X, point.Y];
                        sum = false;
                    }
                    else
                    {
                        sum = true;
                        break;
                    }
                    points[point.X, point.Y] = 0;
                    point =nextP;
                    nextP= convertNext(point);
                    newPoint = true;
                }
            }
        }   
            
        if (checkedGrid)
        {
            CheckGrid();
            if (newPoint)
                CreateNewPoint();            
        }
        
    }

    void PressKey()
    {
        direction=Direction.NULL;
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown( KeyCode.S))
            direction = Direction.Down;
            else if (Input.GetKeyDown(KeyCode.UpArrow) ||Input.GetKeyDown(KeyCode.W))
                direction = Direction.Up;
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    direction = Direction.Left;
                    else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                        direction = Direction.Right;
        if (direction!= Direction.NULL)
            SwapPoints();   
        ShowGrid();  
    }    

    void ShowGrid()
    {
        for (int x=0;x<4;x++)
            for (int y=0;y<4;y++)            
                HudPoint[y*4 + x].text = (points[x,y] != 0)? points[x,y].ToString():"";
    }

    void CheckGameOver()
    {
        int[,] tmpPoints = NewArray(points);        
        bool result =false;
        checkedGrid =false;
        for (int i=1;i<5;i++)
        {
            direction = (Direction)i;            
            SwapPoints();            
            if (!CheckEquality(tmpPoints))
            {
                RefreshPoint(tmpPoints);
                result = true;
            }
        }
        if(!result)
            Refresh();
        checkedGrid = true;        
        newPoint=false;
    }

    bool CheckEquality(int [,] tmp)
    {
        for (int i = 0 ; i<size; i++)
            for (int j = 0; j<size; j++)
                if (tmp[i,j] != points[i,j])
                    return false;
        return true;
    }

    int[,] NewArray(int[,] array)
    {
        int[,] nArray = new int[size,size];
        for (int i=0;i<size;i++)
            for(int j=0;j<size;j++)
                nArray[i,j]=array[i,j];
        return nArray;
    }

    void RefreshPoint(int[,] array)
    {
        for (int i = 0 ; i<size; i++)
            for (int j = 0; j<size; j++)
                points[i,j] = array[i,j];
    }
}
