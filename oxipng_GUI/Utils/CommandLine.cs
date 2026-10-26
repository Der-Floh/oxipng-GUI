using System.Text;

namespace OxipngGui.Utils;

/// <summary>
/// Builds a Windows command line that the MSVC argument rules (which Rust's parser follows too) split back into
/// exactly the given arguments.
/// </summary>
public static class CommandLine
{
    private static readonly char[] CharsRequiringQuotes = [' ', '\t', '\n', '\v', '"'];

    public static string Join(IEnumerable<string> arguments)
    {
        if (arguments is null)
            throw new ArgumentNullException(nameof(arguments));

        return string.Join(" ", arguments.Select(Quote));
    }

    public static string Quote(string argument)
    {
        if (argument is null)
            throw new ArgumentNullException(nameof(argument));

        if (argument.Length > 0 && argument.IndexOfAny(CharsRequiringQuotes) < 0)
            return argument;

        var quoted = new StringBuilder(argument.Length + 2).Append('"');
        var backslashes = 0;
        foreach (var c in argument)
        {
            if (c == '\\')
            {
                backslashes++;
                continue;
            }

            quoted.Append('\\', c == '"' ? backslashes * 2 + 1 : backslashes).Append(c);
            backslashes = 0;
        }

        return quoted.Append('\\', backslashes * 2).Append('"').ToString();
    }
}
