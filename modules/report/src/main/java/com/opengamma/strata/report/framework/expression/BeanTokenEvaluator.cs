using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{

	using Bean = org.joda.beans.Bean;

	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using Iterables = com.google.common.collect.Iterables;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using LegAmount = com.opengamma.strata.market.amount.LegAmount;
	using LegAmounts = com.opengamma.strata.market.amount.LegAmounts;

	/// <summary>
	/// Evaluates a token against a bean to produce another object.
	/// <para>
	/// The token must be the name of one of the properties of the bean and the result is the value of the property.
	/// </para>
	/// <para>
	/// There is special handling of beans with a single property. The name of the property can be omitted from
	/// the expression if the bean only has one property.
	/// </para>
	/// <para>
	/// For example, the bean <seealso cref="LegAmounts"/> has a single property named {@code amounts} containing a list of
	/// <seealso cref="LegAmount"/> instances. The following expressions are equivalent and both return the first amount in the
	/// list. {@code LegInitialNotional} is a measure that produces {@code LegAmounts}.
	/// <pre>
	///   Measures.LegInitialNotional.0
	///   Measures.LegInitialNotional.amounts.0
	/// </pre>
	/// </para>
	/// <para>
	/// If the token matches the property then the default behaviour applies; the property value is returned and
	/// the remaining tokens do not include the property token. If the token doesn't match the property, the property value
	/// is returned but the token isn't consumed. i.e. the remaining tokens returned from <seealso cref="#evaluate"/> include
	/// the first token.
	/// </para>
	/// </summary>
	public class BeanTokenEvaluator : TokenEvaluator<Bean>
	{

	  public override Type<Bean> TargetType
	  {
		  get
		  {
			return typeof(Bean);
		  }
	  }

	  public override ISet<string> tokens(Bean bean)
	  {
		if (bean.propertyNames().size() == 1)
		{
		  string singlePropertyName = Iterables.getOnlyElement(bean.propertyNames());
		  object propertyValue = bean.property(singlePropertyName).get();
		  ISet<string> valueTokens = ValuePathEvaluator.tokens(propertyValue);

		  return ImmutableSet.builder<string>().add(singlePropertyName).addAll(valueTokens).build();
		}
		else
		{
		  return bean.propertyNames();
		}
	  }

	  public override EvaluationResult evaluate(Bean bean, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

		Optional<string> propertyName = bean.propertyNames().Where(p => p.equalsIgnoreCase(firstToken)).First();

		if (propertyName.Present)
		{
		  object propertyValue = bean.property(propertyName.get()).get();

		  return propertyValue != null ? EvaluationResult.success(propertyValue, remainingTokens) : EvaluationResult.failure("No value available for property '{}'", firstToken);
		}
		// The bean has a single property which doesn't match the token.
		// Return the property value without consuming any tokens.
		// This allows skipping over properties when the bean only has a single property.
		if (bean.propertyNames().size() == 1)
		{
		  string singlePropertyName = Iterables.getOnlyElement(bean.propertyNames());
		  object propertyValue = bean.property(singlePropertyName).get();
		  IList<string> tokens = ImmutableList.builder<string>().add(firstToken).addAll(remainingTokens).build();

		  return propertyValue != null ? EvaluationResult.success(propertyValue, tokens) : EvaluationResult.failure("No value available for property '{}'", firstToken);
		}
		return invalidTokenFailure(bean, firstToken);
	  }

	}

}