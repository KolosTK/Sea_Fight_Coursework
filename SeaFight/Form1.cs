using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SeaFight
{
    public partial class Form1 : Form
    {
        public const int mapSize = 10;
        public const int cellSize = 30;
        public const int displacemant = 500;

        public int[,] myMap = new int[mapSize, mapSize];
        public int[,] enemyMap = new int[mapSize, mapSize];


        public Button[,] myButtons = new Button[mapSize,mapSize];
        public Button[,] enemyButtons = new Button[mapSize, mapSize];


        public string latters = "ABCDEFGHIJ";
        public bool isGameStarts = false;

        public EnemyMap enemy;


        public Form1()
        {
            InitializeComponent();
            Init();
        }
        public void Init ()
        {
            InitMap();
            enemy = new EnemyMap(enemyMap, myMap, enemyButtons, myButtons);
            enemyMap = enemy.PlaceShips();
        }

        public void PlayerShoot(object sender, EventArgs e)
        {
            Button pressedButton = sender as Button;
                bool playerTurn = Shooting(enemyMap, pressedButton);
                if (!playerTurn)
                {
                    enemy.Shooting();
                }
        }
        public void InitMap ()
        {

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    
                    myMap[i, j] = 0;

                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    button.BackColor = Color.White;
                    if (i==0||j==0)
                    {
                        button.BackColor = Color.AntiqueWhite;
                        if(i == 0 && j != 0)
                            button.Text = latters[j-1].ToString();

                        if (j == 0 && i != 0)
                            button.Text = i.ToString();
                    }
                    else 
                        button.Click += new EventHandler(PlaceShips);
                    myButtons[i, j] = button;
                    this.Controls.Add(button);
                }
            }
            
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    enemyMap[i, j] = 0;

                    Button button = new Button();
                    button.Location = new Point(displacemant + j * cellSize, i * cellSize);
                    button.Size = new Size(cellSize, cellSize);
                    if (i == 0 || j == 0)
                    {
                        button.BackColor = Color.AntiqueWhite;
                        if (i == 0 && j != 0)
                            button.Text = latters[j - 1].ToString();

                        if (j == 0 && i != 0)
                            button.Text = i.ToString();
                    }
                    else
                        button.Click += new EventHandler(Attack);
                    enemyButtons[i, j] = button;
                    this.Controls.Add(button);
                }
            }

            Label gameBeginning = new Label();
            gameBeginning.Text = "Place your ships";
            gameBeginning.Location = new Point(cellSize*3,cellSize*11);
            this.Controls.Add(gameBeginning);

            Button atackButton = new Button();
            atackButton.Location = new Point(cellSize * 12, cellSize * 11);
            atackButton.Text = "Start fight";
            atackButton.Click += new EventHandler(StartGame);
            this.Controls.Add(atackButton);
        }
        public void StartGame(object sender, EventArgs e)
        {
                isGameStarts = true;
        }

        public void PlaceShips(object sender, EventArgs e)
        {
            if (!isGameStarts)
            {
                Button currentButton = sender as Button;
                if (myMap[currentButton.Location.Y / cellSize, currentButton.Location.X / cellSize] == 0)
                {
                    currentButton.BackColor = Color.CadetBlue;
                    myMap[currentButton.Location.Y / cellSize, currentButton.Location.X / cellSize] = 1;
                }
                else
                {
                    currentButton.BackColor = Color.White;
                    myMap[currentButton.Location.Y / cellSize, currentButton.Location.X / cellSize] = 0;
                }
            }
            
        }

        public bool Shooting (int[,] map,Button currentButton)
        {
            bool shoot = false;
            if (isGameStarts)
            {
                
                if (map[currentButton.Location.Y / cellSize, (currentButton.Location.X - displacemant) / cellSize] == 1)
                {
                    shoot = true;
                    currentButton.BackColor = Color.Red;
                    currentButton.Text = "X";
                }
                else
                {
                    currentButton.BackColor = Color.Black;
                }
            }
            return shoot;
        }

        public void Attack(object sender, EventArgs e)
        {
            Button currentButton = sender as Button;
            bool isAttackFailed = Shooting(enemyMap, currentButton);
            if (!isAttackFailed)
                enemy.Shooting();
            if (ShipsChecking())
            {
                this.Controls.Clear();
                Init();
            }
        }

        public bool ShipsChecking()
        {
            bool isEnemyMapEmpty = true;
            bool isMyMapEmpty = true;
            for (int i = 1; i < mapSize; i++)
            {
                for (int j = 2; j < mapSize; j++)
                {
                    if (myMap[i, j] != 0)
                        isMyMapEmpty = false;
                    if (enemyMap[i, j] != 0)
                        isEnemyMapEmpty = false;
                }
            }
            if (isEnemyMapEmpty || isMyMapEmpty)
            {
                return false;
            }
            else
                return true;
        }

    }
}
