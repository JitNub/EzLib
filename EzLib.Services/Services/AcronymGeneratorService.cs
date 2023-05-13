using System.Text;

namespace EzLib.Services.Services
{
    public class AcronymGeneratorService : IAcronymGeneratorService
    {
        public string GenerateAcronym(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                return title;
            }

            var acronym = new StringBuilder();
            var words = title.Split(' ');

            foreach (var word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    acronym.Append(word[0]);
                }
            }

            return $"{title} ({acronym.ToString().ToUpper()})";
        }
    }
}
