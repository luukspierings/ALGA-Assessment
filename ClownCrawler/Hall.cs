using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClownCrawler
{
    struct Hall
    {

        public Room vertex1;
        public Room vertex2;
        public int weight;

        public Hall(Room vertex1, Room vertex2, int weight)
        {
            this.vertex1 = vertex1;
            this.vertex2 = vertex2;
            this.weight = weight;
        }

    }
}
