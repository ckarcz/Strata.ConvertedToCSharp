using System;
using System.Text;

/*
 * Copyright (C) 2015 - present by OpenGamma Inc. and the OpenGamma group of companies
 *
 * Please see distribution for license.
 */
namespace com.opengamma.strata.market.observable
{

	using ImmutableBean = org.joda.beans.ImmutableBean;
	using JodaBeanUtils = org.joda.beans.JodaBeanUtils;
	using MetaBean = org.joda.beans.MetaBean;
	using TypedMetaBean = org.joda.beans.TypedMetaBean;
	using BeanDefinition = org.joda.beans.gen.BeanDefinition;
	using PropertyDefinition = org.joda.beans.gen.PropertyDefinition;
	using LightMetaBean = org.joda.beans.impl.light.LightMetaBean;

	using StandardId = com.opengamma.strata.basics.StandardId;
	using Index = com.opengamma.strata.basics.index.Index;
	using FieldName = com.opengamma.strata.data.FieldName;
	using ObservableId = com.opengamma.strata.data.ObservableId;
	using ObservableSource = com.opengamma.strata.data.ObservableSource;

	/// <summary>
	/// An identifier used to access the current value of an index.
	/// <para>
	/// This identifier can also be used to access the historic time-series of values.
	/// </para>
	/// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @BeanDefinition(style = "light", cacheHashCode = true) public final class IndexQuoteId implements com.opengamma.strata.data.ObservableId, org.joda.beans.ImmutableBean, java.io.Serializable
	[Serializable]
	public sealed class IndexQuoteId : ObservableId, ImmutableBean
	{
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull") private final com.opengamma.strata.basics.index.Index index;
		private readonly Index index;
	  /// <summary>
	  /// The field name in the market data record that contains the market data item.
	  /// The most common field name is <seealso cref="FieldName#MARKET_VALUE market value"/>.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.data.FieldName fieldName;
	  private readonly FieldName fieldName;
	  /// <summary>
	  /// The source of observable market data.
	  /// </summary>
//JAVA TO C# CONVERTER TODO TASK: Most Java annotations will not have direct .NET equivalent attributes:
//ORIGINAL LINE: @PropertyDefinition(validate = "notNull", overrideGet = true) private final com.opengamma.strata.data.ObservableSource observableSource;
	  private readonly ObservableSource observableSource;

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Obtains an instance used to obtain an observable value of the index.
	  /// <para>
	  /// The field name containing the data is <seealso cref="FieldName#MARKET_VALUE"/> and the market
	  /// data source is <seealso cref="ObservableSource#NONE"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <returns> the identifier </returns>
	  public static IndexQuoteId of(Index index)
	  {
		return new IndexQuoteId(index, FieldName.MARKET_VALUE, ObservableSource.NONE);
	  }

	  /// <summary>
	  /// Obtains an instance used to obtain an observable value of the index.
	  /// <para>
	  /// The market data source is <seealso cref="ObservableSource#NONE"/>.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="fieldName">  the name of the field in the market data record holding the data </param>
	  /// <returns> the identifier </returns>
	  public static IndexQuoteId of(Index index, FieldName fieldName)
	  {
		return new IndexQuoteId(index, fieldName, ObservableSource.NONE);
	  }

	  /// <summary>
	  /// Obtains an instance used to obtain an observable value of the index,
	  /// specifying the source of observable market data.
	  /// </summary>
	  /// <param name="index">  the index </param>
	  /// <param name="fieldName">  the name of the field in the market data record holding the data </param>
	  /// <param name="obsSource">  the source of observable market data </param>
	  /// <returns> the identifier </returns>
	  public static IndexQuoteId of(Index index, FieldName fieldName, ObservableSource obsSource)
	  {
		return new IndexQuoteId(index, fieldName, obsSource);
	  }

