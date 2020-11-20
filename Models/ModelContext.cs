using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SBD.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Grade> Grade { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupStudent> GroupStudent { get; set; }
        public virtual DbSet<GroupSubject> GroupSubject { get; set; }
        public virtual DbSet<LoginData> LoginData { get; set; }
        public virtual DbSet<Message> Message { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        public virtual DbSet<Teacher> Teacher { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=212.33.90.213)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=SBD_ST_PS3_4;Password=Bazy$Projekt$Tajne4;",
                    o => o.UseOracleSQLCompatibility("11"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "SBD_ST_PS3_4");

            modelBuilder.Entity<Event>(entity =>
            {
                entity.ToTable("EVENT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.Date).HasColumnType("DATE");

                entity.Property(e => e.Description)
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TeacherId)
                    .HasColumnName("TEACHER_ID")
                    .HasColumnType("NUMBER(6)");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("EVENT_TEACHER_FK");
            });

            modelBuilder.Entity<Grade>(entity =>
            {
                entity.ToTable("GRADE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.Date).HasColumnType("DATE");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("DESCRIPTION")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.StudentId)
                    .HasColumnName("STUDENT_ID")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SUBJECT_ID")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.Value)
                    .IsRequired()
                    .HasColumnName("VALUE")
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Grade)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("GRADE_STUDENT_FK");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Grade)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("GRADE_SUBJECT_FK");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<GroupStudent>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.StudentId })
                    .HasName("GROUP_STUDENT_PK");

                entity.ToTable("GROUP_STUDENT");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasColumnType("NUMBER(6)")
                    .ValueGeneratedNever();

                entity.Property(e => e.StudentId)
                    .HasColumnName("STUDENT_ID")
                    .HasColumnType("NUMBER(6)")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupStudent)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("GROUP_STUDENT_GROUP_FK");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.GroupStudent)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("GROUP_STUDENT_STUDENT_FK");
            });

            modelBuilder.Entity<GroupSubject>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.SubjectId })
                    .HasName("GROUP_SUBJECT_PK");

                entity.ToTable("GROUP_SUBJECT");

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasColumnType("NUMBER(6)")
                    .ValueGeneratedNever();

                entity.Property(e => e.SubjectId)
                    .HasColumnName("SUBJECT_ID")
                    .HasColumnType("NUMBER(6)")
                    .ValueGeneratedNever();

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupSubject)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("GROUP_SUBJECT_GROUP_FK");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.GroupSubject)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("GROUP_SUBJECT_SUBJECT_FK");
            });

            modelBuilder.Entity<LoginData>(entity =>
            {
                entity.ToTable("LOGIN_DATA");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("LOGIN")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("PASSWORD")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.ToTable("MESSAGE");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("CONTENT")
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("DATE");

                entity.Property(e => e.StudentId)
                    .HasColumnName("STUDENT_ID")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.TeacherId)
                    .HasColumnName("TEACHER_ID")
                    .HasColumnType("NUMBER(6)");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("MESSAGE_STUDENT_FK");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Message)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("MESSAGE_TEACHER_FK");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("STUDENT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(6)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("FIRST_NAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SecondName)
                    .HasColumnName("SECOND_NAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("SURNAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.Id)
                    .HasConstraintName("STUDENT_LOGIN_DATA_FK");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("SUBJECT");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("NAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TeacherId)
                    .HasColumnName("TEACHER_ID")
                    .HasColumnType("NUMBER(6)");

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Subject)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("SUBJECT_TEACHER_FK");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("TEACHER");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasColumnType("NUMBER(6)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("FIRST_NAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SecondName)
                    .HasColumnName("SECOND_NAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasColumnName("SURNAME")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdNavigation)
                    .WithOne(p => p.Teacher)
                    .HasForeignKey<Teacher>(d => d.Id)
                    .HasConstraintName("TEACHER_LOGIN_DATA_FK");
            });

            modelBuilder.HasSequence("EVENT_SQ");

            modelBuilder.HasSequence("GRADE_SQ");

            modelBuilder.HasSequence("GROUP_SQ");

            modelBuilder.HasSequence("LOGIN_SQ");

            modelBuilder.HasSequence("MESSAGE_SQ");

            modelBuilder.HasSequence("STUDENT_SQ");

            modelBuilder.HasSequence("SUBJECT_SQ");

            modelBuilder.HasSequence("TEACHER_SQ");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
