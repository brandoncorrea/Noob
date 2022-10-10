using Noob.DL;

namespace Noob.Discord.SlashCommands
{
    public class DailyCommand : RecurrentCommand
    {
        public override string CommandName => "daily";

        public DailyCommand(
            IUserRepository userRepository,
            IUserCommandRepository userCommandRepository)
            : base(userRepository, userCommandRepository)
        {
            IntervalDays = 1;
            CommandId = 1;
        }

        internal override int GetNiblets() => new Random().Next(1, 50);
    }
}
