using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class Node(string value, Node? leftNode = null, Node? rightNode = null)
    {
        public string value = value;
        public Node? leftChild = leftNode;
        public Node? rightChild = rightNode;

        public NFA createNFA()
        {
            NFA? leftNFA = null, rightNFA = null;
            if (leftChild is not null)
                leftNFA = leftChild.createNFA();
            if (rightChild is not null)
                rightNFA = rightChild.createNFA();
            return _mergeNFA(leftNFA, rightNFA);
        }

        public virtual NFA _mergeNFA(NFA leftNFA, NFA rightNFA)
        {
            var nfa = new NFA();
            Guid state1 = Guid.NewGuid(), state2 = Guid.NewGuid();
            nfa.add(state1.ToString(), state2.ToString(), value);


            nfa.startState = state1.ToString();
            nfa.setFinishState(state2.ToString());


            return nfa;
        }
    }

    public class NodeOperator(string value, Node? leftNode = null, Node? rightNode = null) : Node(value, leftNode, rightNode)
    {

    }

    public class NodeStar(Node? leftNode = null, Node? rightNode = null) : Node(Consts.starSymbol, leftNode, rightNode)
    {
        public override NFA _mergeNFA(NFA leftNFA, NFA rightNFA)
        {
            var nfa = new NFA();

            foreach (var elem in leftNFA.stateDict)
                nfa.stateDict = nfa.stateDict.Append(elem).ToDictionary();

            string state1 = Guid.NewGuid().ToString(), state2 = Guid.NewGuid().ToString();

            nfa.add(state1, leftNFA.startState, Consts.epsSymbol);
            nfa.add(state1, state2, Consts.epsSymbol);
            foreach (var finishState in leftNFA.finishState)
            {
                nfa.add(finishState, leftNFA.startState, Consts.epsSymbol);
                nfa.add(finishState, state2, Consts.epsSymbol);
            }

            nfa.startState = state1;
            nfa.setFinishState(state2);

            return nfa;
        }
    }

    public class NodePlus(Node? leftNode = null, Node? rightNode = null) : Node(Consts.plusSymbol, leftNode, rightNode)
    {
        public override NFA _mergeNFA(NFA leftNFA, NFA rightNFA)
        {
            var nfa = new NFA();

            foreach (var elem in leftNFA.stateDict)
                nfa.stateDict = nfa.stateDict.Append(elem).ToDictionary();

            string state1 = Guid.NewGuid().ToString(), state2 = Guid.NewGuid().ToString();

            nfa.add(state1, leftNFA.startState, Consts.epsSymbol);
            foreach (var finishState in leftNFA.finishState)
            {
                nfa.add(finishState, leftNFA.startState, Consts.epsSymbol);
                nfa.add(finishState, state2, Consts.epsSymbol);
            }

            nfa.startState = state1;
            nfa.setFinishState(state2);

            return nfa;
        }
    }

    public class NodeOr(Node ? leftNode = null, Node ? rightNode = null) : Node(Consts.orSymbol, leftNode, rightNode)
    {
        public override NFA _mergeNFA(NFA leftNFA, NFA rightNFA)
        {
            var nfa = new NFA();

            foreach (var elem in leftNFA.stateDict)
                nfa.stateDict = nfa.stateDict.Append(elem).ToDictionary();
            foreach (var elem in rightNFA.stateDict)
                nfa.stateDict = nfa.stateDict.Append(elem).ToDictionary();

            string state1 = Guid.NewGuid().ToString(), state2 = Guid.NewGuid().ToString();

            nfa.add(state1, leftNFA.startState, Consts.epsSymbol);
            nfa.add(state1, rightNFA.startState, Consts.epsSymbol);

            foreach (var finishState in leftNFA.finishState)
                nfa.add(finishState, state2, Consts.epsSymbol);
            foreach (var finishState in rightNFA.finishState)
                nfa.add(finishState, state2, Consts.epsSymbol);

            nfa.startState = state1;
            nfa.setFinishState(state2);

            return nfa;
        }
    }

    public class NodeAnd(Node? leftNode = null, Node? rightNode = null) : Node(Consts.andSymbol, leftNode, rightNode)
    {
        public override NFA _mergeNFA(NFA leftNFA, NFA rightNFA)
        {
            var nfa = new NFA();

            foreach (var elem in leftNFA.stateDict)
                nfa.stateDict = nfa.stateDict.Append(elem).ToDictionary();
            foreach (var elem in rightNFA.stateDict)
                nfa.stateDict = nfa.stateDict.Append(elem).ToDictionary();

            foreach (var finishState in leftNFA.finishState)
                nfa.add(finishState, rightNFA.startState, Consts.epsSymbol);

            nfa.startState = leftNFA.startState;
            foreach (var finishState in rightNFA.finishState)
                nfa.setFinishState(finishState);

            return nfa;
        }
    }
}
