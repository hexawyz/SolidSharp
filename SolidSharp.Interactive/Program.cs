using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using SolidSharp.Expressions;
using System;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SolidSharp.Interactive
{
	internal static class Program
	{
		private static async Task Main(string[] args)
		{
			// This seems to be required for enabling the console to read and write Unicode on Windows. 🙁
			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				Console.OutputEncoding = Encoding.Unicode;
				Console.InputEncoding = Encoding.Unicode;
			}

			var host = new InteractiveHost
			{
				IsNewSessionRequested = true,
			};

			var options = CreateScriptOptions();
			
			ScriptState<object> state = null;

			var cts = CancellationTokenSource.CreateLinkedTokenSource(host.CancellationToken, default);

			Console.WriteLine("Solid# Interactive");
			Console.WriteLine("------------------");
			Console.WriteLine();
			Console.WriteLine("Use the reset() method to reset the session, and the exit() method to quit.");
			Console.WriteLine();
			
			Console.CancelKeyPress += (s, e)
				=> cts.Cancel();

			while (!cts.IsCancellationRequested && ReadLine() is string line)
			{
				if (string.IsNullOrWhiteSpace(line)) continue;

				try
				{
					if (host.IsNewSessionRequested)
					{
						state = await CSharpScript.RunAsync(line, options, host, cancellationToken: cts.Token);
						host.IsNewSessionRequested = false;
					}
					else
					{
						state = await state.ContinueWithAsync(line, options, cancellationToken: cts.Token);
					}
				}
				catch (CompilationErrorException ex)
				{
					PrintErrorMessage(ex.Message);
					continue;
				}

				if (state.Exception != null)
				{
					PrintErrorMessage(state.Exception.ToString());
					continue;
				}

				switch (state.ReturnValue)
				{
					case SymbolicExpression expr: Console.WriteLine(expr); break;
					case SymbolicEquation eq: Console.WriteLine(eq); break;
					case SymbolicEquationSystem eqs: break;
					case Vector2 v: Console.WriteLine(v); break;
					case Vector3 v: Console.WriteLine(v); break;
					case string s: Console.WriteLine(@"""" + s.Replace(@"""", @"""""") + @""""); break;
					default:
						Console.WriteLine(state.ReturnValue.GetType().ToString() + ":");
						Console.WriteLine(state.ReturnValue.ToString());
						break;
					case null: break;
				}
			}
		}

		private static string ReadLine()
		{
			Console.Write(">");
			return Console.ReadLine();
		}

		private static void PrintErrorMessage(string message)
		{
			var oldColor = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Error.WriteLine(message);
			Console.ForegroundColor = oldColor;
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
