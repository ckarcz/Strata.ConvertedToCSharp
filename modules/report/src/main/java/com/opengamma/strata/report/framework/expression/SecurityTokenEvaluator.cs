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
	using Security = com.opengamma.strata.product.Security;
	using SecurityInfo = com.opengamma.strata.product.SecurityInfo;
	using SecurityPriceInfo = com.opengamma.strata.product.SecurityPriceInfo;

	/// <summary>
	/// Evaluates a token against a security to produce another object.
	/// <para>
	/// This merges the <seealso cref="Security"/>, <seealso cref="SecurityInfo"/> and <seealso cref="SecurityPriceInfo"/>
	/// objects, giving priority to {@code Security}.
	/// </para>
	/// </summary>
	public class SecurityTokenEvaluator : TokenEvaluator<Security>
	{

	  public override Type<Security> TargetType
	  {
		  get
		  {
			return typeof(Security);
		  }
	  }

	  public override ISet<string> tokens(Security security)
	  {
		MetaBean metaBean = MetaBean.of(security.GetType());
		return Sets.union(Sets.union(metaBean.metaPropertyMap().Keys, security.Info.propertyNames()), security.Info.PriceInfo.propertyNames());
	  }

	  public override EvaluationResult evaluate(Security security, CalculationFunctions functions, string firstToken, IList<string> remainingTokens)
	  {

		MetaBean metaBean = MetaBean.of(security.GetType());

		// security
		Optional<string> securityPropertyName = metaBean.metaPropertyMap().Keys.Where(p => p.equalsIgnoreCase(firstToken)).First();
		if (securityPropertyName.Present)
		{
		  object propertyValue = metaBean.metaProperty(securityPropertyName.get()).get((Bean) security);
		  return propertyValue != null ? EvaluationResult.success(propertyValue, remainingTokens) : EvaluationResult.failure("Property '{}' not set", firstToken);
		}

		// security info
		Optional<string> securityInfoPropertyName = security.Info.propertyNames().Where(p => p.equalsIgnoreCase(firstToken)).First();
		if (securityInfoPropertyName.Present)
		{
		  object propertyValue = security.Info.property(securityInfoPropertyName.get()).get();
		  return propertyValue != null ? EvaluationResult.success(propertyValue, remainingTokens) : EvaluationResult.failure("Property '{}' not set", firstToken);
		}

		// security price info
		Optional<string> securityPriceInfoPropertyName = security.Info.PriceInfo.propertyNames().Where(p => p.equalsIgnoreCase(firstToken)).First();
		if (securityPriceInfoPropertyName.Present)
		{
		  object propertyValue = security.Info.PriceInfo.property(securityPriceInfoPropertyName.get()).get();
		  return propertyValue != null ? EvaluationResult.success(propertyValue, remainingTokens) : EvaluationResult.failure("Property '{}' not set", firstToken);
		}

		// not found
		return invalidTokenFailure(security, firstToken);
	  }

	}

}