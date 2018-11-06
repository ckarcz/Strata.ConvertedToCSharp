using System;
using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.expression
{

	using Bean = org.joda.beans.Bean;
	using MetaBean = org.joda.beans.MetaBean;

	using Sets = com.google.common.collect.Sets;
	using CalculationFunctions = com.opengamma.strata.calc.runner.CalculationFunctions;
	using Position = com.opengamma.strata.product.Position;
	using PositionInfo = com.opengamma.strata.product.PositionInfo;

	/// <summary>
	/// Evaluates a token against a trade to produce another object.
	/// <para>
	/// This merges the <seealso cref="Position"/> and <seealso cref="PositionInfo"/> objects, giving priority to {@code Position}.
	/// </para>
	/// </summary>
	public class PositionTokenEvaluator : TokenEvaluator<Position>
	{

	  public override Type<Position> TargetType
	  {
		  get
		  {
			return typeof(Position);
		  }
	  }

	  public override ISet<string> tokens(Position position)
	  {
		MetaBean metaBean = MetaBean.of(position.GetType());
		return Sets.union(metaBean.metaPropertyMap().Keys, position.Info.propertyNames());
	  }

	  public override EvaluationResult evaluate(Position position, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

		MetaBean metaBean = MetaBean.of(position.GetType());

		// position
		Optional<string> positionPropertyName = metaBean.metaPropertyMap().Keys.Where(p => p.equalsIgnoreCase(firstToken)).First();
		if (positionPropertyName.Present)
		{
		  object propertyValue = metaBean.metaProperty(positionPropertyName.get()).get((Bean) position);
		  return propertyValue != null ? EvaluationResult.success(propertyValue, remainingTokens) : EvaluationResult.failure("Property '{}' not set", firstToken);
		}

		// position info
		Optional<string> positionInfoPropertyName = position.Info.propertyNames().Where(p => p.equalsIgnoreCase(firstToken)).First();
		if (positionInfoPropertyName.Present)
		{
		  object propertyValue = position.Info.property(positionInfoPropertyName.get()).get();
		  return propertyValue != null ? EvaluationResult.success(propertyValue, remainingTokens) : EvaluationResult.failure("Property '{}' not set", firstToken);
		}

		// not found
		return invalidTokenFailure(position, firstToken);
	  }

	}

}