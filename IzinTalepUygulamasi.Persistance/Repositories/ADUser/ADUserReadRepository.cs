public class ADUserReadRepository : ReadRepository<ADUser>, IADUserReadRepository
{
    public ADUserReadRepository(IzinTalepAPIContext context) : base(context)
    {
    }
}
