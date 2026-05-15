namespace CourseProject.Utilities
{
    /// <summary>
    /// Utility class for console input validation.
    /// </summary>
    public static class ConsoleHelper
    {
        public static string ReadNonEmpty(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                var input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrEmpty(input)) return input;
                Console.WriteLine("  Input cannot be empty. Try again.");
            }
        }

        public static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine()?.Trim(), out int value) && value >= min && value <= max)
                return value;
                Console.WriteLine($"  Please enter a valid number ({min}-{max}).");
            }
        }

        public static int ReadMenuChoice(int min, int max)
        {
            return ReadInt($"  Select option ({min}-{max}): ", min, max);
        }

        /// <summary>
        /// Reads a GUID from console input. Returns Guid.Empty if user cancels.
        /// User can type 'cancel' or 'c' to cancel.
        /// </summary>
        public static Guid ReadGuid(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(" (type 'c' to cancel): ");
                Console.ResetColor();

                var input = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("  Input cannot be empty. Try again.");
                    continue;
                }

                // Check for cancel command
                if (input.Equals("c", StringComparison.OrdinalIgnoreCase) || 
                    input.Equals("cancel", StringComparison.OrdinalIgnoreCase))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("  Operation cancelled.");
                    Console.ResetColor();
                    return Guid.Empty;
                }

                if (Guid.TryParse(input, out Guid result))
                    return result;

                Console.WriteLine("  Invalid ID format. Try again.");
            }
        }
    }
}
