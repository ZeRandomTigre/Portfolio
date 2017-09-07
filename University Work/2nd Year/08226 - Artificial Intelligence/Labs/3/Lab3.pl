% Author:
% Date: 05/12/2014

/* Weather facts */

temperature(high).
cloud(low).
visibility(high).

/* Weather Rules */

weather(bad) :-
               temperature(low),
               cloud(high),
               visibility(low).
weather(good):-
               temperature(high),
               cloud(low),
               visibility(high).
weather(unsure).
             


