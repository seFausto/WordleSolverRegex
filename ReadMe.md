# Wordle Solver Regex, Now with Nerdle Solver

## Description

I had to implement a solver as it's the cool thing to do right now. I believe that using Regex would be the easiest option because I can have a first pattern that can be any combination of any 5 letters, and as I get more clues by using Green, Yellow, or Gray letters, I adjust the regex to match that.

The regex is combined from 5 individual patterns that contain all letters of the English alphabet. Each pattern corresponds to a solution letter.

So, if a letter shows green on the third box on Wordle (represented by the number 2) I know that box three should be that letter, so the regex pattern is replaced by the individual letter.

If a letter shows yellow on the second, I know that it exists in another box, so I can remove that letter as an option for the pattern for the second letter.

If a letter is gray, I know that it doesn't exist at all on the solution.

### Wordle Solution
Wordle has a default RegEx and as more information is known, the application updates the RegEx string with the new facts.

### Nerdle Solution
On Nerdle, the equations are built with the available information. The first step is to get all combinations available, then filter out by different rules (must have only one "=", or cannot start or end with a symbol), then both sides of the equation must compute, finally, they have to match.

Because building all possible solutions with no facts can take a long time (it's 2 ^ 10 + 6 ^ 14 possibilities), The first two suggestions are hardcoded, with this new information, the number of possibilities drops drastically and will make processing a lot faster.

## About the Code

I used the strategy pattern, the user has to choose either Worlde Or Nerdle by inputting W or N. This will choose the strategy to use.


## Acknowledgement

The console application was inspired by https://github.com/LeoTheBestCoder/wordle-solver
