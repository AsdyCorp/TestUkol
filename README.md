# TestUkol

Test task - unity project contains 3 scene. Game started from dcene - main menu, after that choose first or second part of task. 

First task - logical. 
User must insert any count of bets(which will appear after as possible bet in "choose bet" drop down menu), input fields multipliers and chanses must contain non negative numbers with same count(multipliers array must be equall in items count to chances array)

Player can choose bet or percent of his current kredit for bet.

Iterations field can be empty or zero (play till zero in kredit) or some numper of iterations.
Result will be visible in the right scrollwindow. User can export result to result.txt file.

Second task - logical/graphical.
User has start kredit and preinstalled bets, multipliers and chances. With arrow user can controll bet, with red button user can start spin. (user noticed that he win only after spin stop) 

Scenes contains Canvas and Gamemanager object. GameManager object uses fortunemachine manager for spining and checking if users win and for gui controlling with UIManager. 
