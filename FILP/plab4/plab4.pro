predicates

factorial(integer, integer).
factHelp(integer, integer, integer).

fibbonachi(integer, integer).
fibHelp(integer, integer, integer, integer).

clauses

factHelp(N, CurA, Res) :- N < 2, Res = CurA, !.

factHelp(N, CurA, Res) :-
	NextA = CurA * N,
	NextN = N - 1,
	factHelp(NextN, NextA, Res).

factorial(N, Res) :-
	factHelp(N, 1, Res).

fibHelp(N, Res, _, PrevA) :- 
N < 3, Res = PrevA, !.

fibHelp(N, Res, PPA, PrevA) :-
NextN = N - 1,
NewPrev = PPA + PrevA,
fibHelp(NextN, Res, PrevA, NewPrev).

fibbonachi(N, Res) :- 
fibHelp(N, Res, 1, 1).

goal
% factorial(1, Answer).
% factorial(2, Answer).
% factorial(3, Answer).
% fibbonachi(1, Answer).
fibbonachi(10, Answer).
