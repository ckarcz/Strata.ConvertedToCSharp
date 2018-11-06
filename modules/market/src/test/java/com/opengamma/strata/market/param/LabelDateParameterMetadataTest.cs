/*
 * Copyright (C) 2016 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.param
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.assertSerialization;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverBeanEquals;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.TestHelper.date;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="LabelDateParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LabelDateParameterMetadataTest
	public class LabelDateParameterMetadataTest
	{

	  private static readonly LocalDate DATE = date(2015, 7, 30);

	  //-------------------------------------------------------------------------
	  public virtual void test_of_1arg()
	  {
		LabelDateParameterMetadata test = LabelDateParameterMetadata.of(DATE);
		assertEquals(test.Date, DATE);
		assertEquals(test.Label, DATE.ToString());
		assertEquals(test.Identifier, DATE.ToString());
	  }

	  public virtual void test_of_2args()
	  {
		LabelDateParameterMetadata test = LabelDateParameterMetadata.of(DATE, "Label");
		assertEquals(test.Date, DATE);
		assertEquals(test.Label, "Label");
		assertEquals(test.Identifier, "Label");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		LabelDateParameterMetadata test = LabelDateParameterMetadata.of(DATE, "Label");
		coverImmutableBean(test);
		LabelDateParameterMetadata test2 = LabelDateParameterMetadata.of(date(2014, 1, 1), "Label2");
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		LabelDateParameterMetadata test = LabelDateParameterMetadata.of(DATE, "Label");
		assertSerialization(test);
	  }

	}

}