/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.observable
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrows;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class QuoteScenarioArrayTest
	public class QuoteScenarioArrayTest
	{

	  private static readonly QuoteScenarioArray ARRAY = QuoteScenarioArray.of(DoubleArray.of(1d, 2d, 3d));

	  public virtual void get()
	  {
		assertThat(ARRAY.get(0)).isEqualTo(1d);
		assertThat(ARRAY.get(1)).isEqualTo(2d);
		assertThat(ARRAY.get(2)).isEqualTo(3d);
		assertThrows(() => ARRAY.get(-1), typeof(System.IndexOutOfRangeException));
		assertThrows(() => ARRAY.get(3), typeof(System.IndexOutOfRangeException));
	  }

	  public virtual void getValues()
	  {
		assertThat(ARRAY.Quotes).isEqualTo(DoubleArray.of(1d, 2d, 3d));
	  }

	  public virtual void getScenarioCount()
	  {
		assertThat(ARRAY.ScenarioCount).isEqualTo(3);
	  }

	}

}