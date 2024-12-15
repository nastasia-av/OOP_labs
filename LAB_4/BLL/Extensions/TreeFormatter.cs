using System.Collections.Generic;
using System.Linq;
using System.Text;
using LAB_4.DAL.Models;

namespace LAB_4.BLL.Extensions
{
    public static class TreeFormatter
    {
        public static string FormatTree(List<Person> people, List<Relationship> relationships)
        {
            var result = new StringBuilder();

            foreach (var person in people)
            {
                result.AppendLine(person.FullName);
                var children = relationships
                    .Where(r => r.PersonId == person.Id && r.RelationType == "Ребенок")
                    .Select(r => r.RelatedPersonId)
                    .Select(id => people.FirstOrDefault(p => p.Id == id)?.FullName);

                if (children.Any())
                {
                    result.AppendLine("  |");
                    result.AppendLine(string.Join(" | ", children));
                }
            }

            return result.ToString();
        }
    }
}
