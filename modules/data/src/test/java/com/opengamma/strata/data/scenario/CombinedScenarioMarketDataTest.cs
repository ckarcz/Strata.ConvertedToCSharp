/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.data.scenario
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertThrowsIllegalArg;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;


	using Test = org.testng.annotations.Test;

	using ImmutableSet = com.google.common.collect.ImmutableSet;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using LocalDateDoubleTimeSeries = com.opengamma.strata.collect.timeseries.LocalDateDoubleTimeSeries;

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class CombinedScenarioMarketDataTest
	public class CombinedScenarioMarketDataTest
	{

	  private static readonly TestId TEST_ID1 = new TestId("1");
	  private static readonly TestId TEST_ID2 = new TestId("2");
	  private static readonly TestId TEST_ID3 = new TestId("3");

	  public virtual void test_combinedWith()
	  {
		LocalDateDoubleTimeSeries timeSeries1 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1).put(date(2011, 3, 9), 2).put(date(2011, 3, 10), 3).build();

		LocalDateDoubleTimeSeries timeSeries2 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 10).put(date(2011, 3, 9), 20).put(date(2011, 3, 10), 30).build();

		LocalDateDoubleTimeSeries timeSeries2a = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 1000).put(date(2011, 3, 9), 2000).put(date(2011, 3, 10), 3000).build();

		LocalDateDoubleTimeSeries timeSeries3 = LocalDateDoubleTimeSeries.builder().put(date(2011, 3, 8), 100).put(date(2011, 3, 9), 200).put(date(2011, 3, 10), 300).build();

		ImmutableScenarioMarketData marketData1 = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addTimeSeries(TEST_ID1, timeSeries1).addTimeSeries(TEST_ID2, timeSeries2).addBox(TEST_ID1, MarketDataBox.ofScenarioValues(1.0, 1.1, 1.2)).addBox(TEST_ID2, MarketDataBox.ofScenarioValues(2.0, 2.1, 2.2)).build();

		ImmutableScenarioMarketData marketData2 = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 10)).addTimeSeries(TEST_ID2, timeSeries2a).addTimeSeries(TEST_ID3, timeSeries3).addBox(TEST_ID2, MarketDataBox.ofScenarioValues(21.0, 21.1, 21.2)).addBox(TEST_ID3, MarketDataBox.ofScenarioValues(3.0, 3.1, 3.2)).build();

		// marketData1 values should be in the combined data when the same ID is present in both
		ImmutableScenarioMarketData expected = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addTimeSeries(TEST_ID1, timeSeries1).addTimeSeries(TEST_ID2, timeSeries2).addTimeSeries(TEST_ID3, timeSeries3).addBox(TEST_ID1, MarketDataBox.ofScenarioValues(1.0, 1.1, 1.2)).addBox(TEST_ID2, MarketDataBox.ofScenarioValues(2.0, 2.1, 2.2)).addBox(TEST_ID3, MarketDataBox.ofScenarioValues(3.0, 3.1, 3.2)).build();

		ScenarioMarketData combined = marketData1.combinedWith(marketData2);
		assertThat(combined).isEqualTo(expected);
		assertThat(combined.Ids).isEqualTo(ImmutableSet.of(TEST_ID1, TEST_ID2, TEST_ID3));
	  }

	  public virtual void test_combinedWithIncompatibleScenarioCount()
	  {
		ImmutableScenarioMarketData marketData1 = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(TEST_ID1, MarketDataBox.ofScenarioValues(1.0, 1.1, 1.2)).build();

		ImmutableScenarioMarketData marketData2 = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(TEST_ID2, MarketDataBox.ofScenarioValues(1.0, 1.1)).build();

		assertThrowsIllegalArg(() => marketData1.combinedWith(marketData2), ".* same number of scenarios .* 3 and 2");
	  }

	  public virtual void test_combinedWithReceiverHasOneScenario()
	  {
		ImmutableScenarioMarketData marketData1 = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(TEST_ID1, MarketDataBox.ofSingleValue(1.0)).build();

		ImmutableScenarioMarketData marketData2 = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(TEST_ID2, MarketDataBox.ofScenarioValues(1.0, 1.1)).build();

		ImmutableScenarioMarketData expected = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(TEST_ID1, MarketDataBox.ofSingleValue(1.0)).addBox(TEST_ID2, MarketDataBox.ofScenarioValues(1.0, 1.1)).build();

		ScenarioMarketData combined = marketData1.combinedWith(marketData2);
		assertThat(combined).isEqualTo(expected);
		assertThat(combined.Ids).isEqualTo(ImmutableSet.of(TEST_ID1, TEST_ID2));
	  }

	  public virtual void test_combinedWithOtherHasOneScenario()
	  {
		ImmutableScenarioMarketData marketData1 = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(TEST_ID2, MarketDataBox.ofScenarioValues(1.0, 1.1)).build();

		ImmutableScenarioMarketData marketData2 = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(TEST_ID1, MarketDataBox.ofSingleValue(1.0)).build();

		ImmutableScenarioMarketData expected = ImmutableScenarioMarketData.builder(LocalDate.of(2011, 3, 8)).addBox(TEST_ID1, MarketDataBox.ofSingleValue(1.0)).addBox(TEST_ID2, MarketDataBox.ofScenarioValues(1.0, 1.1)).build();

		ScenarioMarketData combined = marketData1.combinedWith(marketData2);
		assertThat(combined).isEqualTo(expected);
		assertThat(combined.Ids).isEqualTo(ImmutableSet.of(TEST_ID1, TEST_ID2));
	  }

	  //-------------------------------------------------------------------------
	  private sealed class TestId : ObservableId
	  {

		internal readonly string id;

		internal TestId(string id)
		{
		  this.id = id;
		}

		public StandardId StandardId
		{
			get
			{
			  throw new System.NotSupportedException("getStandardId not implemented");
			}
		}

		public FieldName FieldName
		{
			get
			{
			  throw new System.NotSupportedException("getFieldName not implemented");
			}
		}

		public ObservableSource ObservableSource
		{
			get
			{
			  throw new System.NotSupportedException("getObservableSource not implemented");
			}
		}

		public ObservableId withObservableSource(ObservableSource obsSource)
		{
		  throw new System.NotSupportedException("withObservableSource not implemented");
		}

		public override bool Equals(object o)
		{
		  if (this == o)
		  {
			return true;
		  }
		  if (o == null || this.GetType() != o.GetType())
		  {
			return false;
		  }
		  TestId testId = (TestId) o;
		  return Objects.Equals(id, testId.id);
		}

		public override int GetHashCode()
		{
		  return Objects.hash(id);
		}
	  }

	}

}