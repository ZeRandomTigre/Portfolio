% Author:
% Date: 05/06/2015
/*
go :- write('Enter you Name: '),
      read(Yourname),
      write('Hello ') , write(Yourname), nl,
      
      write('Enter a month as an integer: '),
      read(Month),
      write('The Month is '), month(Month), nl.
      
month(1) :- write('January').
month(2) :- write('February').
month(3) :- write('March').
month(4) :- write('April').
month(5) :- write('May').
month(6) :- write('June').
month(7) :- write('July').
month(8) :- write('August').
month(9) :- write('September').
month(10) :- write('October').
month(11) :- write('November').
month(12) :- write('December').
month(_) :- write('Invalid Value').
*/

translate(you,vous).
translate(i,je).
translate(the,le).
translate(house,maison).
translate(now,maintenant).
translate(_,'Not Known').

go :- write('Enter an English word: '),
       read(English), nl,
       translate(English,French),
       write('In French, the word '), write(English),
       write(' is '), write(French).

