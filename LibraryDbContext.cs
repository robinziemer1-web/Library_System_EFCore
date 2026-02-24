using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;
using Library_systemEF.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Library_systemEF;



public class LibraryDbContext : DbContext
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Member> Members => Set<Member>();
    
    
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=efcoredb;Username=tester4;Password=test");
        optionsBuilder.UseSnakeCaseNamingConvention();
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Book_Author>()
            .HasKey(ba => new { ba.BookId, ba.AuthorId });

        modelBuilder.Entity<Book_Author>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);


        modelBuilder.Entity<Book>(e =>
        {

            e.HasIndex(b => b.ISBN)
                .IsUnique();

            e.Property(b => b.ISBN)
                .IsRequired()
                .HasMaxLength(100);


            e.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(100);

        });

        modelBuilder.Entity<Member>(e =>
        {

            e.Property(m => m.Name)
                .IsRequired()
                .HasMaxLength(100);

            e.Property(m => m.Email)
                .IsRequired()
                .HasMaxLength(200);
            e.Property(m => m.PasswordHash)
                .HasMaxLength(100)
                .IsRequired();



        });
        
            
            
            
        

        modelBuilder.Entity<Book_Author>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId);

       
        
        
            
        
        



    }
}