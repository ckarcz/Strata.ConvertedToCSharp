using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using Bean = org.joda.beans.Bean;
	using Property = org.joda.beans.Property;

	using HashMultiset = com.google.common.collect.HashMultiset;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Iterables = com.google.common.collect.Iterables;
	using Multiset = com.google.common.collect.Multiset;
	using Ints = com.google.common.primitives.Ints;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using PayReceive = com.opengamma.strata.product.common.PayReceive;
	using SwapLeg = com.opengamma.strata.product.swap.SwapLeg;
	using SwapLegType = com.opengamma.strata.product.swap.SwapLegType;

	/// <summary>
	/// Evaluates a token against an iterable object and returns a value.
	/// <para>
	/// The token can be the index of the item in the iterable (zero based). For example, this expression selects
	/// the start date of the first leg of a swap:
	/// <pre>
	///   Product.legs.0.startDate
	/// </pre>
	/// It is also possible to select items based on the value of their properties. For example, <seealso cref="SwapLeg"/> has
	/// a property {@code payReceive} whose value can be {@code PAY} or {@code RECEIVE}. It is possible to select
	/// a leg based on the value of this property:
	/// <pre>
	///   Product.legs.pay.startDate     // Pay leg start date
	///   Product.legs.receive.startDate // Receive leg start date
	/// </pre>
	/// The comparison between property values and expression values is case-insensitive.
	/// </para>
	/// <para>
	/// This works for any property where each item has a unique value. For example, consider a cross-currency swap where
	/// one leg has the currency USD and the other has the currency GBP:
	/// <pre>
	///   Product.legs.USD.startDate // USD leg start date
	///   Product.legs.GBP.startDate // GBP leg start date
	/// </pre>
	/// If both legs have the same currency it would obviously not be possible to use the currency to select a leg.
	/// </para>
	/// </summary>
	public class IterableTokenEvaluator : TokenEvaluator<IEnumerable<JavaToDotNetGenericWildcard>>
	{

	  private static readonly ISet<Type> SUPPORTED_FIELD_TYPES = ImmutableSet.of(typeof(Currency), typeof(SwapLegType), typeof(PayReceive));

	  public override Type TargetType
	  {
		  get
		  {
			return typeof(System.Collections.IEnumerable);
		  }
	  }

	  public override ISet<string> tokens<T1>(IEnumerable<T1> iterable)
	  {
		Multiset<string> tokens = HashMultiset.create();
		int index = 0;

		foreach (object item in iterable)
		{
		  tokens.add((index++).ToString());
		  tokens.addAll(fieldValues(item));
		}
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return tokens.Where(token => tokens.count(token) == 1).collect(toImmutableSet());
	  }

	  public override EvaluationResult evaluate<T1>(IEnumerable<T1> iterable, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

		string token = firstToken.ToLower(Locale.ENGLISH);
		int? index = Ints.tryParse(token);

		if (index != null)
		{
		  try
		  {
			return EvaluationResult.success(Iterables.get(iterable, index), remainingTokens);
		  }
		  catch (System.IndexOutOfRangeException)
		  {
			return invalidTokenFailure(iterable, token);
		  }
		}
		ISet<string> tokens = this.tokens(iterable);

		foreach (object item in iterable)
		{
		  if (!fieldValues(item).Contains(token))
		  {
			continue;
		  }
		  if (!tokens.Contains(token))
		  {
			return ambiguousTokenFailure(iterable, token);
		  }
		  return EvaluationResult.success(item, remainingTokens);
		}
		return invalidTokenFailure(iterable, token);
	  }

	  //-------------------------------------------------------------------------
	  private ISet<string> fieldValues(object @object)
	  {
		if (!(@object is Bean))
		{
		  return ImmutableSet.of();
		}
		Bean bean = (Bean) @object;
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		return bean.propertyNames().Select(bean.property).Where(p => SUPPORTED_FIELD_TYPES.Contains(p.metaProperty().propertyType())).Select(Property.get).Where(v => v != null).Select(object.toString).Select(v => v.ToLower(Locale.ENGLISH)).collect(toImmutableSet());
	  }

	}

}