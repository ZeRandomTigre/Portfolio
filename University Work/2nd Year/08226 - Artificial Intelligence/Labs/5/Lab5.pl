% Author:
% Date: 05/06/2015

food(apple,fruit).
food(tomato,fruit).
food(lettuce,salad).
food(beef,meat).
food(cucumber,salad).

display_salad_food :- food(Food,salad),
                      write(Food), write(' is a salad'), nl, fail.