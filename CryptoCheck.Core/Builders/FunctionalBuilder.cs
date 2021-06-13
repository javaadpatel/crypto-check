using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoCheck.Core.Builders
{
    public class FunctionalBuilder<TSubject, TSelf>
        where TSelf: FunctionalBuilder<TSubject, TSelf> //TSelf is the builder's type
        where TSubject : new() //TSubject is the class you're actually building
    {
        //keep a list of funcs to apply to the TSubject
        private readonly List<Func<TSubject, TSubject>> actions = new List<Func<TSubject, TSubject>>();

        public TSelf Do(Action<TSubject> action) => AddAction(action);

        public TSubject Build() => actions.Aggregate(new TSubject(), (p, f) => f(p)); //start with default object and apply all funcs to it

        private TSelf AddAction(Action<TSubject> action)
        {
            actions.Add(p =>
            {
                action(p);
                return p;
            });

            return (TSelf)this;
        }
    }
}
