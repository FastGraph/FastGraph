using System;
using System.Collections.Generic;

namespace  QuikGraph.Petri
{
	public interface IExpression<Token>
	{
		IList<Token> Eval(IList<Token> marking);
	}
}
