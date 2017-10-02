using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownCrawler
{
    class Dungeon
    {


        private Room currentRoom;
        private Room startRoom;
        private Room endRoom;

        private Room[,] roomMap;

        bool allVisible = true;

        public String lastMessage = "";


        public Dungeon(int width, int height)
        {
            roomMap = new Room[width, height];

            Random random = new Random();

            int xStart = random.Next(0, width);
            int yStart = random.Next(0, height);

            int xEnd = random.Next(0, width);
            int yEnd = random.Next(0, height);

            while(xEnd == xStart && yEnd == yStart)
            {
                xEnd = random.Next(0, width);
                yEnd = random.Next(0, height);
            }

            roomMap[xStart, yStart] = new Room('S');
            startRoom = roomMap[xStart, yStart];

            currentRoom = roomMap[xStart, yStart];
            currentRoom.visited = true;
            currentRoom.isCurrentRoom = true;


            roomMap[xEnd, yEnd] = new Room('E');
            endRoom = roomMap[xEnd, yEnd];



            for(int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if(roomMap[x,y] == null)
                    {
                        roomMap[x,y] = new Room('N', random.Next(0, 10));
                    }

                    if (x > 0)
                    {
                        roomMap[x, y].westRoom = roomMap[x - 1, y];
                        roomMap[x-1, y].eastRoom = roomMap[x, y];
                    }
                    if (y > 0)
                    {
                        roomMap[x, y].northRoom = roomMap[x, y - 1];
                        roomMap[x, y - 1].southRoom = roomMap[x, y];
                    }
                }
            }

        }

        public void drawDungeon()
        {

            for (int y = 0; y < roomMap.GetLength(1); y++)
            {
                int line = 1;
                while(line <= 5)
                {
                    for (int x = 0; x < roomMap.GetLength(0); x++)
                    {
                        roomMap[x, y].drawRoom(line, allVisible);
                    }
                    Console.WriteLine();
                    line++;
                }
            }

        }

        public void nextCurrentRoom(string room)
        {
            lastMessage = "";
            switch (room)
            {
                case "noord":
                    if(movableNorth())
                    {
                        currentRoom.isCurrentRoom = false;
                        currentRoom = currentRoom.northRoom;
                        currentRoom.visited = true;
                        currentRoom.isCurrentRoom = true;
                    }
                    else
                    {
                        lastMessage = "Het is niet mogelijk om noordelijk te gaan.";
                    }
                    break;
                case "oost":
                    if (movableEast())
                    {
                        currentRoom.isCurrentRoom = false;
                        currentRoom = currentRoom.eastRoom;
                        currentRoom.visited = true;
                        currentRoom.isCurrentRoom = true;
                    }
                    else
                    {
                        lastMessage = "Het is niet mogelijk om oostelijk te gaan.";
                    }
                    break;
                case "zuid":
                    if (movableSouth())
                    {
                        currentRoom.isCurrentRoom = false;
                        currentRoom = currentRoom.southRoom;
                        currentRoom.visited = true;
                        currentRoom.isCurrentRoom = true;
                    }
                    else
                    {
                        lastMessage = "Het is niet mogelijk om zuidelijk te gaan.";
                    }
                    break;
                case "west":
                    if (movableWest())
                    {
                        currentRoom.isCurrentRoom = false;
                        currentRoom = currentRoom.westRoom;
                        currentRoom.visited = true;
                        currentRoom.isCurrentRoom = true;
                    }
                    else
                    {
                        lastMessage = "Het is niet mogelijk om westelijk te gaan.";
                    }
                    break;
            }
            
        }


        public void useTalisman()
        {

            for (int y = 0; y < roomMap.GetLength(1); y++)
            {
                for (int x = 0; x < roomMap.GetLength(0); x++)
                {
                    roomMap[x, y].shortestRoute = false;
                }                
            }

            lastMessage = "De eindkamer is " + breadthFirstSearch() + " kamers weg.";
        }

        public void useHandgranaat()
        {

        }

        public void useKompas()
        {

        }



        public int breadthFirstSearch()
        {
            Queue<Room> queue = new Queue<Room>();
            Dictionary<Room, Room> visited = new Dictionary<Room, Room>();

            queue.Enqueue(currentRoom);

            while(queue.Count != 0)
            {
                Room pivot = queue.Dequeue();


                if(pivot.roomType == 'E')
                {
                    int steps = 0;
                    while(currentRoom != pivot)
                    {
                        pivot.shortestRoute = true;
                        pivot = visited[pivot];
                        steps++;
                    }

                    return steps;
                }

                if (pivot.northRoom != null && !pivot.collapsedNorth && !visited.ContainsKey(pivot.northRoom))
                {
                    visited.Add(pivot.northRoom, pivot);
                    queue.Enqueue(pivot.northRoom);
                }
                if (pivot.eastRoom != null && !pivot.collapsedEast && !visited.ContainsKey(pivot.eastRoom))
                {
                    visited.Add(pivot.eastRoom, pivot);
                    queue.Enqueue(pivot.eastRoom);
                }
                if (pivot.southRoom != null && !pivot.collapsedSouth && !visited.ContainsKey(pivot.southRoom))
                {
                    visited.Add(pivot.southRoom, pivot);
                    queue.Enqueue(pivot.southRoom);
                }
                if (pivot.westRoom != null && !pivot.collapsedWest && !visited.ContainsKey(pivot.westRoom))
                {
                    visited.Add(pivot.westRoom, pivot);
                    queue.Enqueue(pivot.westRoom);
                }
            }

            return 0;
        }



        public bool movableNorth()
        {
            return (currentRoom.northRoom != null && !currentRoom.collapsedNorth);
        }
        public bool movableEast()
        {
            return (currentRoom.eastRoom != null && !currentRoom.collapsedEast);
        }
        public bool movableSouth()
        {
            return (currentRoom.southRoom != null && !currentRoom.collapsedSouth);
        }
        public bool movableWest()
        {
            return (currentRoom.westRoom != null && !currentRoom.collapsedWest);
        }





    }
}
