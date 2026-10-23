using System.Collections.Generic;
using System.Linq;
using DependencySystem.Models;

namespace DependencySystem.Dependencies
{
    public class OrDependency : IDependency
    {
        public List<IDependency> Children { get; }

        // Overloads for up to 5 arguments
        public OrDependency() => Children = new List<IDependency>();
        public OrDependency(IDependency a) => Children = new List<IDependency> { a };
        public OrDependency(IDependency a, IDependency b) => Children = new List<IDependency> { a, b };
        public OrDependency(IDependency a, IDependency b, IDependency c) => Children = new List<IDependency> { a, b, c };
        public OrDependency(IDependency a, IDependency b, IDependency c, IDependency d) => Children = new List<IDependency> { a, b, c, d };
        public OrDependency(IDependency a, IDependency b, IDependency c, IDependency d, IDependency e) => Children = new List<IDependency> { a, b, c, d, e };
        public OrDependency(List<IDependency> deps) => Children = deps;

        public bool IsSatisfied(HashSet<Question> completed) =>
            Children.Count == 0 || Children.Any(dep => dep.IsSatisfied(completed));

        public override string ToString() => "OR(" + string.Join(", ", Children) + ")";
    }
}