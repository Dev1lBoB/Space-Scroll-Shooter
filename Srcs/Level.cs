using Spice_Scroll_Shooter.Srcs.Enemies.Bosses;

namespace Spice_Scroll_Shooter
{
    public class Level
    {
        public string LevelName { get; }
        public int NumOfEnemies { get; }
        public int Limit { get; }
        public Level(string levelName, int numOfEnemies, int limit)
        {
            LevelName = levelName;
            NumOfEnemies = numOfEnemies;
            Limit = limit;
        }
    }
    public class BossLevel : Level
    {
        public ABoss Boss { get; set; }
        public BossLevel(string levelName, ABoss boss) : base(levelName, 1, 1) { Boss = boss; }
    }
}
