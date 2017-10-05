using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownCrawler
{
    class Room
    {

        public Room northRoom;
        public Room eastRoom;
        public Room southRoom;
        public Room westRoom;

        public bool collapsedNorth = false;
        public bool collapsedEast = false;
        public bool collapsedSouth = false;
        public bool collapsedWest = false;

        public int enemies;
        public char roomType;
        public bool visited = false;
        public bool shortestRoute = false;
        public bool isCurrentRoom = false;
        public bool cheapestRoute = false;


        public Room(char roomType = 'N', int enemies = 0)
        {
            this.enemies = enemies;
            this.roomType = roomType;
        }

        public void drawRoom(int line, bool allVisible)
        {
            setColor(false, allVisible);

            if (visited || allVisible)
            {
                switch (line)
                {
                
                    case 1:
                        Console.Write((collapsedNorth) ? "   #   " : (northRoom == null) ? "       " : "   |   ");
                        break;
                    case 2:
                        setColor(true);
                        Console.Write(" ╔═══╗ ");
                        break;
                    case 3:
                        Console.Write(((collapsedWest) ? "#" : (westRoom == null) ? " " : "─"));

                        setColor(true);
                        Console.Write("║ " + roomFilling() + " ║");

                        Console.ResetColor();
                        setColor();
                        Console.Write(((collapsedEast) ? "#" : (eastRoom == null) ? " " : "─"));
                        break;
                    case 4:
                        setColor(true);
                        Console.Write(" ╚═══╝ ");
                        break;
                    case 5:
                        Console.Write((collapsedSouth) ? "   #   " : (southRoom == null) ? "       " : "   |   ");
                        break;
                }
            }
            else
            {
                if (line == 3) Console.Write("   .   ");
                else Console.Write("       ");
            }

            
            Console.ResetColor();

        }


        private void setColor(bool colorWhenCurrent = false, bool showHintableColors = true)
        {

            if      (colorWhenCurrent && isCurrentRoom) Console.ForegroundColor = ConsoleColor.Blue;
            else if (roomType == 'S') Console.ForegroundColor = ConsoleColor.Red;
            else if ((showHintableColors || visited) && roomType == 'E') Console.ForegroundColor = ConsoleColor.Green;
            else if ((showHintableColors || visited) && shortestRoute) Console.ForegroundColor = ConsoleColor.Yellow;
            else if ((showHintableColors || visited) && cheapestRoute) Console.ForegroundColor = ConsoleColor.Magenta;

        }


        private string roomFilling()
        {
            return ((roomType == 'N') ? enemies.ToString() : roomType.ToString());
        }


    }
}
