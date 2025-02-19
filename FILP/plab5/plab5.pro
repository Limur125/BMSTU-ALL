	domains
	list = integer*
	n = integer
	
	predicates
	sumOddPos(list, n).
	sumHelp(list, n, n).
	
	clauses
	sumOddPos(L, Res) :- sumHelp(L, Res, 0).
	sumHelp([], Res, Res).
	sumHelp([_], Res, Res).
	sumHelp([_,H|T], Res, CurSum) :- NewCurSum = CurSum + H, 
	sumHelp(T, Res, NewCurSum).
	
	goal
	sumOddPos([1, 2, 3, 5], Res).