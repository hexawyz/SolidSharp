using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using SolidSharp.Expressions;
using System;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SolidSharp.Interactive
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
			Console.OutputEncoding = Encoding.Unicode;

			var options = CreateScriptOptions();

			var state = await CSharpScript.RunAsync
			(
				"using static SolidSharp.Expressions.SymbolicMath;",
				options,
				new InteractiveHost()
			);

			while (Console.ReadLine() is string line && line.Length > 0)
			{
				state = await state.ContinueWithAsync(line, options);

				switch (state.ReturnValue)
				{
					case SymbolicExpression expr: Console.WriteLine(expr); break;
					case SymbolicEquation eq: Console.WriteLine(eq); break;
					case SymbolicEquationSystem eqs: break;
					case Vector2 v: Console.WriteLine(v); break;
					case Vector3 v: Console.WriteLine(v); break;
				}
			}
        }

		private static ScriptOptions CreateScriptOptions()
		{
			return ScriptOptions.Default
				.AddReferences(typeof(SymbolicMath).Assembly)
				.AddImports("SolidSharp.Expressions")
				.AddImports("SolidSharp.Geometries")
				.AddImports("SolidSharp.Vectors");
		}
    }
}
