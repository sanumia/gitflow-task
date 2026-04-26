using Serialization;

using var context = new SerializationDbContext();
context.Database.EnsureDeleted();
context.Database.EnsureCreated();
var userRepo = new Repository<User>(context);

var csvUsers = new CsvEnumerable<User>("Data/user.csv");

userRepo.AddRange(csvUsers);

var users = userRepo.GetAll();

var contracts = users.Select(u => new UserContract
{
    Id = u.Id,
    Name = u.Name,
    Email = u.Email,
    DateOfCreation = u.DateOfCreation,
});

Directory.CreateDirectory("Output");
JsonExporter.Export(contracts, "Output/users.json");