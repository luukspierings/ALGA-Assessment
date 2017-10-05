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

            Dungeon dungeon = new Dungeon();

            bool isNumeric;
            int width;
            int height;

            while (true)
            {
                isNumeric = false;
                width = 0;
                height = 0;

                Console.Clear();

                while (!isNumeric || width <= 1)
                {
                    Console.WriteLine("Hoeveel kamers breed is de dungeon?");
                    Console.WriteLine("Het maximale binnen het scherm is 33, maar meer breedte mag.");
                    Console.Write("Alleen nummers en hoger dan 1 >> ");
                    isNumeric = int.TryParse(Console.ReadLine(), out width);
                    Console.WriteLine();
                }
                
                while (!isNumeric || height <= 1)
                {
                    Console.WriteLine("Hoeveel kamers hoog is de dungeon?");
                    Console.Write("Alleen nummers en hoger dan 1 >> ");
                    isNumeric = int.TryParse(Console.ReadLine(), out height);
                    Console.WriteLine();
                }


                dungeon.generateDungeon(width, height);

                bool reset = false;
                while (!reset)
                {
                    Console.Clear();


                    // Legenda
                    Console.WriteLine();
                    Console.WriteLine("S   = Startkamer (rood)");
                    Console.WriteLine("E   = Eindkamer (groen)");
                    Console.WriteLine("-   = Hal naar volgende kamer");
                    Console.WriteLine("#   = Ingestorte hal (niet doorloopbaar)");
                    Console.WriteLine(".   = Niet bezochte kamer");
                    Console.WriteLine("0-9 = Kamer met clowns");
                    Console.WriteLine("      Positie speler (blauw)");
                    Console.WriteLine();

                    // Kaart
                    dungeon.drawDungeon();
                    Console.WriteLine();


                    // Bericht over de vorige actie
                    Console.WriteLine(dungeon.lastMessage);
                    Console.WriteLine();


                    // Acties
                    Console.WriteLine("Mogelijke acties:");

                    if (dungeon.movableNorth()) Console.WriteLine("Noord (N)   > Naar de noordelijke kamer lopen.");
                    if (dungeon.movableEast()) Console.WriteLine("Oost  (O)   > Naar de oostelijke kamer lopen.");
                    if (dungeon.movableSouth()) Console.WriteLine("Zuid  (Z)   > Naar de zuidelijke kamer lopen.");
                    if (dungeon.movableWest()) Console.WriteLine("West  (W)   > Naar de Westelijke kamer lopen.");

                    Console.WriteLine("Talisman    > Laat de verte van de eindkamer zien.");
                    Console.WriteLine("Handgranaat > Verslaat alles in een kamer. Mogelijke kans op instortingen.");
                    Console.WriteLine("Bom         > Laat alle gangen instorten buiten het minimum pad.");
                    Console.WriteLine("Kompas      > Laat het kortste pad zien.");
                    Console.WriteLine("Cheat       > Laat heel de dungeon zien.");
                    Console.WriteLine("Reset       > Reset de dungeon.");

                    Console.WriteLine("");

                    Console.Write(">> ");
                    String input = Console.ReadLine();
                    input = input.ToLower();

                    switch (input)
                    {
                        case "noord":
                        case "n":
                        case "oost":
                        case "o":
                        case "zuid":
                        case "z":
                        case "west":
                        case "w":
                            dungeon.nextCurrentRoom(input);
                            break;
                        case "talisman":
                            dungeon.useTalisman();
                            break;
                        case "handgranaat":
                            dungeon.useHandgranaat(false);
                            break;
                        case "bom":
                            dungeon.useHandgranaat(true);
                            break;
                        case "kompas":
                            dungeon.useKompas();
                            break;
                        case "reset":
                            reset = true;
                            break;
                        case "cheat":
                            dungeon.allVisible = true;
                            break;
                        default:
                            dungeon.lastMessage = "Dit is geen mogelijke actie: " + input;
                            break;
                    }



                }


            }

            



        }
    }
}
