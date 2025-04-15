---
author: Lektion 5
date: MMMM dd, YYYY
paging: "%d / %d"
---

# Lektion 5

Hej och välkommen!

## Dagens agenda

1. Frågor och repetition
2. Repetition PostgreSQL
3. Introduktion till Entity Framework
4. Repetition repository-pattern
5. Övning med handledning

---

# Fråga

Vad är skillnaden mellan IEnumerable och ICollection?

# Svar

- **IEnumerable:** något som går att iterera igenom (loopa)
- **ICollection:** något som går att iterera igenom och modifiera (add, remove)

`ICollection` ärver av `IEnumerable`.

---

## Installera PostgreSQL

1. `docker pull postgres`
2. `docker run -p 5432:5432 -e POSTGRES_PASSWORD=password --name my-postgres -d postgres`
3. `docker start my-postgres` - om container är stoppad

## Gå in i PostgreSQL

1. `docker exec -it my-postgres bash` - gå in i container
2. `psql -U postgres` - starta klient och ange lösenord
3. `\l` - lista upp databaser
4. `CREATE DATABASE mydb;` - skapa egen databas
5. `\c mydb` - anslut till egen databas

---

# PostgreSQL kod exempel

```csharp
using var conn = new NpgsqlConnection("Host=localhost;Database=mydb;Username=myuser;Password=mypassword");
await conn.OpenAsync();

using var cmd = new NpgsqlCommand(@"
  CREATE TABLE users (name VARCHAR(50) NOT NULL, email VARCHAR(100) UNIQUE NOT NULL)", conn);
await cmd.ExecuteNonQueryAsync();

using var cmd = new NpgsqlCommand(@"INSERT INTO users (name, email) VALUES (@name, @email)", conn);
cmd.Parameters.AddWithValue("@name", "Ironman");
cmd.Parameters.AddWithValue("@email", "tony@stark.com");
await cmd.ExecuteNonQueryAsync();

using var cmd = new NpgsqlCommand("SELECT * FROM users", conn);
using var reader = await cmd.ExecuteReaderAsync();
while (await reader.ReadAsync()) {
    Console.WriteLine($"ID: {reader.GetInt32(0)}, Name: {reader.GetString(1)}, Email: {reader.GetString(2)}");
}
```

---

# INSERT, SELECT, UPDATE, DELETE

CRUD nyckelord för SQL.

- **INSERT** (create): ladda upp data
- **SELECT** (read): hämta/sök data
- **UPDATE** (update): ändra befintlig data
- **DELETE** (delete): radera data

---

# Introduktion till ORMs

Object-relational mapping, förkortat ORM, är ett verktyg som utvecklare kan använda för att förenkla databashantering.

- Skapar tabeller (CREATE TABLE)
- Automatiserar queries (SELECT, INSERT, DELETE)
- Förenklar villkor och joins
- Hanterar versioner av tabeller

---

# Entity Framework (EF)

Entity Framework är en ORM för C# som har stöd för det mesta.

- Klasser bildar modeller som bildar tabeller
- Migrations för att uppdatera tabeller
- Funktioner för CRUD
- Stöd för relationer och joins

```sh
# Installera och kom igång med EF i ett projekt
dotnet tool install --global dotnet-ef
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
```

<https://learn.microsoft.com/en-us/ef/core/get-started/overview/install>

---

# DbContext

- Ansluter till databas
- Kommunicerar med databas (queries)
- Hanterar relationer och joins automatiskt

---

# Exempel DbContext

```csharp
public class Todo // Model
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
}

public class AppContext : DbContext
{
    public DbSet<Todo> Todos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        builder.UseNpgsql("Host=localhost;Database=todos;Username=postgres;Password=password");
    }
}
```

---

# Migrations

- Beskrivning av tabeller
- Håller koll på historik och versioner
- Skapar och uppdaterar tabeller

```sh
# Skapa en migration och uppdatera databasen
dotnet ef migrations add [namn]
dotnet ef database update
```

---

# Ett enkelt exempel

```csharp
using var db = new AppContext();

var todo = new Todo { Title = "Städa" };

// INSERT: Spara todo
db.Todos.Add(todo);
db.SaveChanges();

// DELETE: Radera todo
db.Todos.Remove(todo);
db.SaveChanges();

// SELECT: Hämta todos
var todos = db.Todos.Where(todo => todo.Title.Contains("a"));
foreach (var todo in todos)
{
    Console.WriteLine(todo.Title);
}
```
