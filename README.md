# Library System EF CORE (Library Console App)

This is a Library Application system which using c# with EF CORE with Code first migrations + POSTGRESQL database.

# Features

- First menu shows what the user can choose what to do.
- Logging in to the system, create a new member to system or browse library system.

## Logged in to the system.
-A member menu appears where the user can loan books, return books or delete the membership.
 -"Loan a book" will print out all the books in a list and showing which one is available or "on loan".
  -User has to type a bookId to successfully borrow a book from the list.
 -The user can check its book-loan list and how many he/she has borrowed.
 -"Return a book" will print out all the book user has borrowed and need to type in specific (bookId) to return.
 -User can choose to delete he/hers membership, but cant have a borrowed book. Has to return it first to do it.
 -The user can simply press "0" to exit to the first menu.

## Creating a new member
-User can create a new member from the first menu. Then user will choose name, email and password.

## Browse Library
-When choosing "Browse library", the user can browse all books, all members and add a book to the system.
 -When browse all books, all the books from the library will appear and showing which one is available or "on loan".
 -Browse all members will show all members name and their active book loans count from the system.
 -Adding a new book + new author that will connect togheter as (M-M) with explicit transaction.

#Entities

The system contains the following entities:

-Book
-Author
-Member
-Loan
-Book_author (Join-entity (M : M relationship))

# Relationships

- One to many:
 -Member to Loans
 -Book to Loans

- Many to many:
 -Book <-> Author(in Book_Author)

# Seed-data

The system uses EF CORE to read the JSON files to seed data into the database.

When the application starts the DbSeeder class checks if the database already contains the data. If not it reads data
with EF core from the following JSON files.

- Authors-seed.json
- Books-seed.json
- Book_Authors-seed.json


These files contain predefined authors, books, and the many-to-many connections between them.

This means that when running the application for the first time, the library already contains books and authors.  
Members and loans are not seeded — they are created dynamically by the user through the application.
