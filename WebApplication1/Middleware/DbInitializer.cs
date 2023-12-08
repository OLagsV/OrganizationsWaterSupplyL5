using System;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Drawing.Text;
using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Models;
namespace OrganizationsWaterSupplyL4.Middleware
{
    public class DbInitializer
    {
        public static void Initialize(OrganizationsWaterSupplyContext db)
        {
            db.Database.EnsureCreated();


            int countermodel_num = 50;
            int organization_num = 50;
            int counters_num = 100;
            int rates_num = 50;
            int models_num = 20;
            Random rand = new Random(1);
            string[] org_names = { "ГомСельМаш", "Брилево", "Сухого", "Евроопт", "Кари", "Platoon" };
            string[] ownership_types = { "Индивидуальный предприниматель", "Унитарное предприятие", "ОАО", "ООО", "ОДО" };
            string[] countries = { "Беларусь", "Германия", "Польша", "Япония", "Франция", "США" };
            string[] towns = { "Гомель", "Минск", "Дрезден", "Токио", "Париж", "Вашингтон", "Новиград" };
            string[] streets = { "Чечерская", "Фадеева", "Яблочная", "Пенязькова", "Советская" };
            string[] first_names = {"Владимир", "Михаил" , "Григорий" , "Анжела" , "Виктория" , "Олег" , "Дмитрий" , "Крубер"};
            string[] last_names = { "Петрик", "Никитин", "Гордеев", "Иващенко", "Бобров", "Гордеев", "Богданов" };
            string[] model_names = { "MK II", "Стандартный", "Расширенный", "MV I", "KD-009L" };
            string[] manufacturers = { "ГомельАква", "МосВодоКанал", "Сож", "Японская ГЭС"};
            string[] rates = { "Субсидируемый", "Полный" };
            string[] results = { "Задолженность", "Оплачено", "Нет данных" };
            string RandomDigits(int length)
            {
                var random = new Random();
                string s = string.Empty;
                for (int i = 0; i < length; i++)
                    s = String.Concat(s, random.Next(10).ToString());
                return s;
            }
            DateTime RandomDay()
            {
                DateTime start = new DateTime(1995, 1, 1);
                int range = (DateTime.Today - start).Days;
                return start.AddDays(rand.Next(range));
            }
            if (!db.CounterModels.Any())
            {
                for (int i = 0; i < countermodel_num; i++)
                {
                    db.CounterModels.Add(new CounterModel { 
                        ModelName = model_names[rand.Next(0,model_names.Length)], 
                        Manufacturer = manufacturers[rand.Next(0, manufacturers.Length)],
                        ServiceTime = rand.Next(4, 18)
                    });
                }
                db.SaveChanges();
            }

            if (!db.Organizations.Any())
            {
                for (int i = 0; i < organization_num; i++)
                {
                    db.Organizations.Add(new Organization
                    {
                        OrgName = org_names[rand.Next(0, org_names.Length)],
                        OwnershipType = ownership_types[rand.Next(0, ownership_types.Length)],
                        Adress = countries[rand.Next(0, countries.Length)] + ", " + towns[rand.Next(1, towns.Length)] + ", " + streets[rand.Next(0, streets.Length)] + " " + rand.Next(1, 120),
                        DirectorFullname = first_names[rand.Next(0, first_names.Length)] + " " + last_names[rand.Next(0, last_names.Length)],
                        DirectorPhone = "+" + RandomDigits(9),
                        ResponsibleFullname = first_names[rand.Next(0, first_names.Length)] + " " + last_names[rand.Next(0, last_names.Length)],
                        ResponsiblePhone = "+" + RandomDigits(9),
                    });
                }
                db.SaveChanges();
            }

            if (!db.Rates.Any())
            {
                for (int i = 0; i < rates_num; i++)
                {
                    db.Rates.Add(new Rate
                    {
                        RateName = rates[rand.Next(0, rates.Length)] + ", " + towns[rand.Next(0, towns.Length)],
                        Price = rand.Next(15,60)
                    });
                }
                db.SaveChanges();
            }


            if (!db.Counters.Any())
            {
                for (int i = 0; i < counters_num; i++)
                {
                    db.Counters.Add(new Counter
                    {
                        ModelId = rand.Next(1, models_num),
                        TimeOfInstallation = RandomDay(),
                        OrganizationId = rand.Next(1, organization_num),
                    });
                }
                db.SaveChanges();
            }
            if (!db.CountersChecks.Any())
            {
                for (int i = 0; i < counters_num; i++)
                {
                    db.CountersChecks.Add(new CountersCheck
                    {
                        CounterRegistrationNumber = rand.Next(1, counters_num),
                        CheckDate = RandomDay(),
                        CheckResult = results[rand.Next(0, results.Length)],
                    });
                }
                db.SaveChanges();
            }

            if (!db.CountersData.Any())
            {
                for (int i = 0; i < counters_num; i++)
                {
                    db.CountersData.Add(new CountersDatum
                    {
                        CounterRegistrationNumber = rand.Next(1, counters_num),
                        DataCheckDate = RandomDay(),
                        Volume = rand.Next(400, 1200)
                    });
                }
                db.SaveChanges();
            }
            if (!db.RateOrgNotes.Any())
            {
                for (int i = 0; i < organization_num; i++)
                {
                    db.RateOrgNotes.Add(new RateOrgNote
                    {
                        RateId = rand.Next(1, rates_num),
                        OrganizationId = rand.Next(1, organization_num),
                    });
                }
                db.SaveChanges();
            }
        }
    }
}
