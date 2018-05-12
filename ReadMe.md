# Solid♯

A poorly designed attempt at creating a symbolic computation framework geared towards 2D and 3D geometry works.

## Basic idea

The core of the framework are ````SymbolicExpression```` objects that can represent any kind of mathematical expression.
These expressions are designed to be created seamlessly in C#, by relying on operator overloading and the ````SymbolicMath```` class.

The geometry part of the framework will provinde 2D and 3D Vectors and Matrices that are all based on ````SymbolicExpression```` so that geometry can benefit from symbolic computation.

Then, geometry objects will be able to be defined by symbolic equations, represented by the class ````SymbolicEquation````.

## Try it out

Everything is C# / .NET Core, but you can use the Roslyn-powered command line interface to quickly evaluate some expressions.
The interactive shell provides specific helper methods for working with expressions.

### Start an interactive command line shell

* Make sure you have the latest [.nET Core](https://dot.net) runtime installed.
* Clone the repository with the tool of your choice.
* Start the SolidSharp.Interactive project

If you don't have or don't want to use an IDE, you can start the interactive command line project from the command line:
````
dotnet run -c Release -p SolidSharp.Interactive
````

### Things you can try

Output a number:
````
n(0)
````
````
n(42)
````

Evaluate trigonometric methods:
````
sin(0)
````
````
sin(pi/2)
````
````
sin(3*π/2)
````

Simplify fractions:
````
n(39)/n(11)
````

Declare new symbolic variables:
````
var n = var("n"); /* Creates a symbolic variable named n and store it in the C# variable n */
````

Use pre-defined variables ````x````, ````y````, ````z```` and ````t````:
````
3 * x / 2
````
````
sqrt(x * x)
````
````
pow(sqrt(x), 2)
````
````
pow(abs(x), 2)
````
````
pow(x, 2) + pow(y, 2)
````
````
n(2) + n(3) * t - n(1)/n(2)
````