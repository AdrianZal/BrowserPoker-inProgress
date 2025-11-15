using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PokerServer.Models;

public partial class PokerContext : DbContext
{
    public PokerContext()
    {
    }

    public PokerContext(DbContextOptions<PokerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Card> Cards { get; set; }

    public virtual DbSet<CardFrontSkin> CardFrontSkins { get; set; }

    public virtual DbSet<CardReverseSkin> CardReverseSkins { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerEquippedFaceSkin> PlayerEquippedFaceSkins { get; set; }

    public virtual DbSet<PlayerEquippedReverseSkin> PlayerEquippedReverseSkins { get; set; }

    public virtual DbSet<PlayerOwnedFaceSkin> PlayerOwnedFaceSkins { get; set; }

    public virtual DbSet<PlayerOwnedReverseSkin> PlayerOwnedReverseSkins { get; set; }

    public virtual DbSet<PlayerTable> PlayerTables { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Table> Tables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>(entity =>
        {
            entity.HasKey(e => e.Card1).HasName("Card_pk");

            entity.ToTable("Card");

            entity.Property(e => e.Card1)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("card");
        });

        modelBuilder.Entity<CardFrontSkin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CardFrontSkin_pk");

            entity.ToTable("CardFrontSkin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Card)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("card");
            entity.Property(e => e.Filename)
                .HasMaxLength(54)
                .IsUnicode(false)
                .HasColumnName("filename");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");

            entity.HasOne(d => d.CardNavigation).WithMany(p => p.CardFrontSkins)
                .HasForeignKey(d => d.Card)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CardFrontSkin_Card");
        });

        modelBuilder.Entity<CardReverseSkin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CardReverseSkin_pk");

            entity.ToTable("CardReverseSkin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Filename)
                .HasMaxLength(54)
                .IsUnicode(false)
                .HasColumnName("filename");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Player_pk");

            entity.ToTable("Player", tb => tb.HasTrigger("tr_Player_Insert_AddDefaultSkins"));

            entity.HasIndex(e => e.Email, "UQ_Player_Email").IsUnique();

            entity.HasIndex(e => e.Name, "UQ_Player_Name").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Balance).HasColumnName("balance");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(450)
                .IsUnicode(false)
                .HasColumnName("password");
        });

        modelBuilder.Entity<PlayerEquippedFaceSkin>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.Card }).HasName("PlayerEquippedFaceSkin_pk");

            entity.ToTable("PlayerEquippedFaceSkin");

            entity.Property(e => e.PlayerId).HasColumnName("Player_id");
            entity.Property(e => e.Card)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("card");
            entity.Property(e => e.SkinId).HasColumnName("Skin_id");

            entity.HasOne(d => d.CardNavigation).WithMany(p => p.PlayerEquippedFaceSkins)
                .HasForeignKey(d => d.Card)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayerEquippedFaceSkin_Card");

            entity.HasOne(d => d.PlayerOwnedFaceSkin).WithMany(p => p.PlayerEquippedFaceSkins)
                .HasForeignKey(d => new { d.PlayerId, d.SkinId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayerEquippedFaceSkin_PlayerOwnedFaceSkin");
        });

        modelBuilder.Entity<PlayerEquippedReverseSkin>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PlayerEquippedReverseSkin_pk");

            entity.ToTable("PlayerEquippedReverseSkin");

            entity.Property(e => e.PlayerId)
                .ValueGeneratedNever()
                .HasColumnName("Player_id");
            entity.Property(e => e.SkinId).HasColumnName("Skin_id");

            entity.HasOne(d => d.PlayerOwnedReverseSkin).WithMany(p => p.PlayerEquippedReverseSkins)
                .HasForeignKey(d => new { d.PlayerId, d.SkinId })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayerEquippedReverseSkin_PlayerOwnedReverseSkin");
        });

        modelBuilder.Entity<PlayerOwnedFaceSkin>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.SkinId }).HasName("PlayerOwnedFaceSkin_pk");

            entity.ToTable("PlayerOwnedFaceSkin");

            entity.Property(e => e.PlayerId).HasColumnName("Player_id");
            entity.Property(e => e.SkinId).HasColumnName("Skin_id");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerOwnedFaceSkins)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayerOwnedFaceSkin_Player");

            entity.HasOne(d => d.Skin).WithMany(p => p.PlayerOwnedFaceSkins)
                .HasForeignKey(d => d.SkinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayerOwnedFaceSkin_CardFrontSkin");
        });

        modelBuilder.Entity<PlayerOwnedReverseSkin>(entity =>
        {
            entity.HasKey(e => new { e.PlayerId, e.SkinId }).HasName("PlayerOwnedReverseSkin_pk");

            entity.ToTable("PlayerOwnedReverseSkin");

            entity.Property(e => e.PlayerId).HasColumnName("Player_id");
            entity.Property(e => e.SkinId).HasColumnName("Skin_id");

            entity.HasOne(d => d.Player).WithMany(p => p.PlayerOwnedReverseSkins)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayerOwnedReverseSkin_Player");

            entity.HasOne(d => d.Skin).WithMany(p => p.PlayerOwnedReverseSkins)
                .HasForeignKey(d => d.SkinId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("PlayerOwnedReverseSkin_CardReverseSkin");
        });

        modelBuilder.Entity<PlayerTable>(entity =>
        {
            entity.HasKey(e => e.PlayerId).HasName("PlayerTable_pk");

            entity.ToTable("PlayerTable");

            entity.Property(e => e.PlayerId)
                .ValueGeneratedNever()
                .HasColumnName("Player_id");
            entity.Property(e => e.TableBalance).HasColumnName("table_balance");
            entity.Property(e => e.TableJoinCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("Table_joinCode");

            entity.HasOne(d => d.Player).WithOne(p => p.PlayerTable)
                .HasForeignKey<PlayerTable>(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TablePlayer_Player");

            entity.HasOne(d => d.TableJoinCodeNavigation).WithMany(p => p.PlayerTables)
                .HasForeignKey(d => d.TableJoinCode)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TablesPlayers_Tables");
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RefreshToken_pk");

            entity.ToTable("RefreshToken");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("expires_at");
            entity.Property(e => e.PlayerId).HasColumnName("Player_id");
            entity.Property(e => e.Revoked).HasColumnName("revoked");
            entity.Property(e => e.TokenHash)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("token_hash");

            entity.HasOne(d => d.Player).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.PlayerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RefreshToken_Player");
        });

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.JoinCode).HasName("Table_pk");

            entity.ToTable("Table");

            entity.Property(e => e.JoinCode)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasColumnName("join_code");
            entity.Property(e => e.BuyIn).HasColumnName("buy_in");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
