% Author:
% Date: 06/06/2015

writelist([]).

writelist([Head|Rest]) :- write(Head), nl, writelist(Rest).
