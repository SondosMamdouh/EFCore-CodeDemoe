using System;
using System.Collections.Generic;

namespace SamuraoApp.Domain
{
    public class Battle
    {
        public Battle()
        {
            SamuraiBattles = new List<SamuraiBattle>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartName { get; set; }
        public DateTime EndName { get; set; }
        public List<SamuraiBattle> SamuraiBattles { get; set; }


    }
}
