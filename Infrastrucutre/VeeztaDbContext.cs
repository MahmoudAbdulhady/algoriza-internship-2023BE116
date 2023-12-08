using Domain.Entities;
using Domain.Enums;
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
    
          new Specialization { SpecializationId = 6, SpecializationName = "Hematologists" },
          new Specialization { SpecializationId = 7, SpecializationName = "Pathologists" },
          new Specialization { SpecializationId = 8, SpecializationName = "Pediatricians" },
          new Specialization { SpecializationId = 9, SpecializationName = "Physiatrists" },
          new Specialization { SpecializationId = 10, SpecializationName = "Radiologists" },

          new Specialization { SpecializationId = 11, SpecializationName = "Urology" },
          new Specialization { SpecializationId = 12, SpecializationName = "Ophthalmology" },
          new Specialization { SpecializationId = 13, SpecializationName = "Surgery" },
          new Specialization { SpecializationId = 14, SpecializationName = "Endocrinologist" },
          new Specialization { SpecializationId = 15, SpecializationName = "Gastroenterology" }
          );


            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser
                {
                    AccountRole = AccountRole.Admin,
                    Email= "VeeztaAdmin@gmail.com",
                    FirstName="Veezta",
                    LastName="Admin",
                    FullName="VezetaAdmin",
                    DateOfBirth =new DateTime(1999/9/24),
                    Gender= Gender.Male,
                    ImageUrl="Admin",
                });

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



            ////RelationShip Between Booking and Doctor One To Many 
            //modelBuilder.Entity<Booking>()
            //    .HasOne(b => b.Doctor)
            //    .WithMany(d => d.Bookings)
            //    .HasForeignKey(b => b.DoctorId);


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


            //Releation Between Coupon and Patient (Idenitiy Table) One To Many 
            modelBuilder.Entity<Coupon>()
                .HasOne(c => c.Patient)
                .WithMany(p => p.Coupons)
                .HasForeignKey(c => c.PatientId)
                .IsRequired(false);
        }


        public DbSet<Appointement> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Time> Times { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Coupon> Coupons { get; set; }

    }
}
