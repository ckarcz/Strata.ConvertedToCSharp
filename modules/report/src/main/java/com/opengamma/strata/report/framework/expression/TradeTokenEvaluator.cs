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
	using MetaBean = org.joda.beans.MetaBean;

	using Sets = com.google.common.collect.Sets;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using Trade = com.opengamma.strata.product.Trade;
	using TradeInfo = com.opengamma.strata.product.TradeInfo;

	/// <summary>
	/// Evaluates a token against a trade to produce another object.
	/// <para>
	/// This merges the <seealso cref="Trade"/> and <seealso cref="TradeInfo"/> objects, giving priority to {@code Trade}.
	/// </para>
	/// </summary>
	public class TradeTokenEvaluator : TokenEvaluator<Trade>
	{

	  public override Type<Trade> TargetType
	  {
		  get
		  {
			return typeof(Trade);
		  }
	  }

	  public override ISet<string> tokens(Trade trade)
	  {
		MetaBean metaBean = MetaBean.of(trade.GetType());
		return Sets.union(metaBean.metaPropertyMap().Keys, trade.Info.propertyNames());
	  }

	  public override EvaluationResult evaluate(Trade trade, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

		MetaBean metaBean = MetaBean.of(trade.GetType());

		// trade
		Optional<string> tradePropertyName = metaBean.metaPropertyMap().Keys.Where(p => p.equalsIgnoreCase(firstToken)).First();

		if (tradePropertyName.Present)
		{
		  object propertyValue = metaBean.metaProperty(tradePropertyName.get()).get((Bean) trade);
		  if (propertyValue == null)
		  {
			return EvaluationResult.failure("Property '{}' not set", firstToken);
		  }
		  return EvaluationResult.success(propertyValue, remainingTokens);
		}

		// trade info
		Optional<string> tradeInfoPropertyName = trade.Info.propertyNames().Where(p => p.equalsIgnoreCase(firstToken)).First();

		if (tradeInfoPropertyName.Present)
		{
		  object propertyValue = trade.Info.property(tradeInfoPropertyName.get()).get();
		  if (propertyValue == null)
		  {
			return EvaluationResult.failure("Property '{}' not set", firstToken);
		  }
		  return EvaluationResult.success(propertyValue, remainingTokens);
		}
		return invalidTokenFailure(trade, firstToken);
	  }

	}

}