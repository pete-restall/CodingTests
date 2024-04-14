# The Test
This repository is provided as my response to a technical test that I have been presented with.

## The Problem Statement
Set aside 2 hours to create some code that shows how you would code a minefield/minesweeper style game running on the command line (no UI), in order to demonstrate how you would code & test a real-world application using established best practices.

In the game a player navigates from one side of a chessboard grid to the other whilst trying to avoid hidden mines. The player has a number of lives, losing one each time a mine is hit, and the final score is the number of moves taken in order to reach the other side of the board.  The command line / console interface should be simple, allowing the player to input move direction (up, down, left, right) and the game to show the resulting position (e.g. C2 in chess board terminology) along with number of lives left and number of moves taken.

Above all else please follow these guidelines
1. Quality is more important than quantity
2. We will assess your ability to write clean-code that has good structure & is covered by meaningful tests
3. Don’t code a UI

When complete, upload your code to a public GitHub repository and forward the URL to us.
Be prepared to talk through you code and explain key design features and coding principles and why you have used them.
Good luck!

## How I Tackled This
The first step to solving a problem is to understand the problem.  From the text above we can pull out some important features which will define requirements and inform the solution.  The Problem Statement above is not a complete specification, and in the absence of further input, assumptions will have to be made.  For example, the size of the Grid is not specified.

### The Problem Statement Distilled
This is a (Computer) Game.  Games have Players.  Players need a way to enter their Moves and influence the state of the Game.  A feedback mechanism that can inform them of their progress and help them formulate their next Move is also required.

Games typically have a 'Game Loop', similar to a Read-Eval-Print Loop; ie. the Player provides input stimuli (Read), those stimuli are evaluated and the state of the Game is updated / mutated (Eval), before the new state of the Game is fed back to the Player by rendering the frame (Print).  In this case, being a console application for a single Player, the loop will be along the lines of:
1. Read a keypress from the keyboard - for this Game the input can be a simple blocking operation as the Game does not change state without input and there is no need for continual feedback (eg. there are no animations requiring frame updates, nor players or other actors altering state concurrently).
2. Evaluate the keypress - if it is a known key (Up, Down, Left, Right) then attempt to move the Player in the requested direction; bounds checking will be required as will rules around Mines and Lives.  There should also be a key to exit the Game, although this was not explicitly mentioned.
3. Print the current location of the Player in the form of `A1`, `B7`, `C2`, etc.  There is no explicit mention in the Problem Statement of drawing the Grid but I will make the assumption that will provide better feedback to the Player and enhance the Game's appeal.  The number of remaining Lives and the number of Moves taken thus far are also required as outputs.

The Problem Statement also uses the terms Grid and Board somewhat interchangeably - I will rather arbitrarily settle on Grid in the absense of further input from the problem setter.  There is also no mention of the origin or direction for indexing the Grid - it will be assumed that the origin will be the top-left corner; rows are numbered and increment top-to-bottom starting at `1`, whereas columns are lettered and increment left-to-right starting at `A`.

We can see from the Problem Statement that if the Player moves to a Grid square that contains a Mine then the number of Lives needs to be decremented.  The Problem Statement does not mention what happens if the Player steps on the same Mine more than once - the assumption will be that only the first encounter will reduce the number of Lives.

The Problem Statement does not specify what happens when the number of Lives is reduced to zero (or indeed whether it can go below zero).  Nor is the initial number of Lives specified.  These requirements will need to be assumed - it is sensible to terminate the Game when there are no more Lives for the Player, and it is sensible to make the initial number of Lives a configurable parameter as the problem setter may wish to vary the difficulty; the cost of making this particular parameter configurable is neglegible.

There is no mention of the number of Mines or what happens if the Player does not make it from one side of the Grid to the other before the Lives are depleted.  Again, the sensible assumption would be to make the number of Mines configurable, and the obvious behaviour on reaching zero Lives would be to exit the Game.  One potential problem here is the location of the Mines - if the Player is to start in the left-hand column and the number of Mines is configurable, it may be possible that all of the Player's potential initial positions are mined.  There are several ways around this problem, but it is a complication nonetheless - one that has not been specified for.

Because we have made the decision to use a blocking operation for keypresses (for simplicity), it initially seems to make sense to reorder the loop slightly so that the Player can see the initial state of the Grid - Print-Read-Eval.  However, upon completing the game there will be no visual feedback showing the Player's updated position; a classic 'off-by-one' error.  We will keep the initial Read-Eval-Print ordering and instead insert an initial Print step prior to entering the loop.

### The Code
In the absence of any provided coding standards, I will opt for my own style which is based on the guidelines set out in Clean Code but with the norms of C# rather than Java (PascalCase rather than camelCase for method names, for example).

