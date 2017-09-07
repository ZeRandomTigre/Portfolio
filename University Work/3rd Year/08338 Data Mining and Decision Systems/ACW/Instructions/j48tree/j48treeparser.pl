% Display Lists in FULL
%  working_directory(P,'h:/08338-dmds/trees').
:- set_prolog_flag(toplevel_print_options,[quoted(true), portray(true)]).

:-dynamic(rulenumber/1).
%:-dynamic( firstrule/0 ).
:-dynamic(rule/2).
:-dynamic(rule/3).
:-dynamic(dss/2).

rulenumber(0).
% top level predicate
tree:-
	retract( rulenumber(_X) ),
	assert( rulenumber( 0 ) ),
%	assert( firstrule ), !,
	open('j48.txt', read, FP),
	readtree(FP, [], []),
	close(FP), !,
	rulenumber(Now),
	interim_rules(Now), !,
	open('j48-rules.csv', write, FPx),
	write_out_rules(Now, 1, FPx),
	close(FPx), !,
	open('j48-dss-rules.csv', write, FPxx),
	write_dss_rules(1, Now, FPxx),
	close(FPxx).

% clean of bar seperators and note how many at start of rule in rule/3
interim_rules(1):-
	clean_of_trailing_bars(1).
interim_rules(N):-
	clean_of_trailing_bars(N),
	M is N-1,
	interim_rules(M),
	!.
clean_of_trailing_bars(N):-
	rule(N, Rule) , !,
	clean_of_barsN(X, Rule, NewRule ),
	assert( rule(N, X, NewRule) ), !.
clean_of_barsN(0,[], []).
clean_of_barsN(N,[bar|Rest], CleanRule):-
	clean_of_barsN(X,Rest, CleanRule ),
	N is X+1.
clean_of_barsN(0,Rest, CleanRule):-
	clean_of_bars(Rest, CleanRule ).
clean_of_bars([], []).
clean_of_bars([bar|Rest], CleanRule):-
	clean_of_bars(Rest, CleanRule ).
clean_of_bars([Att|Rest], [Att|CleanRule]):-
	clean_of_bars(Rest, CleanRule ).

% take dss rules and write with conclusion first
write_dss_rules(N, Max, _FP):-
	N > Max,
	!.
write_dss_rules(N, Max, FP):-
	dss(N, Rule),
	write(FP, 'j48-'), write(FP, N),
	reverse(Rule, [Outcome, Comp, Label | Reversed]),
	write(FP,', '),
	write(FP, Label), write(FP, ' '),
	write(FP, Comp), write(FP, ' '),
	write(FP, Outcome),
	reverse(Reversed, Abbreviated),
	write_format_rule(FP, Abbreviated),
	nl(FP),
	M is N+1,	!,
	write_dss_rules(M, Max, FP).

% take interim rules and add any missing part to the start
write_out_rules(Max, N, _FP):-
	N > Max,
	!.
write_out_rules(Max, N, FP):-
	rule(N, X, Rule),
	build_first_part(X, N, Rule, BuiltRule),
	write(FP, 'j48-'), write(FP, N),
	write_format_rule(FP, BuiltRule),
	nl(FP),
	M is N+1,
	assert( dss(N, BuiltRule) ),
	!,
	write_out_rules(Max, M, FP).

% format rules output so good for pasting into Excel!
write_format_rule(_FP, []):-
	!.
write_format_rule(FP, [Att, Comp, Value | BuiltRule]):-
	write(FP,', '),
	write(FP, Att), write(FP, ' '),
	write(FP, Comp), write(FP, ' '),
	write(FP, Value),
	write_format_rule(FP,BuiltRule), !.

% add any missing part to the start
build_first_part(0, _RN, Rule, Rule):- !.
build_first_part(_X, 1, Rule, Rule):- !.
build_first_part(X, N, Rule, BuiltRule):-
	leading_bars(X, N, Rule, BuiltRule), !.

