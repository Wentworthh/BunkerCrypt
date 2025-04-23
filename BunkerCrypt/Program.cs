using System.Text;

class BunkerCrypt
{
    static void Main()
    {
        Console.WriteLine("== BunkerCrypt - Simple File Encryption ==");
        Thread.Sleep(700);
        Console.WriteLine("Choose an option below:");
        Console.WriteLine("1. Encrypt File");
        Console.WriteLine("2. Decrypt File");

        int option;
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out option) && (option == 1 || option == 2))
            {
                break;
            }
            Console.WriteLine("Invalid option, try again.");
        }

        string filePath;
        while (true)
        {
            Console.Write("Enter the file path: ");
            filePath = Console.ReadLine()!.Trim('"');

            if (string.IsNullOrWhiteSpace(filePath))
            {
                Console.WriteLine("Path cannot be empty");
                continue;
            }

            var invalidChars = Path.GetInvalidPathChars().Where(c => c != '"').ToArray();
            if (filePath.IndexOfAny(invalidChars) >= 0)
            {
                Console.WriteLine("Path contains illegal characters.");
                continue;
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                continue;
            }

            break;
        }

        int key;
        while (true)
        {
            Console.Write("Enter key: ");
            if (int.TryParse(Console.ReadLine(), out key))
            {
                break;
            }
            Console.WriteLine("Invalid key, it must be a number");
        }

        try
        {
            string text = File.ReadAllText(filePath);
            string result = option == 1 ? CaesarCipher(text, key) : CaesarCipher(text, -key);
            File.WriteAllText(filePath, result);
            Console.WriteLine($"File successfully {(option == 1 ? "encrypted" : "decrypted")}!");
            Console.WriteLine($"Original file has been {(option == 1 ? "encrypted" : "decrypted")} in place.");
        }
        catch (Exception e)
        {
            Console.WriteLine("Error: " + e.Message);
        }
    }

    static string CaesarCipher(string text, int shift)
    {
        shift %= 26;
        var buffer = new StringBuilder(text.Length);

        foreach (char c in text)
        {
            if (char.IsLetter(c))
            {
                char offset = char.IsUpper(c) ? 'A' : 'a';
                char shifted = (char)(((c + shift - offset + 26) % 26) + offset);
                buffer.Append(shifted);
            }
            else
            {
                buffer.Append(c);
            }
        }

        return buffer.ToString();
    }
}