The Problem Statement is instructing me to set aside 2 hours and the emphasis is on quality and not quantity.  I will be adding to the repository as time allows over a period of several days but I do not envisage the exercise will be completed within 2 hours due to various other commitments.  Hopefully there is sufficient here to give a fair impression of my work but there is also a handful of other hardware, software and firmware examples in my personal GitHub repositories [here](https://github.com/pete-restall) and [here](https://githum.com/lophtware).

Convention-based Dependency Injection will be leveraged based on Scrutor - this should prevent the Big Ball of Mud that the default Microsoft DI framework encourages.  This application is not envisaged to be large enough to properly benefit from this but wishing to see an approach to DI was mentioned in correspondence outside of the Problem Statement.

The code will be written in a Top-Down fashion, ie. starting at the program entry-point (`Program.cs`) and elaborating into the details.  Although the analysis above shows a general solution centred around a Game Loop, the actual direction of travel when writing the code will be dictated by the tests, ie. TDD (Red-Green-Refactor).

#### The Skeleton Project
The [initial commit](https://github.com/pete-restall/CodingTests/commit/1b75f5eccbac275d8a436e229cff1bc2f588316b) is a skeleton project with one real test of note - [CompositionRootTest.ExpectGameLoopCanBeConstructed()](https://github.com/pete-restall/CodingTests/commit/1b75f5eccbac275d8a436e229cff1bc2f588316b#diff-a104468f7230f927ada955df691f90113c1453b4e4416f531d25514a7a4fe5a8).  This test - a partial integration test - ensures that the top-level of the object hierarchy, an `IGameLoop`, can always be created.  This is important because a common source of errors when using a DI framework is that of missing or incorrectly configured dependencies that only become evident once the code has been deployed.  Testing for this from the start provides provides an important sanity check and regression test for all future commits.  It will not prevent all such errors but it gives a lot of confidence that the application at least ought to start up.

For a typical application this is also where I will set up a build pipeline - it is important to have this in place from the start where it is easiest and quickest to construct and can provide continual feedback by running all tests on each commit.  I have not done that for this exercise - please ask if you would like me to elaborate.

The `Program.Main()` method is the entrypoint and is not typically (easily or cleanly) testable in an automated way.  The risk of errors can be mitigated by keeping the method to the minimal amount of code necessary to bootstrap the application, allowing us to approach (but not achieve) 100% code coverage.  Of course 100% coverage is possible but it typically introduces a lot of complexity for very little gain, especially as the tests tend to be brittle and require constant maintenance to evolve as many different parts of the application are changed.

#### Implementation of the First Class
Please see the [second commit](chttps://github.com/pete-restall/CodingTests/ommit/50b841fc02318ad939a1c772586afdce55fb4583) to get a flavour of how I develop software.  The `GameLoop` class was left as a stub (`NotImplementedException`) in the initial commit because the class was required to get the integration test passing.  Implementing the class is the next logical course of action.  To do this the [Minefield.Tests/Unit/GameLoopTest.cs](https://github.com/pete-restall/CodingTests/commit/50b841fc02318ad939a1c772586afdce55fb4583#diff-8cbe7896996cf22ef7b824536c3c296c4d316e4e662a1b461326e71ae8627581) was created, and an implementation constructed following the 'Red-Green-Refactor' process of TDD.

There is a lot that I can say regarding how these tests have been developed - the introduction of non-determinism to reduce the number of tests that need to be written and to test a broader range of inputs, for example.  Or the use of fluent assertions and explicitly calling out which Test Doubles are Mocks, Stubs and Dummies to aid the comprehension of the tests.  Please ask if you would like to discuss these details - I will not document them here for brevity.

From the [second commit](https://github.com/pete-restall/CodingTests/commit/50b841fc02318ad939a1c772586afdce55fb4583) you can also see that there are two further stubs that have been created - `GameLoopIteration` and `FrameRenderer`, both of which are throwing `NotImplementedExceptions`.  The next commit will tackle one of these classes and provide an implementation, much like this commit provided the implementation for the `GameLoop` stub.  This process will be followed until the program has been completed.  This second commit illustrates the way in which writing the tests first is a powerful technique to drive the design and development of an application in an incremental manner.

## Conclusion
I have spent more than the alotted time on this test but I do not know by how much - I have tackled it piecemeal over the last four days as time allowed.  The solution is far from complete but there is an extensible Game Loop that can respond to the pressing of the `Q` key to quit and the `Up Arrow` key to trigger evaluation of the 'Move Player Up' command, which has not been written yet.  If I was to continue elaborating this solution then the movement of the Player around the Grid would be the next part to be implemented, no doubt necessitating the creation of classes such as `Grid` and `Coordinates` and `Player` and so on.

Even though there is little functionality of the game itself, there is enough interesting material here that can be discussed further and more in-depth.  For instance, part of my approach to problem solving and software development has been documented.  The code
* is SOLID
* demonstrates Inversion of Control (note the direction of the namespace dependencies for all but the Composition Root, for example, and the interfaces living with their clients and not their implementations)
* demonstrates some Dependency Injection with techniques such as convention-based binding
* has been implemented using some common design patterns, such as the Chain of Responsibility and the Decorator patterns
* has been written in a top-down, test-driven manner (TDD)
* follows the guidelines set out in Clean Code, such as method ordering and naming
* is highly cohesive, grouping by features of the domain rather than technical concerns; for example the `MoveUp` namespace would contain a vertical slice of everything needed to perform that movement

I look forward to a more in-depth discussion with yourselves.  Please also look at my [other](https://github.com/pete-restall) GitHub [repositories](https://githum.com/lophtware) for examples of some of my more complete and varied projects.  Thank you.
