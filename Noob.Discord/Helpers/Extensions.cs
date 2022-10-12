using Discord;
using Noob.Core.Helpers;
namespace Noob.Discord.Helpers;

public static class Extensions
{
    public static EmbedBuilder WithSimpleAuthor(this EmbedBuilder builder, IUser author) =>
        builder.WithAuthor(
            author.Username,
            author.GetAvatarUrl() ?? author.GetDefaultAvatarUrl());

    public static EmbedBuilder WithDescription(this EmbedBuilder builder, IEnumerable<string> lines) =>
        builder.WithDescription(lines.Join("\n"));
    public static EmbedBuilder WithDescription(this EmbedBuilder builder, params string[] lines) =>
        builder.WithDescription(lines.Join("\n"));

    private static T Reduce<T, V>(this T init, Func<V, T> func, IEnumerable<V> coll)
    {
        foreach (var v in coll)
            func.Invoke(v);
        return init;
    }

    public static SelectMenuBuilder AddOptions(
        this SelectMenuBuilder builder,
        IEnumerable<SelectMenuOptionBuilder> options) =>
        builder.Reduce(builder.AddOption, options);

    public static ComponentBuilder WithButtons(
        this ComponentBuilder builder,
        IEnumerable<ButtonBuilder> buttons) =>
        builder.Reduce(x => builder.WithButton(x), buttons);
}
