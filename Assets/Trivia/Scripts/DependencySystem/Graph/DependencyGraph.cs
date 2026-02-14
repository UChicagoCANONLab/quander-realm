using System.Collections.Generic;
using System.Linq;
using DependencySystem.Models;
using DependencySystem.Dependencies;

namespace DependencySystem.Graph
{
    public class DependencyGraph
    {
        private readonly Dictionary<Question, IDependency> _dependencies;

        public DependencyGraph(Dictionary<Question, IDependency> dependencies)
        {
            _dependencies = dependencies;
        }

        public List<Question> Next(HashSet<Question> completed)
        {
            return _dependencies.Keys
                .Where(e => !completed.Contains(e))
                .Where(e => _dependencies[e].IsSatisfied(completed)) 
                .ToList();
        }
    }
}