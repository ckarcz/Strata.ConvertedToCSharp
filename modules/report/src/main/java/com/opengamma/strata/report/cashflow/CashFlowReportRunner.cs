using System.Collections.Generic;
using System.Text;
using System.Globalization;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.report.cashflow
{
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableList;
//JAVA TO C# CONVERTER TODO TASK: This Java 'import static' statement cannot be converted to C#:
//	import static com.opengamma.strata.collect.Guavate.toImmutableSet;


	using ImmutableList = com.google.common.collect.ImmutableList;
	using ImmutableMap = com.google.common.collect.ImmutableMap;
	using ImmutableTable = com.google.common.collect.ImmutableTable;
	using Maps = com.google.common.collect.Maps;
	using Column = com.opengamma.strata.calc.Column;
	using Messages = com.opengamma.strata.collect.Messages;
	using Result = com.opengamma.strata.collect.result.Result;
	using ExplainKey = com.opengamma.strata.market.explain.ExplainKey;
	using ExplainMap = com.opengamma.strata.market.explain.ExplainMap;
	using Measures = com.opengamma.strata.measure.Measures;

	/// <summary>
	/// Report runner for cash flow reports.
	/// </summary>
	public sealed class CashFlowReportRunner : ReportRunner<CashFlowReportTemplate>
	{

	  // TODO - when the cashflow report INI file supports specific columns, the following maps should
	  // be represented by a built-in report template INI file.

	  /// <summary>
	  /// The single shared instance of this report runner.
	  /// </summary>
	  public static readonly CashFlowReportRunner INSTANCE = new CashFlowReportRunner();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final com.opengamma.strata.market.explain.ExplainKey<?> INTERIM_AMOUNT_KEY = com.opengamma.strata.market.explain.ExplainKey.of("InterimAmount");
	  private static readonly ExplainKey<object> INTERIM_AMOUNT_KEY = ExplainKey.of("InterimAmount");

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final java.util.Map<com.opengamma.strata.market.explain.ExplainKey<?>, String> HEADER_MAP = com.google.common.collect.ImmutableMap.of(com.opengamma.strata.market.explain.ExplainKey.ENTRY_TYPE, "Flow Type", com.opengamma.strata.market.explain.ExplainKey.ENTRY_INDEX, "Leg Number", com.opengamma.strata.market.explain.ExplainKey.FORECAST_VALUE, "Flow Amount");
	  private static readonly IDictionary<ExplainKey<object>, string> HEADER_MAP = ImmutableMap.of(ExplainKey.ENTRY_TYPE, "Flow Type", ExplainKey.ENTRY_INDEX, "Leg Number", ExplainKey.FORECAST_VALUE, "Flow Amount");

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final java.util.List<com.opengamma.strata.market.explain.ExplainKey<?>> COLUMN_ORDER = com.google.common.collect.ImmutableList.builder<com.opengamma.strata.market.explain.ExplainKey<?>>().add(com.opengamma.strata.market.explain.ExplainKey.ENTRY_TYPE).add(com.opengamma.strata.market.explain.ExplainKey.ENTRY_INDEX).add(com.opengamma.strata.market.explain.ExplainKey.LEG_TYPE).add(com.opengamma.strata.market.explain.ExplainKey.PAY_RECEIVE).add(com.opengamma.strata.market.explain.ExplainKey.PAYMENT_CURRENCY).add(com.opengamma.strata.market.explain.ExplainKey.NOTIONAL).add(com.opengamma.strata.market.explain.ExplainKey.TRADE_NOTIONAL).add(com.opengamma.strata.market.explain.ExplainKey.UNADJUSTED_START_DATE).add(com.opengamma.strata.market.explain.ExplainKey.UNADJUSTED_END_DATE).add(com.opengamma.strata.market.explain.ExplainKey.START_DATE).add(com.opengamma.strata.market.explain.ExplainKey.END_DATE).add(com.opengamma.strata.market.explain.ExplainKey.FIXED_RATE).add(com.opengamma.strata.market.explain.ExplainKey.INDEX).add(com.opengamma.strata.market.explain.ExplainKey.FIXING_DATE).add(com.opengamma.strata.market.explain.ExplainKey.INDEX_VALUE).add(com.opengamma.strata.market.explain.ExplainKey.GEARING).add(com.opengamma.strata.market.explain.ExplainKey.SPREAD).add(com.opengamma.strata.market.explain.ExplainKey.WEIGHT).add(com.opengamma.strata.market.explain.ExplainKey.COMBINED_RATE).add(com.opengamma.strata.market.explain.ExplainKey.PAY_OFF_RATE).add(com.opengamma.strata.market.explain.ExplainKey.UNADJUSTED_PAYMENT_DATE).add(com.opengamma.strata.market.explain.ExplainKey.PAYMENT_DATE).add(com.opengamma.strata.market.explain.ExplainKey.ACCRUAL_DAYS).add(com.opengamma.strata.market.explain.ExplainKey.ACCRUAL_DAY_COUNT).add(com.opengamma.strata.market.explain.ExplainKey.ACCRUAL_YEAR_FRACTION).add(com.opengamma.strata.market.explain.ExplainKey.COMPOUNDING).add(com.opengamma.strata.market.explain.ExplainKey.UNIT_AMOUNT).add(INTERIM_AMOUNT_KEY).add(com.opengamma.strata.market.explain.ExplainKey.FORECAST_VALUE).add(com.opengamma.strata.market.explain.ExplainKey.DISCOUNT_FACTOR).add(com.opengamma.strata.market.explain.ExplainKey.PRESENT_VALUE).build();
	  private static readonly IList<ExplainKey<object>> COLUMN_ORDER = ImmutableList.builder<ExplainKey<object>>().add(ExplainKey.ENTRY_TYPE).add(ExplainKey.ENTRY_INDEX).add(ExplainKey.LEG_TYPE).add(ExplainKey.PAY_RECEIVE).add(ExplainKey.PAYMENT_CURRENCY).add(ExplainKey.NOTIONAL).add(ExplainKey.TRADE_NOTIONAL).add(ExplainKey.UNADJUSTED_START_DATE).add(ExplainKey.UNADJUSTED_END_DATE).add(ExplainKey.START_DATE).add(ExplainKey.END_DATE).add(ExplainKey.FIXED_RATE).add(ExplainKey.INDEX).add(ExplainKey.FIXING_DATE).add(ExplainKey.INDEX_VALUE).add(ExplainKey.GEARING).add(ExplainKey.SPREAD).add(ExplainKey.WEIGHT).add(ExplainKey.COMBINED_RATE).add(ExplainKey.PAY_OFF_RATE).add(ExplainKey.UNADJUSTED_PAYMENT_DATE).add(ExplainKey.PAYMENT_DATE).add(ExplainKey.ACCRUAL_DAYS).add(ExplainKey.ACCRUAL_DAY_COUNT).add(ExplainKey.ACCRUAL_YEAR_FRACTION).add(ExplainKey.COMPOUNDING).add(ExplainKey.UNIT_AMOUNT).add(INTERIM_AMOUNT_KEY).add(ExplainKey.FORECAST_VALUE).add(ExplainKey.DISCOUNT_FACTOR).add(ExplainKey.PRESENT_VALUE).build();

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private static final java.util.List<com.opengamma.strata.market.explain.ExplainKey<?>> INHERITED_KEYS = com.google.common.collect.ImmutableList.builder<com.opengamma.strata.market.explain.ExplainKey<?>>().add(com.opengamma.strata.market.explain.ExplainKey.ENTRY_INDEX).add(com.opengamma.strata.market.explain.ExplainKey.LEG_TYPE).add(com.opengamma.strata.market.explain.ExplainKey.PAY_RECEIVE).build();
	  private static readonly IList<ExplainKey<object>> INHERITED_KEYS = ImmutableList.builder<ExplainKey<object>>().add(ExplainKey.ENTRY_INDEX).add(ExplainKey.LEG_TYPE).add(ExplainKey.PAY_RECEIVE).build();

	  // restricted constructor
	  private CashFlowReportRunner()
	  {
	  }

	  //-------------------------------------------------------------------------
	  public ReportRequirements requirements(CashFlowReportTemplate reportTemplate)
	  {
		return ReportRequirements.of(Column.of(Measures.EXPLAIN_PRESENT_VALUE));
	  }

	  public Report runReport(ReportCalculationResults calculationResults, CashFlowReportTemplate reportTemplate)
	  {

		int tradeCount = calculationResults.CalculationResults.RowCount;
		if (tradeCount == 0)
		{
		  throw new System.ArgumentException("Calculation results is empty");
		}
		if (tradeCount > 1)
		{
		  throw new System.ArgumentException(Messages.format("Unable to show cashflow report for {} trades at once. " + "Please filter the portfolio to a single trade.", tradeCount));
		}

		int columnIdx = calculationResults.Columns.IndexOf(Column.of(Measures.EXPLAIN_PRESENT_VALUE));
		if (columnIdx == -1)
		{
		  throw new System.ArgumentException(Messages.format("Unable to find column for required measure '{}' in calculation results", Measures.EXPLAIN_PRESENT_VALUE));
		}

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.collect.result.Result<?> result = calculationResults.getCalculationResults().get(0, columnIdx);
		Result<object> result = calculationResults.CalculationResults.get(0, columnIdx);
		if (result.Failure)
		{
		  throw new System.ArgumentException(Messages.format("Failure result found for required measure '{}': {}", Measures.EXPLAIN_PRESENT_VALUE, result.Failure.Message));
		}
		ExplainMap explainMap = (ExplainMap) result.Value;

		return runReport(explainMap, calculationResults.ValuationDate);
	  }

	  private Report runReport(ExplainMap explainMap, LocalDate valuationDate)
	  {
		IList<ExplainMap> flatMap = flatten(explainMap);

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: java.util.List<com.opengamma.strata.market.explain.ExplainKey<?>> keys = getKeys(flatMap);
		IList<ExplainKey<object>> keys = getKeys(flatMap);
//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		IList<string> headers = keys.Select(this.mapHeader).collect(toImmutableList());
		ImmutableTable<int, int, object> data = getData(flatMap, keys);

		return CashFlowReport.builder().runInstant(Instant.now()).valuationDate(valuationDate).columnKeys(keys).columnHeaders(headers).data(data).build();
	  }

	  private IList<ExplainMap> flatten(ExplainMap explainMap)
	  {
		IList<ExplainMap> flattenedMap = new List<ExplainMap>();
		flatten(explainMap, false, ImmutableMap.of(), Maps.newHashMap(), 0, flattenedMap);
		return flattenedMap;
	  }

//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @SuppressWarnings("unchecked") private void flatten(com.opengamma.strata.market.explain.ExplainMap explainMap, boolean parentVisible, java.util.Map<com.opengamma.strata.market.explain.ExplainKey<?>, Object> parentRow, java.util.Map<com.opengamma.strata.market.explain.ExplainKey<?>, Object> currentRow, int level, java.util.List<com.opengamma.strata.market.explain.ExplainMap> accumulator)
	  private void flatten<T1, T2>(ExplainMap explainMap, bool parentVisible, IDictionary<T1> parentRow, IDictionary<T2> currentRow, int level, IList<ExplainMap> accumulator)
	  {

		bool hasParentFlow = currentRow.ContainsKey(ExplainKey.FORECAST_VALUE);
		bool isFlow = explainMap.get(ExplainKey.PAYMENT_DATE).Present;
		bool visible = parentVisible || isFlow;

//JAVA TO C# CONVERTER TODO TASK: Most Java stream collectors are not converted by Java to C# Converter:
		ISet<ExplainKey<IList<ExplainMap>>> nestedListKeys = explainMap.Map.Keys.Where(k => explainMap.get(k).get().GetType().IsAssignableFrom(typeof(System.Collections.IList))).Select(k => (ExplainKey<IList<ExplainMap>>) k).collect(toImmutableSet());

		// Populate the base data
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: for (java.util.Map.Entry<com.opengamma.strata.market.explain.ExplainKey<?>, Object> entry : explainMap.getMap().entrySet())
		foreach (KeyValuePair<ExplainKey<object>, object> entry in explainMap.Map.entrySet())
		{
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.market.explain.ExplainKey<?> key = entry.getKey();
		  ExplainKey<object> key = entry.Key;

		  if (nestedListKeys.Contains(key))
		  {
			continue;
		  }
		  if (key.Equals(ExplainKey.FORECAST_VALUE))
		  {
			if (hasParentFlow)
			{
			  // Collapsed rows, so flow must be the same as we already have
			  continue;
			}
			else if (isFlow)
			{
			  // This is first child flow row, so flow is equal to, and replaces, calculated amount
			  currentRow.Remove(INTERIM_AMOUNT_KEY);
			}
		  }
//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: com.opengamma.strata.market.explain.ExplainKey<?> mappedKey = mapKey(key, isFlow);
		  ExplainKey<object> mappedKey = mapKey(key, isFlow);
		  object mappedValue = mapValue(mappedKey, entry.Value, level);
		  if (isFlow)
		  {
			currentRow[mappedKey] = mappedValue;
		  }
		  else
		  {
			if (!currentRow.ContainsKey(mappedKey)) currentRow.Add(mappedKey, mappedValue);
		  }
		}

		// Repeat the inherited entries from the parent row if this row hasn't overridden them
		INHERITED_KEYS.Where(parentRow.containsKey).ForEach(inheritedKey => currentRow.putIfAbsent(inheritedKey, parentRow[inheritedKey]));

		if (nestedListKeys.Count > 0)
		{
		  IList<ExplainMap> nestedListEntries = nestedListKeys.stream().flatMap(k => explainMap.get(k).get().stream()).sorted(this.compareNestedEntries).collect(Collectors.toList());

		  if (nestedListEntries.Count == 1)
		  {
			// Soak it up into this row
			flatten(nestedListEntries[0], visible, currentRow, currentRow, level, accumulator);
		  }
		  else
		  {
			// Add child rows
			foreach (ExplainMap nestedListEntry in nestedListEntries)
			{
			  flatten(nestedListEntry, visible, currentRow, Maps.newHashMap(), level + 1, accumulator);
			}
			// Add parent row after child rows (parent flows are a result of the children)
			if (visible)
			{
			  accumulator.Add(ExplainMap.of(currentRow));
			}
		  }
		}
		else
		{
		  if (visible)
		  {
			accumulator.Add(ExplainMap.of(currentRow));
		  }
		}
	  }

	  private int compareNestedEntries(ExplainMap m1, ExplainMap m2)
	  {
		Optional<LocalDate> paymentDate1 = m1.get(ExplainKey.PAYMENT_DATE);
		Optional<LocalDate> paymentDate2 = m2.get(ExplainKey.PAYMENT_DATE);
		if (paymentDate1.Present && paymentDate1.Present)
		{
		  return paymentDate1.get().compareTo(paymentDate2.get());
		}
		if (!paymentDate2.Present)
		{
		  return paymentDate1.Present ? 1 : 0;
		}
		return -1;
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private com.opengamma.strata.market.explain.ExplainKey<?> mapKey(com.opengamma.strata.market.explain.ExplainKey<?> key, boolean isFlow)
	  private ExplainKey<object> mapKey<T1>(ExplainKey<T1> key, bool isFlow)
	  {
		if (!isFlow && key.Equals(ExplainKey.FORECAST_VALUE))
		{
		  return INTERIM_AMOUNT_KEY;
		}
		return key;
	  }

	  private object mapValue<T1>(ExplainKey<T1> key, object value, int level)
	  {
		if (ExplainKey.ENTRY_TYPE.Equals(key) && level > 0)
		{
		  return humanizeUpperCamelCase((string) value);
		}
		return value;
	  }

	  private string mapHeader<T1>(ExplainKey<T1> key)
	  {
		string header = HEADER_MAP[key];
		if (!string.ReferenceEquals(header, null))
		{
		  return header;
		}
		return humanizeUpperCamelCase(key.Name);
	  }

	  private string humanizeUpperCamelCase(string str)
	  {
		StringBuilder buf = new StringBuilder(str.Length + 4);
		int lastIndex = 0;
		for (int i = 2; i < str.Length; i++)
		{
		  char cur = str[i];
		  char last = str[i - 1];
		  if (char.GetUnicodeCategory(last) == UnicodeCategory.UppercaseLetter && char.GetUnicodeCategory(cur) == UnicodeCategory.LowercaseLetter)
		  {
			buf.Append(str.Substring(lastIndex, (i - 1) - lastIndex)).Append(' ');
			lastIndex = i - 1;
		  }
		}
		buf.Append(str.Substring(lastIndex));
		return buf.ToString();
	  }

//JAVA TO C# CONVERTER WARNING: Java wildcard generics have no direct equivalent in .NET:
//ORIGINAL LINE: private java.util.List<com.opengamma.strata.market.explain.ExplainKey<?>> getKeys(java.util.List<com.opengamma.strata.market.explain.ExplainMap> explainMap)
	  private IList<ExplainKey<object>> getKeys(IList<ExplainMap> explainMap)
	  {
		return explainMap.stream().flatMap(m => m.Map.Keys.stream()).distinct().sorted((k1, k2) => COLUMN_ORDER.IndexOf(k1) - COLUMN_ORDER.IndexOf(k2)).collect(Collectors.toList());
	  }

	  private ImmutableTable<int, int, object> getData<T1>(IList<ExplainMap> flatMap, IList<T1> keys)
	  {
		ImmutableTable.Builder<int, int, object> builder = ImmutableTable.builder();

		for (int rowIdx = 0; rowIdx < flatMap.Count; rowIdx++)
		{
		  ExplainMap rowMap = flatMap[rowIdx];

		  for (int colIdx = 0; colIdx < keys.Count; colIdx++)
		  {
			builder.put(rowIdx, colIdx, rowMap.get(keys[colIdx]));
		  }
		}
		return builder.build();
	  }

	}

}