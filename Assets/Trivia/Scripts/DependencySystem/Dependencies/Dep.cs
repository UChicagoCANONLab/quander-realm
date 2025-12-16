using System.Collections.Generic;
using System.Linq;
using DependencySystem.Models;

namespace DependencySystem.Dependencies
{
    public static class Dep
    {
        public static IDependency E(string name) => new QuestionDependency(new Question(name));
        public static IDependency And(params IDependency[] deps) => new AndDependency(deps.ToList());
        public static IDependency Or(params IDependency[] deps) => new OrDependency(deps.ToList());
    }
}