% NO leading bras left to find
leading_bars(0, _RN, Rule, Rule):- !.
leading_bars(_X, 1, Rule, Rule):- !.
% earlier rule has same number of bars
leading_bars(X, RN, Rule, BuiltRule):-
	RM is RN-1,
	rule(RM, X, _EarlierRule), !,
	leading_bars(X, RM, Rule, BuiltRule), !.
% earlier rule has more missing bars!
leading_bars(X, RN, Rule, BuiltRule):-
	RM is RN-1,
	rule(RM, Y, _EarlierRule),
	X < Y, !,
	leading_bars(X, RM, Rule, BuiltRule), !.
% Difference says how many attribute value triples required
leading_bars(X, RN, CleanRule, BuiltRule):-
	RM is RN-1,
	rule(RM, Y, EarlierRule),
	Parts is X - Y,
	earlier_rule_part(Parts, EarlierRule, EarlierPart1),
	leading_bars(Y, RM, EarlierPart1, EarlierPart),
	append(EarlierPart, CleanRule, BuiltRule), !.
earlier_rule_part(0, _EarlierRule, []):- !.
earlier_rule_part(Parts, [Att, Comp, Value | EarlierRule], EarlierPart):-
	NP is Parts-1,
	earlier_rule_part(NP, EarlierRule, EarlierPartBit),
	append([Att, Comp, Value], EarlierPartBit, EarlierPart).

% lexical parsing of source file
readtree(FP, CharList, Rules):-
	peek_char(FP, C),
	not(C == end_of_file), !,
	get_char(FP, C),
	lex_parser(FP, C, CharList, ReturnChars, Rules, ReturnRules), !,
	readtree(FP, ReturnChars, ReturnRules).
readtree(FP, [], RuleList):-
	peek_char(FP, C),
	C == end_of_file, !,
%	writeln('here'), trace,
%	writeln(RuleList),
	reverse(RuleList , Rules),
	write_rules(Rules, []), !.
readtree(_FP, CharList, RuleList):-
	reverse(CharList, CharListR),
	atom_chars(Attribute, CharListR),
	writeln(Attribute), !,
	reverse([Attribute | RuleList ], Rules),
	write_rules(Rules, []), !.

lex_parser(_FP, ' ', [], [], Rules, Rules):-
	!.
lex_parser(_FP, ' ', Charlist, [], Rules, [Atom|Rules]):-
	atom_chars(Atom, Charlist),
	!.
lex_parser(_FP, '\n', [], [], Rules, Rules):-
	!.
lex_parser(_FP, '\n', Charlist, [], Rules, [Atom | Rules]):-
	atom_chars(Atom, Charlist),
	!.
lex_parser(_FP, '\t', [], [], Rules, Rules):-
	!.
lex_parser(_FP, '\t', Charlist, [], Rules, [Atom | Rules]):-
	atom_chars(Atom, Charlist),
	!.
lex_parser(_FP, '|', [], [], Rules, [bar | Rules]):-
	!.
lex_parser(_FP, '|', Charlist, [], Rules, [bar, Atom|Rules]):-
	atom_chars(Atom, Charlist),
	!.
