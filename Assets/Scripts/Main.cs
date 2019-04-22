using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    bool newPoint = true;
    private const string Type = "Mino";
    Mino[,] Points = new Mino[4,4];
    public Text HudError;

    void Start()
    {
        CreateNewPoint();
        CreateNewPoint();
        CreateNewPoint();
        
    }


    void Update()
    {
        PressKey();
    }

    void CreateNewPoint()
    {        
        if (!CheckGrid())
            return;
        int rndPosition;
        int posX , posY;

        do
        {
            rndPosition = Random.Range(0,16);
            posX = rndPosition/4;
            posY = rndPosition%4;
        }
        while (Points[posX,posY] != null && CheckGrid()); 

        Vector3 pos = new Vector3(posX, posY, 0);
        GameObject tmpGame = (GameObject)(Instantiate(Resources.Load("Prefabs/Point", typeof(GameObject)), pos, Quaternion.identity));
        Points[posX, posY] = tmpGame.GetComponent<Mino>(); 
    }

    bool CheckGrid()
    {
        foreach (var item in Points)        
            if (item == null) return true;
        
        return false;
    }

    void SwapPoints(Vector3 direction)
    {
        if(direction.x != 0)
        {
            if (direction.x == 1) // свайп вправо
            {
                for (int y=0; y<4;y++)
                    for (int x=2;x>=0;x--)
                    {   
                        int xx = x;
                        while (Points[xx,y]!=null && (xx+1)<4/*Points[xx,y] != Points[xx+1,y]*/)                            
                        {   
                            if (Points[xx+1,y] == null)
                            {                            
                                Points[xx+1,y] = Points[xx,y]; 
                                Points[xx,y] = null;
                                Points[xx+1,y].transform.localPosition+= direction; 
                                xx++;
                                newPoint = true;      
                            }    
                            else if (Points[xx,y] == Points[xx+1,y])
                            {
                                Points[xx+1,y] += Points[xx,y];
                                Points[xx,y] = null;
                                xx++;
                                newPoint = true;
                            }else
                                break;
                            
                        }
                                      

                            

                        //GoMove(Points[x,y], direction, x, y);
                    }
            }
            else        // свайп влево
            {
                for (int y=0; y<4;y++)
                    for (int x=1;x<=3;x++)
                    {

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

                    }
            }
            else
            {
                for (int x=0; x<4;x++)
                    for (int y=1;y<=3;y++)
                    {

                    }
            }


            
        }
        CreateNewPoint();
        newPoint = false;
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
            newPoint = true;
            SwapPoints(direction);
        }
        
        
    }

    bool CheckPosition(int posX, int posY)
    {
        return Points[posX,posY] == null;            
    }
    bool CheckPosition(out int value)
    {
        value=3;
        return true;
    }


}
