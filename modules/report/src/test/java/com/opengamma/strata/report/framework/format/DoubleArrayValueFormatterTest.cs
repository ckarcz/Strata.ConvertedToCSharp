/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static org.assertj.core.api.Assertions.assertThat;

	using Test = org.testng.annotations.Test;

	/// <summary>
	/// Test <seealso cref="DoubleArrayValueFormatter"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class DoubleArrayValueFormatterTest
	public class DoubleArrayValueFormatterTest
	{

	  public virtual void formatForDisplay()
	  {
		double[] array = new double[] {1, 2, 3};
		assertThat(DoubleArrayValueFormatter.INSTANCE.formatForDisplay(array)).isEqualTo("[1.0, 2.0, 3.0]");
	  }

	  public virtual void formatForCsv()
	  {
		double[] array = new double[] {1, 2, 3};
		assertThat(DoubleArrayValueFormatter.INSTANCE.formatForCsv(array)).isEqualTo("[1.0 2.0 3.0]");
	  }
	}

}