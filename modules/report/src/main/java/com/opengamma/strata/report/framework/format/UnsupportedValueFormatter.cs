using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{

	using Messages = com.opengamma.strata.collect.Messages;
	using ValuePathEvaluator = com.opengamma.strata.report.framework.expression.ValuePathEvaluator;

	/// <summary>
	/// Catch-all formatter that outputs the type of the value in angular brackets,
	/// e.g. {@literal <MyCustomType>}, along with details of the valid tokens that could be used.
	/// </summary>
	internal sealed class UnsupportedValueFormatter : ValueFormatter<object>
	{

	  /// <summary>
	  /// The single shared instance of this formatter.
	  /// </summary>
	  internal static readonly UnsupportedValueFormatter INSTANCE = new UnsupportedValueFormatter();

	  // restricted constructor
	  private UnsupportedValueFormatter()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public string formatForCsv(object @object)
	  {
		return Messages.format("<{}>", @object.GetType().Name);
	  }

	  public string formatForDisplay(object @object)
	  {
		ISet<string> validTokens = ValuePathEvaluator.tokens(@object);

		if (validTokens.Count == 0)
		{
		  return Messages.format("<{}> - drilling into this type is not supported", @object.GetType().Name);
		}
		else
		{
		  IList<string> orderedTokens = new List<string>(validTokens);
		  orderedTokens.sort(null);
		  return Messages.format("<{}> - drill down using a field: {}", @object.GetType().Name, orderedTokens);
		}
	  }

	}

}