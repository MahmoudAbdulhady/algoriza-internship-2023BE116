using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucutre
{
    public  class VeeztaDbContext : IdentityDbContext<CustomUser>
    {
        public VeeztaDbContext(DbContextOptions<VeeztaDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Specialization>().HasData(
          new Specialization { SpecializationId = 1, SpecializationName = "Dentist" },
          new Specialization { SpecializationId = 2, SpecializationName = "Neurology" },
          new Specialization { SpecializationId = 3, SpecializationName = "Cardiology" },
          new Specialization { SpecializationId = 4, SpecializationName = "Dermatology" },
          new Specialization { SpecializationId = 5, SpecializationName = "Surgeon" },
          // Add 5 more random specializations
          new Specialization { SpecializationId = 6, SpecializationName = "Hematologists" },
          new Specialization { SpecializationId = 7, SpecializationName = "Pathologists" },
          new Specialization { SpecializationId = 8, SpecializationName = "Pediatricians" },
          new Specialization { SpecializationId = 9, SpecializationName = "Physiatrists" },
          new Specialization { SpecializationId = 10, SpecializationName = "Radiologists" }
          );

            //releation Between Doctor and CustomUser (Idenitiy Table) One To One 
            modelBuilder.Entity<Doctor>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<Doctor>(d => d.UserId);



            //ReleationShip Betweeen Doctor And Appointement One To Many 
            modelBuilder.Entity<Appointement>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointements)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);


            //RelataionShip Between Doctor and Specialization One To Many
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId);


            //Relationship between Times Table and Appointment Table One To Many
            modelBuilder.Entity<Time>()
                .HasOne(t=> t.Appointements)
                .WithMany(a=> a.Times)
                .HasForeignKey(t => t.AppointmentId);


            //RelationShip Between Booking And Patient One To Many 
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Patient)
                .WithMany()
                .HasForeignKey(b => b.PatientId)
                .OnDelete(DeleteBehavior.Restrict);

            //RelationShip Between Booking And Appointment One To Many 
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Appointement)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //RelationShip Between Booking And Time One To Many 
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Time)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.TimeId)
                .OnDelete(DeleteBehavior.Cascade);
        }


        public DbSet<Appointement> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<Booking> Bookings { get; set; }

    }
}
