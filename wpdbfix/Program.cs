using System;

namespace WpDbFix
{
    // https://wpengine.com/support/wordpress-serialized-data/
    // https://www.w3schools.com/php/phptryit.asp?filename=tryphp_func_var_serialize
    // https://www.php.net/manual/en/language.types.string.php#language.types.string.syntax.single

    public static class Program
    {
        private static int Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("WP DB FIX");
            Console.ResetColor();

            if (args.Length != 3)
            {
                Console.WriteLine(
                    "wpdbfix <ReplacementsPath> <InputPath> <OutputPath>");
                return 1;
            }

            try
            {
                Console.WriteLine(
                    $"Replacements: {args[0]}\n" +
                    $"Input: {args[1]}\n" +
                    $"Output: {args[2]}\n");

                int count = ReplaceCommand.Execute(args[0], args[1], args[2]);
                Console.WriteLine($"Replaced: {count}");
                return 0;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
                Console.WriteLine(ex.ToString());
                return 2;
            }
        }
    }
}
