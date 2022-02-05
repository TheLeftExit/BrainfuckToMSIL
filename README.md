# BrainfuckToIL
Proof of concept Brainfuck-to-IL compiler

[**BrainfuckEmit**](BrainfuckEmit.cs) class provides API to emit specific Brainfuck instructions to an [**ILGenerator**](https://docs.microsoft.com/en-us/dotnet/api/system.reflection.emit.ilgenerator) stream.  
[**Program.cs**](Program.cs) compiles and runs Hello world! written in Brainfuck (taken from its Wikipedia page).
