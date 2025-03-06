using System.Diagnostics;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;

namespace lab1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string regex = "(b+|c)*a";
            var parser = new RegexParser();
            var resTree = parser.parseExpression(regex);
            var nfa = resTree.createNFA();
            var nodes = nfa.stateDict.Keys.ToList();
            nodes.AddRange(nfa.finishState);
            using StreamWriter file = new StreamWriter("graph.dot");
            file.WriteLine("digraph nfa {");
            file.WriteLine($"999999 [label=\"{regex}\" peripheries=0 shape=\"box\"];");
            file.WriteLine($"999999 -> {nodes.IndexOf(nfa.startState)}");
            int i = 1;
            foreach (var finNode in nfa.finishState)
            {
                file.WriteLine($"{999999 + i} [style=invis];");
                file.WriteLine($"{nodes.IndexOf(finNode)} -> {999999 + i}");
            }

            foreach (var keyValuePair in nfa.stateDict)
                foreach (var finKeyValuePair in keyValuePair.Value)
                    foreach(var trans in finKeyValuePair.Value)
                        file.WriteLine($"{nodes.IndexOf(keyValuePair.Key)} -> {nodes.IndexOf(trans)} [label=\"{finKeyValuePair.Key}\"];");
            file.WriteLine("}");
        }
    }
}
