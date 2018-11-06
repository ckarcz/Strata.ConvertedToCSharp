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
	/// Test <seealso cref="FormatSettingsProvider"/>.
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @Test public class FormatSettingsProviderTest
	public class FormatSettingsProviderTest
	{

	  public virtual void doubleArray()
	  {
		FormatSettingsProvider settingsProvider = FormatSettingsProvider.INSTANCE;
		FormatSettings<object> defaultSettings = FormatSettings.of(FormatCategory.TEXT, ValueFormatters.UNSUPPORTED);
		FormatSettings<double[]> settings = settingsProvider.settings(typeof(double[]), defaultSettings);
		ValueFormatter<double[]> formatter = settings.Formatter;
		double[] array = new double[] {1, 2, 3};

		assertThat(formatter.formatForDisplay(array)).isEqualTo("[1.0, 2.0, 3.0]");
		assertThat(formatter.formatForCsv(array)).isEqualTo("[1.0 2.0 3.0]");
	  }

	}

}