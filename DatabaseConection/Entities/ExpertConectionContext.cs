﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DatabaseConection.Entities;

public partial class ExpertConectionContext : DbContext
{
    public ExpertConectionContext()
    {
    }

    public ExpertConectionContext(DbContextOptions<ExpertConectionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Advise> Advises { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryMapping> CategoryMappings { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Expert> Experts { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Token> Tokens { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=ExpertConection;User Id=test;Password=Test;TrustServerCertificate=True;Trusted_Connection=true;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Password).HasMaxLength(400);
            entity.Property(e => e.RoleId).HasMaxLength(400);
            entity.Property(e => e.Username).HasMaxLength(400);

            entity.HasOne(d => d.Role).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Account_Role");
        });

        modelBuilder.Entity<Advise>(entity =>
        {
            entity.ToTable("Advise");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.CategoryMappingId).HasMaxLength(400);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.IdRating).HasMaxLength(400);
            entity.Property(e => e.UserId).HasMaxLength(400);

            entity.HasOne(d => d.CategoryMapping).WithMany(p => p.Advises)
                .HasForeignKey(d => d.CategoryMappingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Advise_CategoryMapping");

            entity.HasOne(d => d.IdRatingNavigation).WithMany(p => p.Advises)
                .HasForeignKey(d => d.IdRating)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Advise_Rating");

            entity.HasOne(d => d.User).WithMany(p => p.Advises)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Advise_User");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(400);
        });

        modelBuilder.Entity<CategoryMapping>(entity =>
        {
            entity.ToTable("CategoryMapping");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.CategoryId).HasMaxLength(400);
            entity.Property(e => e.Description).HasMaxLength(400);
            entity.Property(e => e.ExpertId).HasMaxLength(400);
            entity.Property(e => e.Introduction).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryMappings)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryMapping_Category");

            entity.HasOne(d => d.Expert).WithMany(p => p.CategoryMappings)
                .HasForeignKey(d => d.ExpertId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryMapping_Expert");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("Chat");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AdviseId).HasMaxLength(400);
            entity.Property(e => e.ContentChat).HasMaxLength(400);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.FromAcc).HasMaxLength(400);
            entity.Property(e => e.ImageUrl).HasMaxLength(400);
            entity.Property(e => e.ToAc).HasMaxLength(400);

            entity.HasOne(d => d.Advise).WithMany(p => p.Chats)
                .HasForeignKey(d => d.AdviseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chat_Advise");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employee");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AcId).HasMaxLength(400);
            entity.Property(e => e.Fullname).HasMaxLength(200);

            entity.HasOne(d => d.Ac).WithMany(p => p.Employees)
                .HasForeignKey(d => d.AcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Employee_Account");
        });

        modelBuilder.Entity<Expert>(entity =>
        {
            entity.ToTable("Expert");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AcId).HasMaxLength(400);
            entity.Property(e => e.CerfificateLink).HasMaxLength(400);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Fullname).HasMaxLength(200);
            entity.Property(e => e.Introduction).HasMaxLength(400);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsFixedLength();
            entity.Property(e => e.WorlkRole).HasMaxLength(100);

            entity.HasOne(d => d.Ac).WithMany(p => p.Experts)
                .HasForeignKey(d => d.AcId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Expert_Account");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.ToTable("Rating");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Comment).HasMaxLength(400);
            entity.Property(e => e.UserId).HasMaxLength(400);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.ToTable("Token");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AccId).HasMaxLength(400);
            entity.Property(e => e.AccessToken).HasMaxLength(400);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");

            entity.HasOne(d => d.Acc).WithMany(p => p.Tokens)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Token_Account");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).HasMaxLength(400);
            entity.Property(e => e.AcountId).HasMaxLength(400);
            entity.Property(e => e.Address).HasMaxLength(400);
            entity.Property(e => e.Birthday).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.FullName).HasMaxLength(400);
            entity.Property(e => e.Introduction).HasMaxLength(400);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);

            entity.HasOne(d => d.Acount).WithMany(p => p.Users)
                .HasForeignKey(d => d.AcountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
