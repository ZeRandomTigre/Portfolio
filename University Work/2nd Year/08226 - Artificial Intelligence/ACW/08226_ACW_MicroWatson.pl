% Author:
% Date: 11/06/2015
/*
S -> NP VP      e.g. John loves Mary
S -> VP         e.g. Go!
NP -> Det NP2
NP -> NP2
NP -> NP PP
NP2 -> Noun
NP2 -> Adj NP2
PP -> Prep NP
VP -> Verb
VP -> Verb NP
VP -> VP PP

NP = Noun Phrase
VP = Verb Phrase
Det = Determiner
PP = Prepositional Phrase
*/

% take a sentence and parse into noun phrase and verb phrase
sentence(Sentence,sentence(np(Noun_Phrase),vp(Verb_Phrase)),Rem):-    
    np(Sentence,Noun_Phrase,Rem),
    vp(Rem,Verb_Phrase).	
        
np([X|T],np(det(X),NP2),Rem):-
    det(X),
    np2(T,NP2,Rem).
        
np(Sentence,Parse,Rem):-
    np2(Sentence,Parse,Rem).

np(Sentence,np(NP,PP),Rem):-
    np(Sentence,NP,Rem1),
    pp(Rem1,PP,Rem).
        
np2([H|T],np2(noun(H)),T):-
    noun(H).
        
np2([H|T],np2(adj(H),Rest),Rem):-
    adj(H),np2(T,Rest,Rem).
        
pp([H|T],pp(prep(H),Parse),Rem):-
    prep(H),
    np(T,Parse,Rem).
        
vp([H|[]],verb(H)):-
    verb(H).
        
vp([H|Rest],vp(verb(H),RestParsed)):-
    verb(H),
    pp(Rest,RestParsed,_).
        
vp([H|Rest],vp(verb(H),RestParsed)):-
    verb(H),
    np(Rest,RestParsed,_).
        
get_Element(X,[X|_]).

get_Element(X,[_|T]):-
    get_Element(X,T).

det(the).
det(a).

adj(paranoid).
adj(young).
adj(middle_aged).
adj(magic).
adj(faithful).

prep(on).
prep(by).

verb(is).
verb(finds).
verb(destroys).
verb(saves).
verb(was).
verb(expelled).

adverb(quietly).

noun(butler).
noun(hobbit).
noun(ring).
noun(day).
noun(valet).
noun(holden).
noun(robot).
noun(marvin).

micro_watson :- 
    write('Welcome to the Micro Watson 2015 game of Jeopardy!'),
    nl,
    write('The categories are -'),
    nl,
    write('1. Novels/Films of the 20th Century'),
    nl,
    write('2. Novels of the 19th Century'),
    nl,
    write('3. Novels of the 18th Century'),
    nl,
    write('Please select a catergory: '),
    read(Catergory),
    catergory(Catergory).
                
catergory(Catergory) :-
    questionChoice(Catergory).

questionChoice(1) :-
    write('You have picked catergory 1 - "Novels/Films of the 20th Century"'),
    nl,
    write('Please give me an answer'),
    nl,
    write('micro_watson: '),
    micro_watson_answer.

questionChoice(2) :-
    write('Under construction - please pick another catergory.'),
    nl,
    read(Catergory),
    catergory(Catergory).

questionChoice(3) :-
    write('Under construction - please pick another catergory.'),
    nl,
    read(Catergory),
    catergory(Catergory).

micro_watson_answer :-
    read(Sentence),
    sentence(Sentence,sentence(np(Noun_Phrase),vp(Verb_Phrase)),Verb_Phrase_List),
    write('sentence('),nl,
    write('    '),write(Noun_Phrase),write(','),nl,
    write('    '),write(Verb_Phrase),write(').'),nl,nl,
	parse_sov(Sentence,Verb_Phrase_List, Parse_sov_List),
    find_match(Parse_sov_List, BookName),
	write(BookName).

parse_sov(Sentence, Verb_Phrase_List, Parse_sov_List) :-
    % first noun in sentence
	get_Element(Subject,Sentence),
	noun(Subject),
	% first noun from verb phrase
	get_Element(Object,Verb_Phrase_List),
	noun(Object),
	% verb from verb phrase
	get_Element(Verb,Verb_Phrase_List),
	verb(Verb),
    Parse_sov_List = [subject(Subject), object(Object), verb(Verb)].

find_match(Parse_sov_List, BookName) :-
    plot_story(Parse_sov_List, BookName).

plot_story( [subject(hobbit), object(ring), verb(finds)], 'The Hobbit' ).
plot_story( [subject(hobbit), object(ring), verb(destroys)], 'The Lord of the Rings' ).
plot_story( [subject(valet), object(day), verb(saves)], 'Thankyou Jeeves').
plot_story( [subject(holden), object(expelled), verb(is)], 'The Catcher in the Rye').
plot_story( [subject(robot), object(marvin), verb(was)], 'The Hitchhikers Guide to the Galaxy').