using System.Collections.Generic;
using System.Text.Json;
using LAB_4.DAL.Models;

namespace LAB_4.DAL.Repositories
{
    public class GenealogyTreeRepository
    {
        private readonly string _filePath = "genealogyTree.json"; 

        public List<Person> People { get; private set; }
        public List<Relationship> Relationships { get; private set; }

        public GenealogyTreeRepository()
        {
            People = new List<Person>();
            Relationships = new List<Relationship>();
            LoadData();
        }

        public void SaveData()
        {
            var data = new
            {
                People = People,
                Relationships = Relationships
            };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(data));
        }

        private void LoadData()
        {
            if (!File.Exists(_filePath)) return;

            var json = File.ReadAllText(_filePath);
            var data = JsonSerializer.Deserialize<DynamicData>(json);
            if (data != null)
            {
                People = data.People ?? new List<Person>();
                Relationships = data.Relationships ?? new List<Relationship>();
            }
        }

        private class DynamicData
        {
            public List<Person>? People { get; set; }
            public List<Relationship>? Relationships { get; set; }
        }
    }
}
