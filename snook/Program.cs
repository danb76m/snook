using System;
using System.Threading;

namespace snook
{
    class Program
    {
        static void Main(string[] args)
        {

            new SnakeGame();

        }
    }

    class SnakeGame
    {
        public static int SIZE = 16;
        public static readonly Random random = new Random();
        private ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();

        public static readonly string GRID = "-", SNAKE = "S", FOOD = "A";

        private string[,] grid = new string[SIZE, SIZE];

        private int dirX = 1, dirY;
        private int foodX, foodY;

        private readonly SnakeQueue snake = new SnakeQueue();

        private Boolean dead;

        private long timestamp;

        public SnakeGame()
        {
            this.timestamp = Environment.TickCount;

            for (int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    grid[x, y] = GRID;
                }
            }

            placeFood();

            snake.add(new SnakeNode(4, 1));

            while (!dead)
            {
                draw();
                processKey();
                move();
                food();
                Thread.Sleep(250);
            }
        }

        private void draw()
        {
            Console.Clear();
            Console.ForegroundColor = random.Next(0, 2) == 0 ? ConsoleColor.Red : ConsoleColor.Green;

            SnakeNode current = snake.head;
            while(current != null)
            {
                grid[current.x, current.y] = SNAKE;
                current = current.next;
            }

            for(int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    Console.Write(grid[x, y]);
                }
                Console.WriteLine();
            }

            Console.WriteLine("");
            Console.WriteLine("Length: " + snake.len);
            Console.WriteLine("Time: " + ((Environment.TickCount - timestamp) / 1000) + " Seconds");
            Console.WriteLine("Direction:" + dirX + "," + dirY);
        }

        private void processKey()
        {
            if (!Console.KeyAvailable) return;
            keyInfo = Console.ReadKey(true);

            char key = keyInfo.KeyChar;
            switch(key)
            {
                case 'w':
                    dirX = -1;
                    dirY = 0;
                    break;
                case 'a':
                    dirX = 0;
                    dirY = -1;
                    break;
                case 's':
                    dirX = 1;
                    dirY = 0;
                    break;
                case 'd':
                    dirX = 0;
                    dirY = 1;
                    break;
                default:
                    break;
            }
        }

        private void move()
        {
            SnakeNode node = new SnakeNode(snake.tail.x + dirX, snake.tail.y + dirY);
            if (node.x < 0 || node.y < 0 || node.x >= SIZE || node.y >= SIZE)
            {
                Console.WriteLine("You're dead!");
                dead = true;
                return;
            }
            snake.add(node);
        }

        private void food()
        {
            if (snake.head.x == foodX && snake.head.y == foodY)
            {
                placeFood();
                return;
            }

            grid[snake.head.x, snake.head.y] = GRID;
            snake.remove();
        }

        private void placeFood()
        {
            grid[foodX, foodY] = GRID;
            foodX = random.Next(0, SIZE);
            foodY = random.Next(0, SIZE);
            grid[foodX, foodY] = FOOD;
        }
    }

    class SnakeNode
    {
        public int x, y;
        public SnakeNode next;

        public SnakeNode(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    class SnakeQueue
    {
        public SnakeNode head, tail;
        public int len;

        public void add(SnakeNode node)
        {
            if (tail != null)
            {
                tail.next = node; //change the pointer
            }
            tail = node; //update the tail of the queue
                         //if the head is null (maybe the queue is empty, make the head the tail node)
            if (head == null)
            {
                head = node;
            }

            len++;
        }

        public SnakeNode remove()
        {
            SnakeNode data = head;
            head = head.next; //remove from Queue
                              //if head is null, set tail to null too, so nothing in list
            if (head == null)
            {
                tail = null;
            }

            len--;
            return data;
        }
    }

    class Utils
    {
        public static int Max(int x, int y)
        {
            return x > y ? x : y;
        }
    }

}
