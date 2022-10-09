namespace Noob.Core.Models;

public class User
{
    public ulong Id { get; set; }
    public int BrowniePoints { get; set; }
    public int Niblets { get; set; }
    public long Experience { get; set; }
    public long Level => Experience / 100 + 1;

    public User SetNiblets(int niblets)
    {
        Niblets = niblets;
        return this;
    }

    public User SetExperience(int experience)
    {
        Experience = experience;
        return this;
    }

    public User SetBrowniePoints(int browniePoints)
    {
        BrowniePoints = browniePoints;
        return this;
    }
}
