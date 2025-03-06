using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1
{
    public class NFA
    {
        public Dictionary<string, Dictionary<string, List<string>>> stateDict;
        public string? startState { get; set; }
        public List<string> finishState;
        public NFA()
        {
            stateDict = [];
            finishState = [];
        }

        public void add(string startState, string finishState, string transition)
        {
            if (!stateDict.ContainsKey(startState))
            {
                stateDict[startState] = [];
                stateDict[startState][transition] = [finishState];
            }
            else if (!stateDict[startState].ContainsKey(transition))
            {
                stateDict[startState][transition] = [finishState];
            }
            else
            {
                stateDict[startState][transition].Add(finishState);
            }
        }

        public void setFinishState(string finishState)
        {
            this.finishState.Add(finishState);
        }

        public Dictionary<string, List<string>> getGoal(string state)
        {
            return stateDict[state];
        }

        public List<string> getGoalSetBySign(string state, string sign) 
        {
            return stateDict.GetValueOrDefault(state, []).GetValueOrDefault(sign, []).Distinct().ToList();
        }

        public List<string> popFinishState()
        {
            var finishStateArr = finishState;
            finishState = [];
            return finishStateArr;
        }

        public List<string> getAlphabet()
        {
            List<string> signArr = [];
            foreach (var valueDict in stateDict.Values)
                signArr.AddRange(valueDict.Keys);
            return signArr.Distinct().ToList();
        }
    }
}
