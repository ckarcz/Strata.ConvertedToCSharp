using System;
using System.Collections.Generic;
using System.Collections.Concurrent;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.framework.format
{

	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using StandardId = com.opengamma.strata.basics.StandardId;
	using Currency = com.opengamma.strata.basics.currency.Currency;
	using CurrencyAmount = com.opengamma.strata.basics.currency.CurrencyAmount;
	using AdjustableDate = com.opengamma.strata.basics.date.AdjustableDate;
	using CurrencyParameterSensitivity = com.opengamma.strata.market.param.CurrencyParameterSensitivity;

	/// <summary>
	/// Provides and caches format settings across types.
	/// </summary>
	public class FormatSettingsProvider
	{
	  // TODO extensibility - perhaps drive from properties file

	  /// <summary>
	  /// The default instance.
	  /// </summary>
	  public static readonly FormatSettingsProvider INSTANCE = new FormatSettingsProvider();

	  /// <summary>
	  /// The map of settings by type.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final java.util.Map<Class, FormatSettings<?>> TYPE_SETTINGS = com.google.common.collect.ImmutableMap.builder<Class, FormatSettings<?>>().put(String.class, FormatSettings.of(FormatCategory.TEXT, ValueFormatters.TO_STRING)).put(com.opengamma.strata.basics.currency.Currency.class, FormatSettings.of(FormatCategory.TEXT, ValueFormatters.TO_STRING)).put(com.opengamma.strata.basics.StandardId.class, FormatSettings.of(FormatCategory.TEXT, ValueFormatters.TO_STRING)).put(java.time.LocalDate.class, FormatSettings.of(FormatCategory.DATE, ValueFormatters.TO_STRING)).put(com.opengamma.strata.basics.date.AdjustableDate.class, FormatSettings.of(FormatCategory.DATE, ValueFormatters.ADJUSTABLE_DATE)).put(com.opengamma.strata.basics.currency.CurrencyAmount.class, FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.CURRENCY_AMOUNT)).put(com.opengamma.strata.market.param.CurrencyParameterSensitivity.class, FormatSettings.of(FormatCategory.TEXT, ValueFormatters.CURRENCY_PARAMETER_SENSITIVITY)).put(Double.class, FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.DOUBLE)).put(Short.class, FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.TO_STRING)).put(Integer.class, FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.TO_STRING)).put(Long.class, FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.TO_STRING)).put(double[].class, FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.DOUBLE_ARRAY)).build();
	  private static readonly IDictionary<Type, FormatSettings<object>> TYPE_SETTINGS = ImmutableMap.builder<Type, FormatSettings<object>>().put(typeof(string), FormatSettings.of(FormatCategory.TEXT, ValueFormatters.TO_STRING)).put(typeof(Currency), FormatSettings.of(FormatCategory.TEXT, ValueFormatters.TO_STRING)).put(typeof(StandardId), FormatSettings.of(FormatCategory.TEXT, ValueFormatters.TO_STRING)).put(typeof(LocalDate), FormatSettings.of(FormatCategory.DATE, ValueFormatters.TO_STRING)).put(typeof(AdjustableDate), FormatSettings.of(FormatCategory.DATE, ValueFormatters.ADJUSTABLE_DATE)).put(typeof(CurrencyAmount), FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.CURRENCY_AMOUNT)).put(typeof(CurrencyParameterSensitivity), FormatSettings.of(FormatCategory.TEXT, ValueFormatters.CURRENCY_PARAMETER_SENSITIVITY)).put(typeof(Double), FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.DOUBLE)).put(typeof(Short), FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.TO_STRING)).put(typeof(Integer), FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.TO_STRING)).put(typeof(Long), FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.TO_STRING)).put(typeof(double[]), FormatSettings.of(FormatCategory.NUMERIC, ValueFormatters.DOUBLE_ARRAY)).build();

	  /// <summary>
	  /// The settings cache.
	  /// </summary>
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private final java.util.Map<Class, FormatSettings<?>> settingsCache = new java.util.concurrent.ConcurrentHashMap<>();
	  private readonly IDictionary<Type, FormatSettings<object>> settingsCache = new ConcurrentDictionary<Type, FormatSettings<object>>();

	  /// <summary>
	  /// Creates an instance.
	  /// </summary>
	  protected internal FormatSettingsProvider()
	  {
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains the format settings for a given type.
	  /// </summary>
	  /// @param <T>  the type of the value </param>
	  /// <param name="clazz">  the type to format </param>
	  /// <param name="defaultSettings">  the default settings, used if no settings are found for the type </param>
	  /// <returns> the format settings </returns>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") public <T> FormatSettings<T> settings(Class clazz, FormatSettings<Object> defaultSettings)
	  public virtual FormatSettings<T> settings<T>(Type clazz, FormatSettings<object> defaultSettings)
	  {
		FormatSettings<T> settings = (FormatSettings<T>) settingsCache[clazz];

		if (settings == null)
		{
		  settings = (FormatSettings<T>) TYPE_SETTINGS[clazz];
		  if (settings == null)
		  {
			settings = (FormatSettings<T>) defaultSettings;
		  }
		  settingsCache[clazz] = settings;
		}
		return settings;
	  }

	}

}