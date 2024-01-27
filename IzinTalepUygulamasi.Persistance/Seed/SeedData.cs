using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public static class SeedData
{
    public static void Seed(this EntityTypeBuilder<ADUser> builder)
    {
        builder.HasData(
                new ADUser { Id = Guid.Parse("e21cd525-031c-4364-b173-4150a4e18c37"), FirstName = "Münir", LastName = "Özkul", Email = "munir.ozkul@negzel.net", UserType = UserType.Manager },
                new ADUser { Id = Guid.Parse("59fb152a-2d59-435d-8fc1-cbc35c0f1d82"), FirstName = "Şener", LastName = "Şen", Email = "sener.sen@negzel.net", UserType = UserType.WhiteCollarEmployee, ManagerId = Guid.Parse("e21cd525-031c-4364-b173-4150a4e18c37") },
                new ADUser { Id = Guid.Parse("23591451-1cf1-46a5-907a-ee3e52abe394"), FirstName = "Kemal", LastName = "Sunal", Email = "kemal.sunal@negzel.net", UserType = UserType.BlueCollarEmployee, ManagerId = Guid.Parse("59fb152a-2d59-435d-8fc1-cbc35c0f1d82") }
            );
    }
}