	  //-------------------------------------------------------------------------
	  /// <summary>
	  /// Gets the identifier of the data.
	  /// <para>
	  /// This returns an artificial identifier with a scheme of 'OG-Future' and
	  /// a value of the index name.
	  /// 
	  /// </para>
	  /// </summary>
	  /// <returns> the standard identifier </returns>
	  public StandardId StandardId
	  {
		  get
		  {
			return StandardId.of("OG-Index", index.Name);
		  }
	  }

	  public IndexQuoteId withObservableSource(ObservableSource obsSource)
	  {
		return new IndexQuoteId(index, fieldName, obsSource);
	  }

	  public override string ToString()
	  {
		return (new StringBuilder(32)).Append("IndexQuoteId:").Append(index).Append('/').Append(fieldName).Append(observableSource.Equals(ObservableSource.NONE) ? "" : "/" + observableSource).ToString();
	  }

	  //------------------------- AUTOGENERATED START -------------------------
	  /// <summary>
	  /// The meta-bean for {@code IndexQuoteId}.
	  /// </summary>
	  private static readonly TypedMetaBean<IndexQuoteId> META_BEAN = LightMetaBean.of(typeof(IndexQuoteId), MethodHandles.lookup(), new string[] {"index", "fieldName", "observableSource"}, new object[0]);

	  /// <summary>
	  /// The meta-bean for {@code IndexQuoteId}. </summary>
	  /// <returns> the meta-bean, not null </returns>
	  public static TypedMetaBean<IndexQuoteId> meta()
	  {
		return META_BEAN;
	  }

	  static IndexQuoteId()
	  {
		MetaBean.register(META_BEAN);
	  }

	  /// <summary>
	  /// The serialization version id.
	  /// </summary>
	  private const long serialVersionUID = 1L;

	  /// <summary>
	  /// The cached hash code, using the racy single-check idiom.
	  /// </summary>
	  [NonSerialized]
	  private int cacheHashCode;

	  private IndexQuoteId(Index index, FieldName fieldName, ObservableSource observableSource)
	  {
		JodaBeanUtils.notNull(index, "index");
		JodaBeanUtils.notNull(fieldName, "fieldName");
		JodaBeanUtils.notNull(observableSource, "observableSource");
		this.index = index;
		this.fieldName = fieldName;
		this.observableSource = observableSource;
	  }

	  public override TypedMetaBean<IndexQuoteId> metaBean()
	  {
		return META_BEAN;
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the index. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public Index Index
	  {
		  get
		  {
			return index;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the field name in the market data record that contains the market data item.
	  /// The most common field name is <seealso cref="FieldName#MARKET_VALUE market value"/>. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public FieldName FieldName
	  {
		  get
		  {
			return fieldName;
		  }
	  }

	  //-----------------------------------------------------------------------
	  /// <summary>
	  /// Gets the source of observable market data. </summary>
	  /// <returns> the value of the property, not null </returns>
	  public ObservableSource ObservableSource
	  {
		  get
		  {
			return observableSource;
		  }
	  }

	  //-----------------------------------------------------------------------
	  public override bool Equals(object obj)
	  {
		if (obj == this)
		{
		  return true;
		}
		if (obj != null && obj.GetType() == this.GetType())
		{
		  IndexQuoteId other = (IndexQuoteId) obj;
		  return JodaBeanUtils.equal(index, other.index) && JodaBeanUtils.equal(fieldName, other.fieldName) && JodaBeanUtils.equal(observableSource, other.observableSource);
		}
		return false;
	  }

	  public override int GetHashCode()
	  {
		int hash = cacheHashCode;
		if (hash == 0)
		{
		  hash = this.GetType().GetHashCode();
		  hash = hash * 31 + JodaBeanUtils.GetHashCode(index);
		  hash = hash * 31 + JodaBeanUtils.GetHashCode(fieldName);
		  hash = hash * 31 + JodaBeanUtils.GetHashCode(observableSource);
		  cacheHashCode = hash;
		}
		return hash;
	  }

	  //-------------------------- AUTOGENERATED END --------------------------
	}

}