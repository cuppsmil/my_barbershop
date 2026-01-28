using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace My_Barbershop.Models;

public partial class BarbershopContext : DbContext
{
    public BarbershopContext()
    {
    }
    public BarbershopContext(DbContextOptions<BarbershopContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Administrator> Administrators { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Barber> Barbers { get; set; }

    public virtual DbSet<Barberclient> Barberclients { get; set; }

    public virtual DbSet<Barberservice> Barberservices { get; set; }

    public virtual DbSet<Barbershop> Barbershops { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=barbershop;Username=postgres;Password=bboystr2469");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Administrator>(entity =>
        {
            entity.HasKey(e => e.AdminPassport).HasName("administrator_pkey");

            entity.ToTable("administrator");

            entity.Property(e => e.AdminPassport)
                .ValueGeneratedNever()
                .HasColumnName("admin_passport");
            entity.Property(e => e.AdmFio)
                .HasMaxLength(254)
                .HasColumnName("adm_fio");
            entity.Property(e => e.AdminPhnumber).HasColumnName("admin_phnumber");
            entity.Property(e => e.BNum).HasColumnName("b_num");

            entity.HasOne(d => d.BNumNavigation).WithMany(p => p.Administrators)
                .HasForeignKey(d => d.BNum)
                .HasConstraintName("administrator_b_num_fkey");
            entity.Property(e => e.PasswordHash)
              .IsRequired()
              .HasMaxLength(255)
              .HasColumnName("password_hash");

        


        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("appointment_pkey");

            entity.ToTable("appointment");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Appointmentdate).HasColumnName("appointmentdate");
            entity.Property(e => e.Appointmenttime).HasColumnName("appointmenttime");
            entity.Property(e => e.Barberpassport).HasColumnName("barberpassport");
            entity.Property(e => e.Clientphone).HasColumnName("clientphone");
            entity.Property(e => e.Servicename)
                .HasMaxLength(254)
                .HasColumnName("servicename");

            entity.HasOne(d => d.BarberpassportNavigation)
            .WithMany(p => p.Appointments)
       .HasForeignKey(d => d.Barberpassport)
       .HasConstraintName("appointment_barberpassport_fkey")
       .OnDelete(DeleteBehavior.Cascade);
           
            entity.HasOne<Client>() // ← Не привязываемся к свойству, указываем тип напрямую
       .WithMany() // ← Клиент может иметь много записей
       .HasForeignKey(a => a.Clientphone) // ← Поле внешнего ключа
       .HasConstraintName("appointment_clientphone_fkey")
       .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(d => d.ServicenameNavigation)
           .WithMany(p => p.Appointments)
      .HasForeignKey(d => d.Servicename)
      .HasConstraintName("appointment_servicename_fkey")
      .OnDelete(DeleteBehavior.Cascade);


            entity.HasOne(d => d.BarberpassportNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Barberpassport)
                .HasConstraintName("appointment_barberpassport_fkey");

            entity.HasOne(d => d.ClientphoneNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Clientphone)
                .HasConstraintName("appointment_clientphone_fkey");

            entity.HasOne(d => d.ServicenameNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.Servicename)
                .HasConstraintName("appointment_servicename_fkey");
            
        });

        modelBuilder.Entity<Barber>(entity =>
        {
            entity.HasKey(e => e.BarberPassport).HasName("barber_pkey");

            entity.ToTable("barber");

            entity.Property(e => e.BarberPassport)
                .ValueGeneratedNever()
                .HasColumnName("barber_passport");
            entity.Property(e => e.BNum).HasColumnName("b_num");
            entity.Property(e => e.BarberFio)
                .HasMaxLength(254)
                .HasColumnName("barber_fio");
            entity.Property(e => e.BarberPhnumber).HasColumnName("barber_phnumber");

            entity.HasOne(d => d.BNumNavigation).WithMany(p => p.Barbers)
                .HasForeignKey(d => d.BNum)
                .HasConstraintName("barber_b_num_fkey");
        });

        modelBuilder.Entity<Barberclient>(entity =>
        {
            entity.ToTable("barberclient");
            entity.HasKey(bc => new { bc.Barbpass, bc.Clientphone });

            entity.Property(e => e.Barbpass).HasColumnName("barbpass");
            entity.Property(e => e.Clientphone).HasColumnName("clientphone");

            entity.HasOne(d => d.BarbpassNavigation).WithMany()
                .HasForeignKey(d => d.Barbpass)
                .HasConstraintName("barberclient_barbpass_fkey");

            entity.HasOne(d => d.ClientphoneNavigation).WithMany()
                .HasForeignKey(d => d.Clientphone)
                .HasConstraintName("barberclient_clientphone_fkey");
             entity.HasOne(d => d.ClientphoneNavigation)
        .WithMany()
        .HasForeignKey(d => d.Clientphone)
        .HasConstraintName("barberclient_clientphone_fkey")
        .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Barberservice>(entity =>
        {
            entity.ToTable("barberservice");
            entity.HasKey(bc => new { bc.Barbpass, bc.Servname });

            entity.Property(e => e.Barbpass).HasColumnName("barbpass");
            entity.Property(e => e.Servname)
                .HasMaxLength(254)
                .HasColumnName("servname");

            entity.HasOne(d => d.BarbpassNavigation).WithMany()
                .HasForeignKey(d => d.Barbpass)
                .HasConstraintName("barberservice_barbpass_fkey");

            entity.HasOne(d => d.ServnameNavigation).WithMany()
                .HasForeignKey(d => d.Servname)
                .HasConstraintName("barberservice_servname_fkey");
            entity.HasOne(d => d.ServnameNavigation)
        .WithMany()
        .HasForeignKey(d => d.Servname)
        .HasConstraintName("barberservice_servname_fkey")
        .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Barbershop>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("barbershop_pkey");

            entity.ToTable("barbershop");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(254)
                .HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(254)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientPhnum).HasName("client_pkey");

            entity.ToTable("client");

            entity.Property(e => e.ClientPhnum)
                .ValueGeneratedNever()
                .HasColumnName("client_phnum");
            entity.Property(e => e.BonusPoints)
                .HasDefaultValueSql("0")
                .HasColumnName("bonus_points");
            entity.Property(e => e.CardIssueDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("card_issue_date");
            entity.Property(e => e.ClFio)
                .HasMaxLength(254)
                .HasColumnName("cl_fio");
            entity.Property(e => e.DiscountCard)
                .HasMaxLength(254)
                .HasColumnName("discount_card");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .HasColumnName("password_hash");
           

        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.ServName).HasName("service_pkey");

            entity.ToTable("service");

            entity.Property(e => e.ServName)
                .HasMaxLength(254)
                .HasColumnName("serv_name");
            entity.Property(e => e.Durofwork).HasColumnName("durofwork");
            entity.Property(e => e.ServPrice).HasColumnName("serv_price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
