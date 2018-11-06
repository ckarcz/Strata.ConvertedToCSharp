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
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="LabelParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class LabelParameterMetadataTest
	public class LabelParameterMetadataTest
	{

	  public virtual void test_of()
	  {
		LabelParameterMetadata test = LabelParameterMetadata.of("Label");
		assertEquals(test.Label, "Label");
		assertEquals(test.Identifier, "Label");
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		LabelParameterMetadata test = LabelParameterMetadata.of("Label");
		coverImmutableBean(test);
		LabelParameterMetadata test2 = LabelParameterMetadata.of("Label2");
		coverBeanEquals(test, test2);
	  }

	  public virtual void test_serialization()
	  {
		LabelParameterMetadata test = LabelParameterMetadata.of("Label");
		assertSerialization(test);
	  }

	}

}