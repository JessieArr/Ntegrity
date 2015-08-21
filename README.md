# Ntegrity
A library of tools for tracking and controlling changes to your .NET assembly's public interface!

Ntegrity allows you to generate a human-readable and text-diff'able representation of a .NET assembly's interface. This allows you to track changes to your interface in version control, and use historical diffs to create a full list of everything that has changed in your library since a given point in history.

Future features will include: support for semantic diff'ing between two representations of an assembly to show added/changed/removed classes, properties etc. In addition, there will be support for detecting changes that violate SemVer best practices, and example unit tests/T4 templates that allow you to better enforce that your development environment takes advantage of Ntegrity!

Curious? You can check out some sample output from the tool [here!](https://github.com/JessieArr/Ntegrity/blob/master/Ntegrity.Test/SampleOutput/TestTargetAssembly.Interface.txt)
