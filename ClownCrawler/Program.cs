using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            

            bool isNumeric = true;
            int width = 33;
            int height = 10;

            while (!isNumeric || width <= 1)
            {
                Console.WriteLine("Hoeveel kamers breed is de dungeon?");
                Console.WriteLine("Het maximale binnen het scherm is 33, maar meer breedte mag.");
                Console.Write("Alleen nummers en hoger dan 2 >> ");
                isNumeric = int.TryParse(Console.ReadLine(), out width);
                Console.WriteLine();
            }

            isNumeric = true;
            while (!isNumeric || height <= 1)
            {
                Console.WriteLine("Hoeveel kamers hoog is de dungeon?");
                Console.Write("Alleen nummers en hoger dan 2 >> ");
                isNumeric = int.TryParse(Console.ReadLine(), out height);
                Console.WriteLine();
            }


            Dungeon dungeon = new Dungeon(width, height);


            while (true)
            {
                Console.Clear();


                // Legenda


                // Kaart
                dungeon.drawDungeon();
                Console.WriteLine();


                // Info over de kamer


                // Bericht over de vorige actie
                Console.WriteLine(dungeon.lastMessage);
                Console.WriteLine();


                // Acties
                Console.WriteLine("Mogelijke acties:");

                if(dungeon.movableNorth())  Console.WriteLine("Noord       > Naar de noordelijke kamer lopen.");
                if(dungeon.movableEast())   Console.WriteLine("Oost        > Naar de oostelijke kamer lopen.");
                if(dungeon.movableSouth())  Console.WriteLine("Zuid        > Naar de zuidelijke kamer lopen.");
                if(dungeon.movableWest())   Console.WriteLine("West        > Naar de Westelijke kamer lopen.");

                Console.WriteLine("Talisman    > Laat de verte van de eindkamer zien.");
                Console.WriteLine("Handgranaat > Verslaat alles in een kamer. Mogelijke kans op instortingen.");
                Console.WriteLine("Kompas      > Laat het kortste pad zien");

                Console.WriteLine("");

                Console.Write(">> ");
                String input = Console.ReadLine();
                input = input.ToLower();

                switch (input)
                {
                    case "noord":
                    case "oost":
                    case "zuid":
                    case "west":
                        dungeon.nextCurrentRoom(input);
                        break;
                    case "talisman":
                        dungeon.useTalisman();
                        break;
                    case "handgranaat":
                        dungeon.useHandgranaat();
                        break;
                    case "kompas":
                        dungeon.useKompas();
                        break;
                    case "reset":
                        dungeon = new Dungeon(width,height);
                        break;
                    default:
                        dungeon.lastMessage = "Dit is geen mogelijke actie: " + input;
                        break;
                }



            }



        }
    }
}
