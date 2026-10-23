using System.Collections.Generic;
using DependencySystem.Models;

namespace DependencySystem.Dependencies
{
    public interface IDependency
    {
        bool IsSatisfied(HashSet<Question> completed);
    }
}