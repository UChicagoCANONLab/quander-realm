namespace DependencySystem.Models
{
    using System.Collections;
    using System.Collections.Generic;

    public class Question
    {
        public string Name { get; }

        public Question(string name) => Name = name;

        public bool Matches(Question other) =>
            Name.Split('-')[0] == other.Name.Split('-')[0];

        public override bool Equals(object obj) => obj is Question e && e.Name == Name;
        public override int GetHashCode() => Name.GetHashCode();
        public override string ToString() => Name;
    }
}