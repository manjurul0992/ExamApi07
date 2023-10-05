namespace ExamApi07.Migrations
{
    using ExamApi07.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ExamApi07.Models.DeviceDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ExamApi07.Models.DeviceDbContext context)
        {
            Device d = new Device { DeviceName = "Nokia", ReleaseDate = new DateTime(2023, 2, 1), OnSale = true, Picture = "1.png", Price = 1200 };
            d.Specs.Add(new Spec { SpecName = "RAM", Value = "10" });
            d.Specs.Add(new Spec { SpecName = "Storage", Value = "10GB" });
            context.Devices.Add(d);
            context.SaveChanges();
        }
    }
}
