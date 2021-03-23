namespace Services
{
    public class BaseRepository
    {
        public DbAppContext context;

        public BaseRepository(DbAppContext context)
        {
            this.context = context;
        }
    }
}