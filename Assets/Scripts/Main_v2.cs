using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Main_v2 : MonoBehaviour
{
    bool newPoint = true;
    int[,] points = new int[4,4];
    public Text[] HudPoint = new Text[16];
    void Start()
    {
        for(int i=0;i<4;i++)
            for(int j=0;j<4;j++)
                points[i,j]=0;
        CreateNewPoint(0,0,8);
        CreateNewPoint(1,0,4);
        CreateNewPoint(2,0,2);
        CreateNewPoint(3,0,2);

        CreateNewPoint(0,1,2);
        CreateNewPoint(1,1,2);
        CreateNewPoint(2,1,4);
        CreateNewPoint(3,1,8);
        
        ShowGrid();
    }
    void Update()
    {
        PressKey();
        ShowGrid();
    }
    void CreateNewPoint()
    {        
        if (!CheckGrid() || !newPoint)
            return;
        int rndPosition;
        int posX, posY;

        do
        {
            rndPosition = Random.Range(0,16);
            posX = rndPosition%4;
            posY = rndPosition/4;
        }
        while (points[posX,posY] != 0 && CheckGrid()); 
        int val = Random.Range(0,11);
        points[posX,posY]  = (val<10)? 2:4;
        newPoint=false;
    }

    void CreateNewPoint(int posX, int posY, int value) //для тестирования
    {                
        points[posX,posY]  = value;
    }
    bool CheckGrid()
    {
        foreach (var item in points)        
            if (item == 0) return true;        
        return false;
    }
    void SwapPoints(Vector3 direction)
    {
        bool sum = true , swap = false;
        if(direction.x != 0)
        {
            if (direction.x == 1) // свайп вправо
            {
                for (int y=0; y<4;y++)
                    for (int x=2;x>=0;x--)
                    {   
                        int xx = x;
                        if (swap)
                            sum=true;
                        if(!sum)
                            swap=true;
                        while ((xx+1)<4 && points[xx,y]!=0)                            
                        {                               
                            if (points[xx+1,y] == 0)    
                            {                                                    
                                points[xx+1,y] = points[xx,y];  
                            }                            
                            else if (points[xx,y] == points[xx+1,y] && sum)   
                            {                         
                                points[xx+1,y] += points[xx,y]; 
                                sum=false;
                                swap = false;
                            }
                            else
                                break;
                            points[xx,y] = 0;
                            xx++;
                            newPoint = true;
                        }
                    }
            }
            else        // свайп влево
            {
                for (int y=0; y<4;y++)
                    for (int x=1;x<=3;x++)
                    {
                        int xx = x;
                        if (swap)
                            sum=true;
                        if(!sum)
                            swap=true;
                        while ((xx-1)>=0 && points[xx,y]!=0 )                            
                        {                               
                            if (points[xx-1,y] == 0)                                                        
                                points[xx-1,y] = points[xx,y];                                
                            else if (points[xx,y] == points[xx-1,y] && sum)   
                            {                         
                                points[xx-1,y] += points[xx,y]; 
                                sum=false;
                                swap = false;
                            }
                            else
                                break;
                            points[xx,y] = 0;
                            xx--;
                            newPoint = true;
                        }
                    }
            }
        }
        else if(direction.y !=0) 
        {
            if (direction.y == 1)
            {
                for (int x=0; x<4;x++)
                    for (int y=2;y>=0;y--)
                    {
                        int yy = y;
                        if (swap)
                            sum=true;
                        if(!sum)
                            swap=true;
                        while (points[x,yy]!=0 && (yy+1)<4)                            
                        {                               
                            if (points[x,yy+1] == 0)                                                        
                                points[x,yy+1] = points[x,yy];                                
                            else if (points[x,yy] == points[x,yy+1] && sum)   
                            {                         
                                points[x,yy+1] += points[x,yy]; 
                                sum=false;
                                swap = false;
                            }
                            else
                                break;
                            points[x,yy] = 0;
                            yy++;
                            newPoint = true;
                        }
                    }
            }
            else
            {
                for (int x=0; x<4;x++)
                    for (int y=1;y<=3;y++)
                    {
                        int yy = y;
                        if (swap)
                            sum=true;
                        if(!sum)
                            swap=true;
                        while (points[x,yy]!=0 && (yy-1)>=0)                            
                        {                               
                            if (points[x,yy-1] == 0)                                                        
                                points[x,yy-1] = points[x,yy];                                
                            else if (points[x,yy] == points[x,yy-1] && sum)   
                            {                         
                                points[x,yy-1] += points[x,yy]; 
                                sum=false;
                                swap = false;
                            }
                            else
                                break;
                            points[x,yy] = 0;
                            yy--;
                            newPoint = true;
                        }
                    }
            }
        }
        CreateNewPoint();
        newPoint = true;
    }
    void PressKey()
    {
        Vector3 direction = new Vector3(0,0,0);
        if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown( KeyCode.S))
            direction = Vector3.down;
            else if (Input.GetKeyDown(KeyCode.UpArrow) ||Input.GetKeyDown(KeyCode.W))
                direction = Vector3.up;
                else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
                    direction = Vector3.left;
                    else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
                        direction = Vector3.right;
        if (direction.x != 0 || direction.y!=0)
        {
            //newPoint = true;
            SwapPoints(direction);
        }
    }
    bool CheckPosition(int posX, int posY)
    {
        return points[posX,posY] == 0;            
    }
    bool CheckPosition(out int value)
    {
        value=3;
        return true;
    }

    void ShowGrid()
    {
        for (int x=0;x<4;x++)
            for (int y=0;y<4;y++)
            {
                if(points[x,y] !=0)
                {
                    HudPoint[y*4 + x].text = points[x,y].ToString();
                }
                else
                    HudPoint[y*4 + x].text="";
            }
    }

}
