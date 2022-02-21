# Wordle Solver Regex

## Description

I had to implement a solver as it's the cool thing to do right now. I though that using Regex would be the easiest options because I can have a first pattern that can be any combination of any 5 letters, and as I get more clues by using Green, Yellow or Gray letters, I adjust the regex to match that.

The regex is combined from 5 individual patterns that contain all letters of the english alphabet. Each individual pattern corresponds to a solution letter.

So, if a letter shows green on the third box on Wordle (represented by the number 2) I know that box three should be that letter, so the regex pattern is replaced by the individual letter.

If a letter shows yellow on the second, I know that it exists in another box, so I can remove that letter as an option for the pattern for the second letter.

If a letter is gray, I know that it doesn't exist at all on the solution.

## Acknowledgement

The console application options were inspired by https://github.com/LeoTheBestCoder/wordle-solver