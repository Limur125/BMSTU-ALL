DOMAINS 
	name = string.
	sex = string.
	
PREDICATES
        parent(name, name, sex).
	grandParent(name, name, sex, sex).
	
CLAUSES

	parent(bestla, bolthorn, m).
	parent(odin, borr, m).
	parent(odin, bestla, f).
	parent(frigg, fjorginn, m).
	parent(frigg, fjorgyn, f).
	parent(thor, odin, m).
	parent(thor, jord, f).
	parent(heimdall, odin, m).
	parent(heimdall, nine, f).
	parent(tyr, odin, m).
	parent(baldr, odin, m).
	parent(baldr, frigg, f).
	parent(hed, odin, m).
	parent(hed, frigg, f).
	parent(loki, farabuti, m).
	parent(loki, laufeya, f).
	parent(fenrir, loki, m).
	parent(hel, loki, m).
	parent(jormungandr, loki, m).
	
	grandParent(CName, GPName, PSex, GPSex) :- 
			parent(CName, PName, PSex), 
			parent(PName, GPName, GPSex).
	
		
GOAL
	grandParent(baldr, GrandParent, _, f).
	%grandParent(baldr, GrandParent, _, m).
	%grandParent(baldr, GrandParent, _, _).
	%grandParent(baldr, GrandParent, f, f).
	%grandParent(baldr, GrandParent, f, _).

