# Solid♯

A poorly designed attempt at creating a symbolic computation framework geared towards 2D and 3D geometry works.

## Basic idea

The core of the framework are ````SymbolicExpression```` objects that can represent any kind of mathematical expression.
These expressions are designed to be created seamlessly in C#, by relying on operator overloading and the ````SymbolicMath```` class.

The geometry part of the framework will provinde 2D and 3D Vectors and Matrices that are all based on ````SymbolicExpression```` so that geometry can benefit from symbolic computation.

Then, geometry objects will be able to be defined by symbolic equations, represented by the class ````SymbolicEquation````.
