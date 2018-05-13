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

* Make sure you have the latest [.NET Core](https://dot.net) runtime installed.
* Clone the repository with the tool of your choice.
* Start the SolidSharp.Interactive project

If you don't have or don't want to use an IDE, you can start the interactive command line project from the command line:
````
dotnet run -c Release -p SolidSharp.Interactive
````

### Things you can try

#### Output numbers

The ````n```` method is used to convert a .NET numeric value into a ````SymbolicExpression```` that can be used in symbolic computations.

Most of the time, numeric values will be implicitly converted into ````SymbolicExpression```` values.
However, the C# compiler is unable to proceed to such an implicit conversion with at least some context.
As such, ````1/2```` is the integer ````0````, while ````n(1)/2````, ````1/n(2)```` and ````n(1)/n(2)```` all represents the simple fraction ````1/2```` (one half).

The most basic feature feature of the CLI is outputing the result of the last expression, as can be seen in the examples below:
````
n(0)
````
````
n(42)
````

#### Evaluate trigonometric methods

A decent CAS is supposed to be able to simplify at least basic trigonometric expressions.
Well known values of ````sin(x)```` and ````cos(x)```` will be simplified when possible.

````
sin(0)
````
````
sin(pi/2)
````
````
sin(3*π/2)
````

#### Simplify fractions

Simple fractions greater than 1 in absolute value will always be reduced to the form ````N + P/Q````, where ````N````, ````P```` and ````Q```` are all integers.

````
n(39)/n(11)
````

#### Declare new symbolic variables

You can define new symbolic variables that can be later reused in expressions:

````
var n = var("n"); /* Creates a symbolic variable named n and store it in the C# variable n */
````

#### Use pre-defined variables ````x````, ````y````, ````z```` and ````t````

The CLI host provides predefined symbolic variables that can be used out-of-the box.
This allows to quickly generate expressions making use of unknowns, as in the examples below:

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

#### Substitute variables with other expressions

This feature can be used to evaluate expressions, replacing variables with numeric values or fractions.

````
replace(2 * x + pow(x, 2), x, 98)
````
````
replace(2 * x + pow(x, 2), x, n(1) / n(2))
````
````
replace(2 * x + pow(x, 2), x, n(2) / n(3))
````
````
replace(replace(2 * x + pow(x, 2), x, 3 * x), x, n(2)/n(3))
````
````
replace(3 * pow(x, 2) + 2 * pow(y, 2), y, 2 * x)
````
````
replace(replace(3 * pow(x, 2) + 2 * pow(y, 2), y, 2 * x), x, 7)
````

#### Control the interactive session

##### Exit the command line interface

You can exit the CLI with the traditional Ctrl+C shortcut, or by calling the ````exit```` method.

````
exit()
````

##### Reset the interactive session

Sometimes, you may want to restart with a clean environment. That can be achieved by calling the ````reset```` method.
````
reset()
````
