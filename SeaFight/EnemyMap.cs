using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace SeaFight
{
    public class EnemyMap
    {
        public int[,] myMap = new int[Form1.mapSize, Form1.mapSize];
        public int[,] enemyMap = new int[Form1.mapSize, Form1.mapSize];

        public Button[,] myButtons = new Button[Form1.mapSize, Form1.mapSize];
        public Button[,] enemyButtons = new Button[Form1.mapSize, Form1.mapSize];

        public EnemyMap(int [,] myMap, int [,] enemyMap,Button[,]myButtons, Button[,] enemyButtons)
        {
            this.myMap = myMap;
            this.enemyMap = enemyMap;
            this.myButtons = myButtons;
            this.enemyButtons = enemyButtons;
        }

        public bool IsMapEmpty(int x ,int y,int length)
        {
            bool isEmpty = true;
            for (int i = x; i < length; i++)
            {
                if (myMap[y,i]!=0)
                {
                    isEmpty = false;
                    break;
                }
            }
            return isEmpty;
        }
        public bool IsInsideMap(int x,int y)
        {
            if (x<0 || y<0 || x>=Form1.mapSize || y>=Form1.mapSize)
            {
                return false;
            }
            return true;
        }
        public int [,] PlaceShips ()
        {
            int shipSize = 4;
            int shipGlobalCount = 10;
            int shipSpecificCount = 1;
            int currentShipSpCount = 0;
            Random rand = new Random();

            int coordinateX = 0;
            int coordinateY = 0;

            while (shipGlobalCount>0)
            {
                currentShipSpCount = shipSpecificCount;
                for (int i = 0; i < shipSpecificCount; i++)
                {
                    rand.Next(1, Form1.mapSize);
                    rand.Next(1, Form1.mapSize);
                    
                    while (!IsInsideMap(coordinateX,coordinateY+shipSize-1)|| !IsMapEmpty(coordinateX, coordinateY, shipSize))
                    {
                        coordinateX = rand.Next(1, Form1.mapSize);
                        coordinateY = rand.Next(1, Form1.mapSize);
                    }

                    for (int j = coordinateX; j < coordinateX+shipSize; j++)
                    {
                        myMap[coordinateY, coordinateX] = 1;
                    }

                    shipGlobalCount--;

                    if (currentShipSpCount > 1)
                    {
                       currentShipSpCount--;
                    }
                    else
                    {
                        shipSpecificCount++;
                        shipSize--;
                    }                    
                }
            }
            return myMap;
        }
        public bool Shooting()
        {
            bool shoot = false;
            Random rand = new Random();

            int coordinateX = rand.Next(1, Form1.mapSize);
            int coordinateY = rand.Next(1, Form1.mapSize);

            while (enemyButtons[coordinateY, coordinateX].BackColor == Color.Green || enemyButtons[coordinateY, coordinateX].BackColor == Color.Black)
            {
                coordinateX = rand.Next(1, Form1.mapSize);
                coordinateY = rand.Next(1, Form1.mapSize);
            }

            if (enemyMap[coordinateY,coordinateX]!=0)
            {
                shoot = true;
                enemyMap[coordinateY, coordinateX] = 0;
                enemyButtons[coordinateY, coordinateX].BackColor = Color.Green;
                enemyButtons[coordinateY, coordinateX].Text = "X";
            }
            else
            {
                shoot = false;
                enemyButtons[coordinateY, coordinateX].BackColor = Color.Black;
            }
            if(shoot)
            {
                Shooting();
            }
            return shoot;

        }

        
    }
}
