using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MusicSchoolEF.Models.Db;

public partial class Ms2Context : DbContext
{
    public Ms2Context()
    {
    }

    public Ms2Context(DbContextOptions<Ms2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<Node> Nodes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<StudentNodeConnection> StudentNodeConnections { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseLazyLoadingProxies()
            .UseMySql("server=localhost;port=3306;user=root;password=root;database=ms_2", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.7.24-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8_general_ci")
            .HasCharSet("utf8");

        modelBuilder.Entity<Group>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("groups");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Node>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("nodes");

            entity.HasIndex(e => e.Owner, "nodes_fk0");

            entity.HasIndex(e => e.ParentId, "nodes_fk1");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(5000)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Owner)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("owner");
            entity.Property(e => e.ParentId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("parent_id");
            entity.Property(e => e.Priority)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("priority");

            entity.HasOne(d => d.OwnerNavigation).WithMany(p => p.Nodes)
                .HasForeignKey(d => d.Owner)
                .HasConstraintName("nodes_fk0");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("nodes_fk1");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.HasIndex(e => e.Name, "name").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<StudentNodeConnection>(entity =>
        {
            entity.HasKey(e => new { e.NodeId, e.StudentId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("student_node_connections");

            entity.HasIndex(e => e.StudentId, "student_node_connections_fk1");

            entity.Property(e => e.NodeId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("node_id");
            entity.Property(e => e.StudentId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("student_id");
            entity.Property(e => e.Comment)
                .HasMaxLength(5000)
                .HasColumnName("comment");
            entity.Property(e => e.Mark)
                .HasColumnType("int(11)")
                .HasColumnName("mark");

            entity.HasOne(d => d.Node).WithMany(p => p.StudentNodeConnections)
                .HasForeignKey(d => d.NodeId)
                .HasConstraintName("student_node_connections_fk0");

            entity.HasOne(d => d.Student).WithMany(p => p.StudentNodeConnections)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("student_node_connections_fk1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "login").IsUnique();

            entity.HasIndex(e => e.RoleId, "role_id");

            entity.Property(e => e.Id)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("id");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(255)
                .HasColumnName("patronymic");
            entity.Property(e => e.RoleId)
                .HasColumnType("int(10) unsigned")
                .HasColumnName("role_id");
            entity.Property(e => e.Surname)
                .HasMaxLength(255)
                .HasColumnName("surname");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_ibfk_1");

            entity.HasMany(d => d.Groups).WithMany(p => p.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentGroupConnection",
                    r => r.HasOne<Group>().WithMany()
                        .HasForeignKey("GroupId")
                        .HasConstraintName("student_group_connections_ibfk_1"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("StudentId")
                        .HasConstraintName("student_group_connections_fk0"),
                    j =>
                    {
                        j.HasKey("StudentId", "GroupId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("student_group_connections");
                        j.HasIndex(new[] { "GroupId" }, "group");
                        j.IndexerProperty<uint>("StudentId")
                            .HasColumnType("int(10) unsigned")
                            .HasColumnName("student_id");
                        j.IndexerProperty<uint>("GroupId")
                            .ValueGeneratedOnAdd()
                            .HasColumnType("int(10) unsigned")
                            .HasColumnName("group_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
