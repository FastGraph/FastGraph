using System;
using System.Collections.Generic;

namespace  QuikGraph.Petri
{
	public interface IConditionExpression<Token>
	{
		bool IsEnabled(IList<Token> tokens);
	}
}
