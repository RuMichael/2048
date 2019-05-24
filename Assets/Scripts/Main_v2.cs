using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Main_v2 : MonoBehaviour
{
    enum Direction : byte
    {
        NULL = 0, Up = 1, Down=2,Left=3,Right=4
    }
    Direction direction = Direction.NULL;
    bool newPoint = true, checkedGrid =true;
    int[,] points = new int[4, 4];
    public Text[] HudPoint = new Text[16];
    const int size = 4;

    void Start()
    {
        Refresh();
    }
    void Refresh()
    {
        for(int i = 0; i < 4; i++)
            for(int j = 0; j < 4; j++)
                points[i, j] = 0;
        direction = Direction.NULL;
        CreateNewPoint();
        newPoint = true;
        CreateNewPoint();        
        ShowGrid();
    }
    void Update()
    {
        #if UNITY_EDITOR
            PressKey();
        #elif UNITY_ANDROID
            PressKeyAndroid();
        #endif

        
    }
    void CreateNewPoint()
    {        
        int rndPosition;
        int posX, posY;
        do
        {
            rndPosition = Random.Range(0, 16);
            posX = rndPosition % 4;
            posY = rndPosition / 4;
        }
        while (points[posX, posY] != 0); 
        int val = Random.Range(0, 11);
        points[posX, posY]  = (val < 10) ? 2 : 4;
        newPoint = false;
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
    private Point ConvertUp(int i, int j) => new Point(i, size - j - 1);
    private Point ConvertDown(int i, int j) => new Point(i, j);
    private Point ConvertLeft(int i, int j) => new Point(j, i);
    private Point ConvertRight(int i, int j) => new Point(size - j - 1, i);
    private Point ConvertNextPointUp(Point p) => new Point(p.X, p.Y + 1);
    private Point ConvertNextPointDown(Point p) => new Point(p.X, p.Y - 1);
    private Point ConvertNextPointLeft(Point p) => new Point(p.X - 1, p.Y);
    private Point ConvertNextPointRight(Point p) => new Point(p.X + 1, p.Y);

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
                Point currentP = convert(i, j);
                Point nextP = convertNext(currentP);
                while (nextP.X < size && nextP.X >= 0 && nextP.Y < size && nextP.Y >= 0 && points[currentP.X, currentP.Y] != 0)
                {
                    if (points[nextP.X, nextP.Y] == 0)
                        points[nextP.X, nextP.Y] = points[currentP.X, currentP.Y];
                    else if (points[currentP.X, currentP.Y] == points[nextP.X, nextP.Y] && sum)
                    {
                        points[nextP.X, nextP.Y] += points[currentP.X, currentP.Y];
                        sum = false;
                    }
                    else
                    {
                        sum = true;
                        break;
                    }
                    points[currentP.X, currentP.Y] = 0;
                    currentP = nextP;
                    nextP = convertNext(currentP);
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
        direction = Direction.NULL;
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown( KeyCode.S))
            direction = Direction.Down;
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
                direction = Direction.Up;
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    direction = Direction.Left;
                    else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                        direction = Direction.Right;
        if (direction != Direction.NULL)
            SwapPoints();   
        ShowGrid();  
    }    

    List<Vector2> touchPositions = new List<Vector2>();
    Vector2 began, ended;
    void PressKeyAndroid()
    {
        //touchPositions = new List<Vector2>();
        direction = Direction.NULL;
        if(Input.touchCount > 0)
        {
            Touch myTouch = Input.GetTouch(0);            
            if (myTouch.phase == TouchPhase.Moved)            
                touchPositions.Add(myTouch.position);
            if (myTouch.phase == TouchPhase.Ended)
            {
                began = touchPositions[0];
                ended = touchPositions[touchPositions.Count-1];
                if (Mathf.Abs(ended.x - began.x) > Mathf.Abs(ended.y - began.y))
                    direction = (began.x > ended.x) ? Direction.Left : Direction.Right; 
                    //if (began.x > ended.x) direction = Direction.Left;
                    //else direction = Direction.Right;
                else
                    direction = (began.y > ended.y) ? Direction.Down : Direction.Up;
                    //if (began.y > ended.y) direction = Direction.Down;
                    //else direction = Direction.Up;
                touchPositions.Clear();
                SwapPoints();   
                ShowGrid();  
            }            
        }            
    }
    
    
    /* 
    private Vector3 fp, lp;   //Последняя позиция касания
    private float dragDistance;  //Минимальная дистанция для определения свайпа
    //private List<Vector3> touchPositions = new List<Vector3>(); //Храним все позиции касания в списке
    void Updatesd()
    {
        
        foreach (Touch touch in Input.touches)  //используем цикл для отслеживания больше одного свайпа
        { //должны быть закоментированы, если вы используете списки 
        //if (touch.phase == TouchPhase.Began) //проверяем первое касание
        //{
        //    fp = touch.position;
        //    lp = touch.position;
        //}
 
            if (touch.phase == TouchPhase.Moved) //добавляем касания в список, как только они определены        
                touchPositions.Add(touch.position);
        
 
            if (touch.phase == TouchPhase.Ended) //проверяем, если палец убирается с экрана
            {
                //lp = touch.position;  //последняя позиция касания. закоментируйте если используете списки
                fp =  touchPositions[0]; //получаем первую позицию касания из списка касаний
                lp =  touchPositions[touchPositions.Count-1]; //позиция последнего касания
 
                //проверяем дистанцию перемещения больше чем 20% высоты экрана
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//это перемещение
                    //проверяем, перемещение было вертикальным или горизонтальным 
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //Если горизонтальное движение больше, чем вертикальное движение ...
                        if ((lp.x>fp.x))  //Если движение было вправо
                        {   //Свайп вправо
                            Debug.Log("Right Swipe");
                        }
                        else
                        {   //Свайп влево
                            Debug.Log("Left Swipe"); 
                        }
                    }
                    else
                    {   //Если вертикальное движение больше, чнм горизонтальное движение
                        if (lp.y>fp.y)  //Если движение вверх
                        {   //Свайп вверх
                            Debug.Log("Up Swipe"); 
                        }
                        else
                        {   //Свайп вниз
                            Debug.Log("Down Swipe");
                        }
                    }   
                } 
            }
            else
            {   //Это ответвление, как расстояние перемещения составляет менее 20% от высоты экрана
 
            }
        }
    } */

    void ShowGrid()
    {
        for (int x = 0; x < 4; x++)
            for (int y = 0; y < 4; y++)            
                HudPoint[y * 4 + x].text = (points[x, y] != 0)? points[x, y].ToString() : "";
    }

    void CheckGameOver()
    {
        int[,] tmpPoints = NewArray(points);        
        bool result = false;
        checkedGrid = false;
        for (int i = 1; i < 5; i++)
        {
            direction = (Direction) i;            
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
        newPoint = false;
    }

    bool CheckEquality(int [,] tmp)
    {
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                if (tmp[i, j] != points[i, j])
                    return false;
        return true;
    }
    int[,] NewArray(int[,] array)
    {
        int[,] nArray = new int[size, size];
        for (int i = 0; i < size; i++)
            for(int j = 0; j < size; j++)
                nArray[i, j]=array[i, j];
        return nArray;
    }
    void RefreshPoint(int[,] array)
    {
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                points[i, j] = array[i, j];
    }
}
