using Noob.DL;

namespace Noob.Discord.SlashCommands
{
    public class WeeklyCommand : RecurrentCommand
    {
        public override string CommandName => "weekly";

        public WeeklyCommand(
            IUserRepository userRepository,
            IUserCommandRepository userCommandRepository)
            : base(userRepository, userCommandRepository)
        {
            IntervalDays = 7;
            CommandId = 2;
        }

        internal override int GetNiblets() => new Random().Next(50, 250);
    }
}
