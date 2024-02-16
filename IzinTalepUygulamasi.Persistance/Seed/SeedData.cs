using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class SeedData : IEntityTypeConfiguration<ADUser>
{
    public void Configure(EntityTypeBuilder<ADUser> builder)
    {
        builder.HasData(
                new ADUser(Guid.Parse("e21cd525-031c-4364-b173-4150a4e18c37"), "Münir", "Özkul", "munir.ozkul@negzel.net", UserType.Manager, null),
                new ADUser(Guid.Parse("59fb152a-2d59-435d-8fc1-cbc35c0f1d82"), "Şener", "Şen", "sener.sen@negzel.net", UserType.WhiteCollarEmployee, Guid.Parse("e21cd525-031c-4364-b173-4150a4e18c37")),
                new ADUser(Guid.Parse("23591451-1cf1-46a5-907a-ee3e52abe394"), "Kemal", "Sunal", "kemal.sunal@negzel.net", UserType.BlueCollarEmployee, Guid.Parse("e21cd525-031c-4364-b173-4150a4e18c37"))
            );
    }
}
