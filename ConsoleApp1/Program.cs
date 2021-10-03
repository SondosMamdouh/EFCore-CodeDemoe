using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraoApp.Domain;
using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        private static SamuraiContext context = new SamuraiContext();
        static void Main(string[] args)
        {
            //context.Database.EnsureCreated();
            //GetSamurais("Before Add:");
            //QueryAndUpdateBattle_DisConnected();
            //InsertBattle();
            //AddSamurai();
            //InsertMultipleSamurais();
            //InsertVariousTypes();
            //QueryFilters();
            //AddQouteToExistingSamuraiNotTracked_Easy(2);
            //AddNewasamuraiwithHourse();
            //ReplaceHourse();
            //GetSamurais("After Add:");
            ////
            //Console.Write("Press any Key...");
            GetSamuraiwithBattles();
            QueryUsingSql();
            Console.ReadKey();
        }

        private static void QueryFilters()
        {
            context.Samurais.FirstOrDefault(s=>s.Name=="sondos");
        }

        private static void InsertVariousTypes()
        {
            var samurai = new Samurai { Name = "haha" };
            var clan = new Clan { ClanName = "Imperial Clan" };
            context.AddRange(samurai, clan);
            context.SaveChanges();
        }

        private static void InsertMultipleSamurais()
        {
            var samurai = new Samurai { Name = "marwa" };
            var samurai2 = new Samurai { Name = "sondos" };
            var samurai3 = new Samurai { Name = "omar" };
            var samurai4 = new Samurai { Name = "abdelrhamna" };
            context.Samurais.AddRange(samurai, samurai2, samurai3, samurai4);
            context.SaveChanges();
        }

        private static void AddSamurai()
        {
            var samurai = new Samurai { Name = "jon" };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void GetSamurais(string text)
        {
            var samurais = context.Samurais.ToList();
            Console.WriteLine($"{text}: Samurai count is {samurais.Count}");
            foreach (var samuria in samurais)
            {
                Console.WriteLine(samuria.Name);
            }
        }
        private static void InsertBattle()
        {
            context.Battles.Add(new Battle
            {
                Name = "Battle of Okehazama",
                EndName = new DateTime(1860, 06, 30),
                StartName=new DateTime(1860,10,30)
            }) ;
            context.SaveChanges();
        }
        private static void QueryAndUpdateBattle_DisConnected()
        {
            var battle = context.Battles.AsNoTracking().FirstOrDefault();
            battle.EndName = new DateTime(1560, 12, 30);
            using(var newContext=new SamuraiContext())
            {
                newContext.Battles.Update(battle);
                newContext.SaveChanges();
            }
        }

        private static void AddQouteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote { 
            Text="new qoute"
            });
            using(var newContext=new SamuraiContext())
            {
                newContext.Samurais.Update(samurai);
                newContext.SaveChanges();
            }
        }
        private static void AddQouteToExistingSamuraiNotTracked_Easy(int samuraiId)
        {
            var qoute=new Quote
            {
                Text = "new qoute",
                SamuraiId = samuraiId
            };
            using (var newContext = new SamuraiContext())
            {
                newContext.Quotes.Add(qoute);
                newContext.SaveChanges();
            }
        }

        private static void EgarLoadSamriewithQoutes()
        {
            var samuraiWithQoutes = context.Samurais.Include(s=>s.Quotes).ToList();
        }

        private static void ProjectSomeProprties()
        {
            var someProprties = context.Samurais.Select(s => new { s.Id, s.Name }).ToList();
        }
        private static void ExplicitLoad()
        {
            var samurai = context.Samurais.FirstOrDefault(s => s.Name.Contains("Julie"));
            context.Entry(samurai).Collection(s => s.Quotes).Load();
            context.Entry(samurai).Reference(s => s.Horse).Load();
        }

        private static void FilteringWithRelatedDate()
        {
            var samurai = context.Samurais.Where(s => s.Quotes.Any(q => q.Text.Contains("happy"))).ToList();
        }

        private static void ModifyingrelatedDatawhenTracked()
        {
            var smaurai = context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);
            smaurai.Quotes[0].Text = "Did you hear that?";
            context.SaveChanges();
        }

        private static void ModifyingrelatedDatawhenNotTracked()
        {
            var samurai = context.Samurais.Include(s => s.Quotes).FirstOrDefault(s => s.Id == 2);
            var qoute = samurai.Quotes[0];
            qoute.Text += "Did you hear that again ?";
            using(var newContext=new SamuraiContext())
            {
                newContext.Quotes.Update(qoute);
                newContext.SaveChanges();
            }
        }

        private static void GetSamuraiwithBattles()
        {
            var join = context.Samurais.Include(s => s.SamuraiBattles)
                .ThenInclude(b => b.Battle)
            .FirstOrDefault(s=>s.Id==2);


            var samuriewithbattle_cleaner = context.Samurais.Where(s => s.Id == 2)
                .Select(s => new
                {
                    samurai = s,
                    battle = s.SamuraiBattles.Select(b => b.Battle)
                }).FirstOrDefault();
        }

        private static void AddNewasamuraiwithHourse()
        {
            var samurai = new Samurai { Name = "alen" };
            samurai.Horse = new Horse { Name = "7osan" };
            context.Samurais.Add(samurai);
            context.SaveChanges();
        }

        private static void ReplaceHourse()
        {
            var samurai = context.Samurais.Include(s => s.Horse).FirstOrDefault(samurai=>samurai.Id==8);
            samurai.Horse = new Horse { Name = "balck devil" };
            context.SaveChanges();
        }


        private static void QuerySamuraiBattleStats()
        {
            var stats = context.SamuraiBattleStats.ToList();
        }

        private static void QueryUsingSql()
        {
            var samurais = context.Samurais.FromSqlRaw("Select * from Samurais").ToList();
        }
    }
}
