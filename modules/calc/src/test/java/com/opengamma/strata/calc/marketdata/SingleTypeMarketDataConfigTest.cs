using System.Collections.Generic;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.calc.marketdata
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using ImmutableMap = com.google.common.collect.ImmutableMap;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class SingleTypeMarketDataConfigTest
	public class SingleTypeMarketDataConfigTest
	{

	  public virtual void getValues()
	  {
		IDictionary<string, object> values = ImmutableMap.of("foo", 1, "bar", 2);
		SingleTypeMarketDataConfig configs = SingleTypeMarketDataConfig.builder().configType(typeof(Integer)).configObjects(values).build();

		assertThat(configs.get("foo")).isEqualTo(1);
		assertThat(configs.get("bar")).isEqualTo(2);
		assertThrowsIllegalArg(() => configs.get("baz"), "No configuration found with type java.lang.Integer and name baz");
	  }

	  public virtual void addValue()
	  {
		IDictionary<string, object> values = ImmutableMap.of("foo", 1, "bar", 2);
		SingleTypeMarketDataConfig configs = SingleTypeMarketDataConfig.builder().configType(typeof(Integer)).configObjects(values).build().withConfig("baz", 3);

		assertThat(configs.get("foo")).isEqualTo(1);
		assertThat(configs.get("bar")).isEqualTo(2);
		assertThat(configs.get("baz")).isEqualTo(3);
	  }

	  public virtual void addValueWrongType()
	  {
		assertThrowsIllegalArg(() => SingleTypeMarketDataConfig.builder().configType(typeof(Integer)).build().withConfig("baz", "3"), ".* not of the required type .*");
	  }
	}

}