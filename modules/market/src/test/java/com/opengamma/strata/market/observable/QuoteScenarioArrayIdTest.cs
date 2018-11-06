/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.observable
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using DoubleArray = com.opengamma.strata.collect.array.DoubleArray;
	using FieldName = com.opengamma.strata.data.FieldName;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;
	using MarketDataBox = com.opengamma.strata.data.scenario.MarketDataBox;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class QuoteScenarioArrayIdTest
	public class QuoteScenarioArrayIdTest
	{

	  private static readonly QuoteScenarioArrayId KEY = QuoteScenarioArrayId.of(StandardId.of("test", "1"), FieldName.of("fieldName"));

	  public virtual void getMarketDataKey()
	  {
		QuoteId quoteId = QuoteId.of(StandardId.of("test", "1"), FieldName.of("fieldName"), ObservableSource.NONE);
		assertThat(KEY.MarketDataId).isEqualTo(quoteId);
		assertThat(QuoteScenarioArrayId.of(quoteId)).isEqualTo(KEY);
	  }

	  public virtual void getMarketDataType()
	  {
		assertThat(KEY.ScenarioMarketDataType).isEqualTo(typeof(QuoteScenarioArray));
	  }

	  public virtual void createScenarioValue()
	  {
		MarketDataBox<double> box = MarketDataBox.ofScenarioValues(1d, 2d, 3d);
		QuoteScenarioArray quotesArray = KEY.createScenarioValue(box, 3);
		assertThat(quotesArray.Quotes).isEqualTo(DoubleArray.of(1d, 2d, 3d));
	  }

	  public virtual void createScenarioValueFromSingleValue()
	  {
		MarketDataBox<double> box = MarketDataBox.ofSingleValue(3d);
		QuoteScenarioArray quotesArray = KEY.createScenarioValue(box, 3);
		assertThat(quotesArray.Quotes).isEqualTo(DoubleArray.of(3d, 3d, 3d));
	  }

	}

}