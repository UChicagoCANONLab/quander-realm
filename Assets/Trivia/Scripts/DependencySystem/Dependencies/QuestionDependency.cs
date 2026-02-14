
namespace DependencySystem.Dependencies
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using DependencySystem.Models;

public class QuestionDependency : IDependency
{
    public Question Target { get; }

    public QuestionDependency(Question target)
    {
        Target = target;
    }

    public bool IsSatisfied(HashSet<Question> completed)
    {
        return completed.Any(e => e.Matches(Target));
    }

    public override string ToString() => Target.Name;
}

}