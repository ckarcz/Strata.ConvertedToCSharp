using System.Collections.Generic;

/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.measure.fxopt
{

	using FromString = org.joda.convert.FromString;
	using ToString = org.joda.convert.ToString;

	using CalculationTarget = com.opengamma.strata.basics.CalculationTarget;
	using CalculationRules = com.opengamma.strata.calc.CalculationRules;
	using Measure = com.opengamma.strata.calc.Measure;
	using CalculationParameter = com.opengamma.strata.calc.runner.CalculationParameter;
	using EnumNames = com.opengamma.strata.collect.named.EnumNames;
	using NamedEnum = com.opengamma.strata.collect.named.NamedEnum;
	using ScenarioMarketData = com.opengamma.strata.data.scenario.ScenarioMarketData;
	using BlackFxOptionVolatilities = com.opengamma.strata.pricer.fxopt.BlackFxOptionVolatilities;
	using FxSingleBarrierOptionTrade = com.opengamma.strata.product.fxopt.FxSingleBarrierOptionTrade;

	/// <summary>
	/// The method to use for pricing FX single barrier options.
	/// <para>
	/// This provides the ability to use different methods for pricing FX options.
	/// The Black and Trinomial-Tree methods are supported.
	/// </para>
	/// <para>
	/// This enum implements <seealso cref="CalculationParameter"/> and is used by passing it
	/// as an argument to <seealso cref="CalculationRules"/>. It provides the link between the
	/// data that the function needs and the data that is available in <seealso cref="ScenarioMarketData"/>.
	/// </para>
	/// <para>
	/// Implementations of this interface must be immutable.
	/// </para>
	/// </summary>
	public sealed class FxSingleBarrierOptionMethod : NamedEnum, CalculationParameter
	{

	  /// <summary>
	  /// The Black (lognormal) model.
	  /// This uses Black volatilities - <seealso cref="BlackFxOptionVolatilities"/>.
	  /// </summary>
	  public static readonly FxSingleBarrierOptionMethod BLACK = new FxSingleBarrierOptionMethod("BLACK", InnerEnum.BLACK);
	  /// <summary>
	  /// The Trinomial-Tree model.
	  /// This uses Black volatilities based on a smile - <seealso cref="BlackFxOptionVolatilities"/>.
	  /// </summary>
	  public static readonly FxSingleBarrierOptionMethod TRINOMIAL_TREE = new FxSingleBarrierOptionMethod("TRINOMIAL_TREE", InnerEnum.TRINOMIAL_TREE);

	  private static readonly IList<FxSingleBarrierOptionMethod> valueList = new List<FxSingleBarrierOptionMethod>();

	  static FxSingleBarrierOptionMethod()
	  {
		  valueList.Add(BLACK);
		  valueList.Add(TRINOMIAL_TREE);
	  }

	  public enum InnerEnum
	  {
		  BLACK,
		  TRINOMIAL_TREE
	  }

	  public readonly InnerEnum innerEnumValue;
	  private readonly string nameValue;
	  private readonly int ordinalValue;
	  private static int nextOrdinal = 0;

	  private FxSingleBarrierOptionMethod(string name, InnerEnum innerEnum)
	  {
		  nameValue = name;
		  ordinalValue = nextOrdinal++;
		  innerEnumValue = innerEnum;
	  }

	  // helper for name conversions
	  private static readonly com.opengamma.strata.collect.named.EnumNames<FxSingleBarrierOptionMethod> NAMES = com.opengamma.strata.collect.named.EnumNames.of(FxSingleBarrierOptionMethod.class);

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance from the specified name.
	  /// <para>
	  /// Parsing handles the mixed case form produced by <seealso cref="#toString()"/> and
	  /// the upper and lower case variants of the enum constant name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="name">  the name to parse </param>
	  /// <returns> the type </returns>
	  /// <exception cref="IllegalArgumentException"> if the name is not known </exception>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @FromString public static FxSingleBarrierOptionMethod of(String name)
	  public static FxSingleBarrierOptionMethod of(string name)
	  {
		return NAMES.parse(name);
	  }

	  //-------------------------------------------------------------------------
	  public Optional<com.opengamma.strata.calc.runner.CalculationParameter> filter(com.opengamma.strata.basics.CalculationTarget target, com.opengamma.strata.calc.Measure measure)
	  {
		if (target is FxSingleBarrierOptionTrade)
		{
		  return this;
		}
		return null;
	  };

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Returns the formatted name of the type.
	  /// </summary>
	  /// <returns> the formatted string representing the type </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @ToString @Override public String toString()
	  public override string ToString()
	  {
		return NAMES.format(this);
	  }


		public static IList<FxSingleBarrierOptionMethod> values()
		{
			return valueList;
		}

		public int ordinal()
		{
			return ordinalValue;
		}

		public static FxSingleBarrierOptionMethod valueOf(string name)
		{
			foreach (FxSingleBarrierOptionMethod enumInstance in FxSingleBarrierOptionMethod.valueList)
			{
				if (enumInstance.nameValue == name)
				{
					return enumInstance;
				}
			}
			throw new System.ArgumentException(name);
		}
	}

}