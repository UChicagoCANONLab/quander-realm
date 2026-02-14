using System.Collections.Generic;
using System.Linq;
using DependencySystem.Models;

namespace DependencySystem.Dependencies
{
    public class AndDependency : IDependency
    {
        private readonly List<IDependency> _children;

        public AndDependency(params IDependency[] dependencies)
        {
            _children = dependencies.ToList();
        }

        public AndDependency(List<IDependency> dependencies)
        {
            _children = dependencies;
        }

        public bool IsSatisfied(HashSet<Question> completed)
        {
            // ✅ Works if using the correct IDependency
            return _children.All(d => d.IsSatisfied(completed));
        }
    }
}