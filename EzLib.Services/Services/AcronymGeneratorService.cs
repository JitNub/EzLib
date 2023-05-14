using System.Text;

namespace EzLib.Services.Services
{
    public class AcronymGeneratorService : IAcronymGeneratorService
    {
        public string GenerateAcronym(string title)
        {
            // Check if the title is null, empty or contains only whitespaces
            if (string.IsNullOrWhiteSpace(title))
            {
                // Return the title as is
                return title;
            }

            // Create a StringBuilder to hold the acronym
            var acronym = new StringBuilder();

            // Split the title into words using space as the delimiter
            var words = title.Split(' ');

            // Loop through each word in the title
            foreach (var word in words)
            {
                // Check if the word is not null, empty or contains only whitespaces
                if (!string.IsNullOrWhiteSpace(word))
                {
                    // Append the first letter of the word to the acronym
                    acronym.Append(word[0]);
                }
            }

            // Return the original title and the acronym in parentheses, with the acronym converted to uppercase
            return $"{title} ({acronym.ToString().ToUpper()})";
        }
    }
}
