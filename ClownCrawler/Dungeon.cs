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
        

        private Room[,] roomMap;

        public bool allVisible = false;

        public String lastMessage = "";


        public void generateDungeon(int width, int height)
        {

            roomMap = new Room[width, height];

            Random random = new Random();

            int xStart = random.Next(0, width);
            int yStart = random.Next(0, height);

            int xEnd = random.Next(0, width);
            int yEnd = random.Next(0, height);

            while (xEnd == xStart && yEnd == yStart)
            {
                xEnd = random.Next(0, width);
                yEnd = random.Next(0, height);
            }

            roomMap[xStart, yStart] = new Room('S');

            currentRoom = roomMap[xStart, yStart];
            currentRoom.visited = true;
            currentRoom.isCurrentRoom = true;

            roomMap[xEnd, yEnd] = new Room('E');


            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (roomMap[x, y] == null)
                    {
                        roomMap[x, y] = new Room('N', random.Next(0, 10));
                    }

                    if (x > 0)
                    {
                        roomMap[x, y].westRoom = roomMap[x - 1, y];
                        roomMap[x - 1, y].eastRoom = roomMap[x, y];
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
            clearColors();

            lastMessage = "";
            switch (room)
            {
                case "noord":
                case "n":
                    if (movableNorth())
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
                case "o":
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
                case "z":
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
                case "w":
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

            clearColors();

            lastMessage = "De eindkamer is " + breadthFirstSearch() + " kamers weg.";
        }

        public void useHandgranaat(bool collapseAll)
        {
            clearColors();

            // Get the minimum spanning tree.
            HashSet<Hall> minimumHalls = PrimsAlgorithm();

            // Generate 2 lists with unique random numbers.
            Random random = new Random();                                
            List<int> randomListX = new List<int>();
            List<int> randomListY = new List<int>();
            int randomNumber = 0;
            while(randomListX.Count < roomMap.GetLength(0))
            {
                randomNumber = random.Next(0, roomMap.GetLength(0));
                if (!randomListX.Contains(randomNumber)) randomListX.Add(randomNumber);
            }
            while (randomListY.Count < roomMap.GetLength(1))
            {
                randomNumber = random.Next(0, roomMap.GetLength(1));
                if (!randomListY.Contains(randomNumber)) randomListY.Add(randomNumber);
            }

            // Collapse some hallways, but not if it exists in the minimum spanning tree list.
            int collapsed = 0;
            int maxCollapsed = collapseAll? minimumHalls.Count+1 : random.Next(10, 16);
            for (int y = 0; y < randomListY.Count && collapsed < maxCollapsed; y++)
            {
                for (int x = 0; x < randomListX.Count && collapsed < maxCollapsed; x++)
                {
                    Room r = roomMap[randomListX[x], randomListY[y]];
                    bool rNorthMin = false;
                    bool rEastMin = false;
                    bool rSouthMin = false;
                    bool rWestMin = false;

                    foreach (Hall h in minimumHalls)
                    {
                        if (r == h.vertex1 && r.northRoom == h.vertex2) rNorthMin = true;
                        if (r.northRoom == h.vertex1 && r == h.vertex2) rNorthMin = true;

                        if (r == h.vertex1 && r.eastRoom == h.vertex2) rEastMin = true;
                        if (r.eastRoom == h.vertex1 && r == h.vertex2) rEastMin = true;

                        if (r == h.vertex1 && r.southRoom == h.vertex2) rSouthMin = true;
                        if (r.southRoom == h.vertex1 && r == h.vertex2) rSouthMin = true;

                        if (r == h.vertex1 && r.westRoom == h.vertex2) rWestMin = true;
                        if (r.westRoom == h.vertex1 && r == h.vertex2) rWestMin = true;
                    }

                    if (r.northRoom != null && !r.collapsedNorth && !rNorthMin && collapsed < maxCollapsed) {
                        r.collapsedNorth = true;
                        r.northRoom.collapsedSouth = true;
                        collapsed++;
                        randomListX.Add(randomListX[x]);
                        randomListY.Add(randomListY[y]);
                        continue;
                    }
                    if (r.eastRoom != null && !r.collapsedEast && !rEastMin && collapsed < maxCollapsed) {
                        r.collapsedEast = true;
                        r.eastRoom.collapsedWest = true;
                        collapsed++;
                        randomListX.Add(randomListX[x]);
                        randomListY.Add(randomListY[y]);
                        continue;
                    }
                    if (r.southRoom != null && !r.collapsedSouth && !rSouthMin && collapsed < maxCollapsed) {
                        r.collapsedSouth = true;
                        r.southRoom.collapsedNorth = true;
                        collapsed++;
                        randomListX.Add(randomListX[x]);
                        randomListY.Add(randomListY[y]);
                        continue;
                    }
                    if (r.westRoom != null && !r.collapsedWest && !rWestMin && collapsed < maxCollapsed) {
                        r.collapsedWest = true;
                        r.westRoom.collapsedEast = true;
                        collapsed++;
                        randomListX.Add(randomListX[x]);
                        randomListY.Add(randomListY[y]);
                        continue;
                    }
                }
            }

            lastMessage = "Aantal gangen ingestort: " + collapsed;

        }

        public void useKompas()
        {
            clearColors();

            lastMessage = "Directies naar de eindkamer: " + Dijkstra();

        }



        public int breadthFirstSearch()
        {
            Queue<Room> queue = new Queue<Room>();
            Dictionary<Room, Room> visited = new Dictionary<Room, Room>();

            queue.Enqueue(currentRoom);

            while(queue.Count != 0)
            {
                Room pivot = queue.Dequeue();
                pivot.searched = true;

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


        public HashSet<Hall> PrimsAlgorithm()
        {
            HashSet<Hall> queue = new HashSet<Hall>();
            HashSet<Hall> visited = new HashSet<Hall>();

            if (currentRoom.northRoom != null && !currentRoom.collapsedNorth) queue.Add(new Hall(currentRoom, currentRoom.northRoom, currentRoom.enemies+ currentRoom.northRoom.enemies));
            if (currentRoom.eastRoom != null && !currentRoom.collapsedEast) queue.Add(new Hall(currentRoom, currentRoom.eastRoom, currentRoom.enemies + currentRoom.eastRoom.enemies));
            if (currentRoom.southRoom != null && !currentRoom.collapsedSouth) queue.Add(new Hall(currentRoom, currentRoom.southRoom, currentRoom.enemies + currentRoom.southRoom.enemies));
            if (currentRoom.westRoom != null && !currentRoom.collapsedWest) queue.Add(new Hall(currentRoom, currentRoom.westRoom, currentRoom.enemies + currentRoom.westRoom.enemies));

            while (queue.Count != 0)
            {
                var sortedQueue = queue.OrderBy(x => x.weight).ToList();
                Hall edge = sortedQueue[0];

                queue.Remove(edge);
                
                bool edge1Visited = false;
                bool edge2Visited = false;
                foreach (Hall e in visited){
                    if(edge.vertex1 == e.vertex1 || edge.vertex1 == e.vertex2)
                    {
                        edge1Visited = true;
                    }
                    if (edge.vertex2 == e.vertex1 || edge.vertex2 == e.vertex2)
                    {
                        edge2Visited = true;
                    }
                }
                if (edge1Visited && edge2Visited) continue;

                visited.Add(edge);

                if (!edge1Visited && edge.vertex1.northRoom != null && !edge.vertex1.collapsedNorth) queue.Add(new Hall(edge.vertex1, edge.vertex1.northRoom, edge.vertex1.enemies + edge.vertex1.northRoom.enemies));
                if (!edge1Visited && edge.vertex1.eastRoom != null && !edge.vertex1.collapsedEast) queue.Add(new Hall(edge.vertex1, edge.vertex1.eastRoom, edge.vertex1.enemies + edge.vertex1.eastRoom.enemies));
                if (!edge1Visited && edge.vertex1.southRoom != null && !edge.vertex1.collapsedSouth) queue.Add(new Hall(edge.vertex1, edge.vertex1.southRoom, edge.vertex1.enemies + edge.vertex1.southRoom.enemies));
                if (!edge1Visited && edge.vertex1.westRoom != null && !edge.vertex1.collapsedWest) queue.Add(new Hall(edge.vertex1, edge.vertex1.westRoom, edge.vertex1.enemies + edge.vertex1.westRoom.enemies));

                if (!edge2Visited && edge.vertex2.northRoom != null && !edge.vertex2.collapsedNorth) queue.Add(new Hall(edge.vertex2, edge.vertex2.northRoom, edge.vertex2.enemies + edge.vertex2.northRoom.enemies));
                if (!edge2Visited && edge.vertex2.eastRoom != null && !edge.vertex2.collapsedEast) queue.Add(new Hall(edge.vertex2, edge.vertex2.eastRoom, edge.vertex2.enemies + edge.vertex2.eastRoom.enemies));
                if (!edge2Visited && edge.vertex2.southRoom != null && !edge.vertex2.collapsedSouth) queue.Add(new Hall(edge.vertex2, edge.vertex2.southRoom, edge.vertex2.enemies + edge.vertex2.southRoom.enemies));
                if (!edge2Visited && edge.vertex2.westRoom != null && !edge.vertex2.collapsedWest) queue.Add(new Hall(edge.vertex2, edge.vertex2.westRoom, edge.vertex2.enemies + edge.vertex2.westRoom.enemies));

            }

            return visited;

        }




        public string Dijkstra()
        {
            Dictionary<Room, int> queue = new Dictionary<Room, int>();
            Dictionary<Room, Room> visited = new Dictionary<Room, Room>();

            queue.Add(currentRoom, 0);

            while (queue.Count != 0)
            {
                var sortedQueue = queue.OrderBy(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                Room pivot = sortedQueue.Keys.First();
                int weight = sortedQueue.Values.First();
                queue.Remove(pivot);
                pivot.searched = true;


                if (pivot.roomType == 'E')
                {
                    String path = ""; 
                    while (currentRoom != pivot)
                    {

                        if (pivot == visited[pivot].northRoom) path = " - Noord" + path;
                        if (pivot == visited[pivot].eastRoom) path = " - Oost" + path;
                        if (pivot == visited[pivot].southRoom) path = " - Zuid" + path;
                        if (pivot == visited[pivot].westRoom) path = " - West" + path;

                        pivot.cheapestRoute = true;
                        pivot = visited[pivot];

                    }

                    if (path.Length > 0) path = path.Substring(3, path.Length-3);
                    return path;
                }

                if (pivot.northRoom != null && !pivot.collapsedNorth && !visited.ContainsKey(pivot.northRoom))
                {
                    visited.Add(pivot.northRoom, pivot);
                    queue.Add(pivot.northRoom, weight + pivot.northRoom.enemies);
                }
                if (pivot.eastRoom != null && !pivot.collapsedEast && !visited.ContainsKey(pivot.eastRoom))
                {
                    visited.Add(pivot.eastRoom, pivot);
                    queue.Add(pivot.eastRoom, weight + pivot.eastRoom.enemies);

                }
                if (pivot.southRoom != null && !pivot.collapsedSouth && !visited.ContainsKey(pivot.southRoom))
                {
                    visited.Add(pivot.southRoom, pivot);
                    queue.Add(pivot.southRoom, weight + pivot.southRoom.enemies);
                }
                if (pivot.westRoom != null && !pivot.collapsedWest && !visited.ContainsKey(pivot.westRoom))
                {
                    visited.Add(pivot.westRoom, pivot);
                    queue.Add(pivot.westRoom, weight + pivot.westRoom.enemies);
                }
            }

            return "";
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

        public void clearColors()
        {
            for (int y = 0; y < roomMap.GetLength(1); y++)
            {
                for (int x = 0; x < roomMap.GetLength(0); x++)
                {
                    roomMap[x, y].shortestRoute = false;
                    roomMap[x, y].cheapestRoute = false;
                    roomMap[x, y].searched = false;
                }
            }
        }



    }
}
