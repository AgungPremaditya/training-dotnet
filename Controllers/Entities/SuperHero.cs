namespace SuperHeroAPI.Controllers.Entities
{
    public class SuperHero
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } =  string.Empty;
    }
}
