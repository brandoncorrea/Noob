using System;
using Discord;
using Noob.API.Models;
using Noob.API.Repositories;

namespace Noob.API.Commands
{
    public class GiveCommand
    {
        private IUserRepository UserRepository;
        public GiveCommand(IUserRepository userRepository) =>
            UserRepository = userRepository;

        public CommandResponse Give(ISlashCommandInteraction command)
        {
            IUser discordFrom = command.User;
            IUser discordTo = (IUser)command.Data.Options.First().Value;
            int amount = unchecked((int)(long)command.Data.Options.Last().Value);

            if (amount < 0)
                return CommandResponse.Fail("Are you trying to /steal Niblets?");
            else if (amount == 0)
                return CommandResponse.Fail("How many Niblets do you want to give?");

            User from = UserRepository.FindOrCreate(discordFrom.Id);
            if (amount > from.Niblets)
                return CommandResponse.Fail("You don't have enough Niblets!");
            else if (from.Id == discordTo.Id)
                return CommandResponse.Ok($"You gave yourself {NibletTerm(amount)}!");

            User to = UserRepository.FindOrCreate(discordTo.Id);
            int earnedBrowniePoints = amount / 5;

            from.Niblets -= amount;
            from.BrowniePoints += earnedBrowniePoints;
            to.Niblets += amount;
            UserRepository.Save(from);
            UserRepository.Save(to);

            var phraseEnding = earnedBrowniePoints == 0 ? "!" : $", earning yourself {BrownieTerm(earnedBrowniePoints)} :)";
            return CommandResponse.Ok($"You gave {discordTo.Username} {NibletTerm(amount)}{phraseEnding}");
        }

        private static string NibletTerm(int niblets) =>
            niblets == 1 ? "1 Niblet" : $"{niblets} Niblets";
        private static string BrownieTerm(int browniePoints) =>
            browniePoints == 1 ? "1 Brownie Point" : $"{browniePoints} Brownie Points";
    }
}
