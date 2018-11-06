using System.Collections.Generic;

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
//	import static com.opengamma.strata.collect.TestHelper.coverImmutableBean;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.testng.Assert.assertEquals;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="ParameterMetadata"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class ParameterMetadataTest
	public class ParameterMetadataTest
	{

	  public virtual void test_empty()
	  {
		ParameterMetadata test = ParameterMetadata.empty();
		assertEquals(test.Label, "");
		assertEquals(test.Identifier, "");
	  }

	  public virtual void test_listOfEmpty()
	  {
		IList<ParameterMetadata> test = ParameterMetadata.listOfEmpty(2);
		assertEquals(test.Count, 2);
	  }

	  //-------------------------------------------------------------------------
	  public virtual void coverage()
	  {
		ParameterMetadata test = ParameterMetadata.empty();
		coverImmutableBean(test);
	  }

	  public virtual void test_serialization()
	  {
		ParameterMetadata test = ParameterMetadata.empty();
		assertSerialization(test);
	  }

	}

}