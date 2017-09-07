% Author:
% Date: 06/06/2015

likes(darren,beer).
likes(steve,running).
likes(steve,programming).
likes(paul,football).
likes(dawn,shopping).
likes(paul,programming).

/* findall(Name,  likes(Name,Activity),  List). */

mymember( Item, [Item | _ ] ).

mymember(Item,  [_|Rest])  :-  mymember(Item,  Rest).

myappend([],List,List).

myappend([Head|Rest],  List,  [Head|Out])  :-  myappend(Rest,  List,  Out).

mylast([Head|[]],  Head).

mylast([_|Rest],  Answer)  :-  mylast(Rest,  Answer).