% if ( skip until newline
lex_parser(FP, '(', [], [], Rules, Rules):-
	skip_until_newline(FP),
	!.
lex_parser(FP, '(', Charlist, [], Rules, [Atom|Rules]):-
	skip_until_newline(FP),
	atom_chars(Atom, Charlist),
	!.
% build comparator
lex_parser(FP, C, [], [], Rules, [Atom | Rules]):-
	member(C,['!', '>', '=', '<']), !,
	build_until_space(FP, NewCharList),
	atom_chars(Atom, [C | NewCharList]),
	!.
lex_parser(FP, C, Charlist, [], Rules, [Atom, Atom1|Rules]):-
	member(C,['!', '>', '=', '<']), !,
	build_until_space(FP, NewCharList),
	atom_chars(Atom, [C | NewCharList]),
	atom_chars(Atom1, Charlist),
	!.
% first character is number so must be number
lex_parser(FP, C, [], [], Rules, [Number | Rules]):-
	member(C,['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']), !,
	build_number(FP, NewCharList),
	atom_chars(Number, [C | NewCharList]),
	!.
lex_parser(FP, C, Charlist, [], Rules, [Number, Atom | Rules]):-
	member(C,['0', '1', '2', '3', '4', '5', '6', '7', '8', '9']), !,
	build_number(FP, NewCharList),
	atom_chars(Number, [C | NewCharList]),
	atom_chars(Atom, Charlist),
	!.
lex_parser(FP, ':', [], [], Rules, [Outcome, '=', 'risk' | Rules]):-
	get_char(FP, ' '), !,
	build_until_space(FP, NewCharList),
%	writeln('ghvghxc@'), writeln(NewCharList),
	atom_chars(Outcome, NewCharList),
	!.
lex_parser(FP, ':', Charlist, [], Rules, [Outcome, '=', 'risk', Atom | Rules]):-
	build_until_space(FP, NewCharList),
	atom_chars(Outcome, NewCharList),
	atom_chars(Atom, Charlist),
%	writeln('xc@'), writeln(NewCharList),
	!.
% all other characters
lex_parser(FP, C, [], [], Rules, [Atom | Rules]):-
	build_until_space(FP, NewCharList),
	atom_chars(Atom, [C | NewCharList]),
	!.
lex_parser(FP, C, Charlist, [], Rules, [Atom, Atom1 | Rules]):-
	build_until_space(FP, NewCharList),
	atom_chars(Atom, [C | NewCharList]),
	atom_chars(Atom1, Charlist),
	!.
build_number(FP, [C | NewCharList]):-
	peek_char(FP, C),
	not(C == end_of_file),
	not(C == ' '),
	not(C=='\n'),
	not(C=='\t'),
	not(C==':'),
	get_char(FP, C),
	build_number(FP, NewCharList),
	!.
build_number(_FP, []):-
	!.
build_until_space(FP, [C | NewCharList]):-
	peek_char(FP, C),
	not(C == end_of_file),
	not(C == ' '),
	not(C == '\n'),
	not(C == '\t'),
	not(C == ':'),
	!,
	get_char(FP, C),
	build_until_space(FP, NewCharList),
	!.
build_until_space(_FP, [ ]):-
	!.

skip_until_newline(FP):-
	peek_char(FP, C),
	not(C == end_of_file),
	not(C == '\n'), !,
	get_char(FP, C),
	skip_until_newline(FP),
	!.
skip_until_newline(_FP):-
	!.

write_rules([], _LIST):-
	!.
/*
write_rules(['<', '='| Rules], LIST):-
%	write(' <= '),
	append( LIST, ['<='], RULE),
	write_rules(Rules, RULE), !.
*/
write_rules(['risk', '=', Risk | Rules], LIST):-
%	write(' risk = '),
%	writeln(Risk),
	append( LIST, ['risk', '=', Risk], RULE),
	retract( rulenumber(N1) ),
	N2 is N1+1,
	assert( rulenumber(N2) ),
	assert( rule( N2, RULE) ), !,
	write_rules(Rules, []), !.
/*
write_rules(['(' | Rules], LIST):-
	discard_rule_part(Rules, LIST), !.
*/
write_rules([ Item | Rules], LIST):-
%	write(Item),
%	write(' '),
	append(LIST, [Item], Rule),
	write_rules(Rules, Rule), !.
/*
discard_rule_part([')'| Rules], List):-
	write_rules(Rules, List), !.
discard_rule_part([ _ITEM | Rules], List):-
	discard_rule_part(Rules, List), !.
*/
run:-
	tree, halt.

%eof




